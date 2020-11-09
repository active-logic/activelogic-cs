# Auto-reset features [beta]

As an example, the `Once` decorator runs a task, then stores the resulting status. After the decorated task has completed or failed, later iterations return the previously stored value without re-iterating the task.

Although you may explicitly `Reset()` a decorator, or its associate `Task/UTask` ordinarily this is not practical, and the library offers convenient alternatives.

The features discussed here are supported by the `Init` (`With`), `Delay` (`Wait`), `FrameDelay`, `InOut`, `Latch`, `Once` and `TimeOut` decorators - along with ordered/mutable sequences and selectors. Other decorators are either stateless, or use an internal reset management policy (such as `Cooldown`).

## Reset criteria

The `with(arg)[ crit ]` syntax (aka 'crit blocks') lets you specify an argument object, aka *criterion*; when the object changes, all decorators within `exp` are reset. Here is an example:

```cs
public status Reach(Vector3 pos) => with(pos)[
    Once()?[Face(pos)]
    && MoveTo(this.transform, pos)
];
```

In this case, an actor face towards a target position before walking to the position. Since walking may reorient the actor, the `Face` function should be called once, until it succeeds. When a new position is selected, the `Once` decorator resets.

In general, use a `with()[]` block wherever stateful decorators are involved.

While the `with()[]` block will reset all traversed decorators (including subfunctions) owned by the calling `Task/UTask`, reset criteria do not cross object boundaries. The argument here is that objects are expected to manage their own state.

## Reset-on-Resume (RoR)

Reset-on-Resume is an automated feature that will reset stateful decorators which have been inactive for one frame or more. This feature is enabled by default (although not recommended, you may disable RoR via `RoR.enabled = false`).

If you are not using Unity, reset-on-resume (RoR) requires a time manager; to provide your own time manager, implement `TimeManager` and assign `AbstractDecorator.timeManager`.

RoR works in combination with other reset strategies (such as reset criteria) to ensure correct behavior. Do not rely exclusively on the RoR feature.

NOTE: *UTask provides a leniency parameter allowing tasks to "drop frames" without resetting decorators; this feature is experimental, and may be removed in the future.*

## Reset-on-Exit (RoE) [PROVISIONAL]

An RoE block will reset decorators enclosed within `roe[ exp ]` when the enclosed expression has succeeded or failed:

```cs
public status Reach(Vector3 pos) => roe[
    Once()?[Face(pos)]
    && MoveTo(this.transform, pos)
];
```

Although RoE blocks save you the effort of picking a criterion, their use is not encouraged. There are edge cases where control navigates away from a task A, while seamlessly pursuing a subtask B. In such cases neither RoE, nor RoR will trigger, and decorators do not reset.

**RoE is a provisional feature, which may be removed in future versions of AL**

## A note of caution

Reset management features are fairly new in Active Logic; if you encounter issues, consider filing bugs and/or encapsulating control in a separate `Task/UTask`:
- `Task.Reset()` ensures all inline (and manually registered) decorators are reset.
- When all else fails, renewing (delete + `new`) an object ensures all control state (including state managed by you) is correctly discarded.
