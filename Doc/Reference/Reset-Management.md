# Reset management features [beta]

Most decorators manage state; as an example, the `Once` decorator runs a task, then stores the resulting status. After the task has completed or failed, further iterations return the previously stored value, without re-iterating the task.

You may explicitly `Reset()` a decorator or its associate `Task/UTask`; however, ordinarily this is not practical; reset management features help you manage the decorator lifecycle.

The features discussed here are supported by the `Init` (`With`), `Delay` (`Wait`), `FrameDelay`, `InOut`, `Latch`, `Once` and `TimeOut` decorators - along with ordered/mutable sequences and selectors.

Other decorators are stateless or use an internal reset management policy (such as `Cooldown`).

The features described in this section are available within *object contexts* (subclass `Task` or `UTask`).

## Crit blocks

`with( CRIT )[ EXP ]` lets you specify an argument object (criterion); when the object changes, decorators within `EXP` are reset:

```cs
public status Reach(Vector3 pos) => with(pos)[
    Once()?[ Face(pos) ]
    && MoveTo(this.transform, pos)
];
```

Here, an agent reorient towards a target position before walking to the position. Since walking may rotate the actor (such as with obstacle avoidance or pathfinding), we do not wish to re-iterate `Face(pos)` while moving towards the target. When a new position is selected, however, the `Once` decorator must reset.

Along with `roe` and `reckon` (see below), crit blocks reset all traversed decorators (including subfunctions) within the calling `Task/UTask`; they do not cross object boundaries.

## Reckon blocks

Similar to crit blocks, reckon blocks let you specify a reset condition:

```cs
reckon( COND )[ EXP ];
```

## Reset-on-Exit (RoE) blocks

An RoE block will reset decorators enclosed within `roe[ exp ]` when the enclosed expression has succeeded or failed:

```cs
public status Reach(Vector3 pos) => roe[
    Once()?[Face(pos)]
    && MoveTo(this.transform, pos)
];
```

While RoE blocks may save you the effort of picking a criterion, their use is not encouraged. There are edge cases where control navigates away from a task A, while seamlessly pursuing a subtask B. In such cases RoE will not trigger.

## Reset-on-Resume (RoR)

Reset-on-Resume is an automated feature that will reset stateful decorators which have been inactive for one frame or more. This feature is enabled by default (although not recommended, you may disable RoR globally via `RoR.enabled = false`).

If you are not using Unity, reset-on-resume (RoR) requires a time manager; to provide your own time manager, implement `TimeManager` and assign `AbstractDecorator.timeManager`.

RoR works in combination with other reset strategies (such as reset blocks) to ensure correct behavior; do not rely exclusively on the RoR feature.

NOTE: *UTask provides a leniency parameter allowing tasks to "drop frames" without resetting decorators; this feature is experimental, and may be removed in the future.*

## A note of caution

Reset management features are fairly new in Active Logic; if you encounter issues, consider filing bugs and/or encapsulating control in a separate `Task/UTask`:
- `Task.Reset()` ensures all inline (and manually registered) decorators are reset.
- When all else fails, renewing (delete + `new`) an object ensures all control state (including state managed by you) is reinitialized.
