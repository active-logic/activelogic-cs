# The Reset-on-Resume (RoR) feature

**NOTE: RoR is planned for deprecation; will be replaced with a Reset-on-Exit (RoE) feature**

Reset-on-Resume (RoR) simplifies the management of stateful decorators and composites. While RoR is automated, understanding how it works is useful.

Most decorators manage state. For example the `Once` decorator runs a task, then stores the resulting status. After the subtask has completed or failed, further invocations return the previously stored value without re-iterating the task.

Although you may explicitly `Reset()` a stateful decorator, ordinarily this is not practical.

By default, the reset-on-resume feature will reset stateful decorators which have been inactive for a number of frames *n*. Ideally, *n* should be 1. However because ordered composites drop frames (ISSUE REF), a value between 5 and 10 helps avoid unexpected behavior (see 'leniency').

This feature is supported with `Init` (the `With` decorator), `Delay`, `FrameDelay`, `InOut`, `Latch`, `Once`, `TimeOut` and stateful sequences and selectors.

RoR is not configurable; adding an option for this would make decorator behavior less predictable.

NOTE: If you are not using Unity, reset-on-resume (RoR) requires a time manager; to provide your own time manager, implement `TimeManager` and assign `AbstractDecorator.timeManager`.

## Leniency

Steppers allow you to set a value for the *n* parameter, the so called leniency parameter.

For real time applications, the default of 8 is recommended.

For turn based games and other "step-wise" mecanisms, use a value of 1.

Note: Until #42 is solved, exercise caution when using ordered composites in turn based games.

## Caveats and workarounds

RoR does not handle repeating tasks. If a task seamlessly cycles, RoR-enabled decorators will not reset.

A common case is this:

```cs
status MyTask() => Once()?[ DoSomeInit() ] && A && B;
```

Here the init will skip when cycling `MyTask`. As a workaround, use an ordered composite:

```cs
status MyTask() => Sequence()[
    and ? DoSomeInit() :
    and ? A && B       : repeat
];
```

As an alternative, delegate repeating tasks to a `Task` or `UTask` object. After the task has completed, `Reset()` or `new` the task; newing a delegate task re-allocates memory, but ensures all dirty state (including control state managed by you) is safely re-initialized.
