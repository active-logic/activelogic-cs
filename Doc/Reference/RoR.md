# The Reset-on-Resume (RoR) feature

Reset-on-Resume (RoR) simplifies the management of stateful decorators and composites.

Active Logic is designed as a lightweight, eventless API. As such you do not receive a notification when a task is interrupted. However with some decorators (such as timers) this may cause issues. In a typical scenario a task is interrupted, then re-entered at a later date. In most cases the programmer's expectation, then, is that the task should reset.

Reset-on-Resume implements this behavior. It is supported by `Init` (the `With` decorator), `Delay`, `FrameDelay`, `InOut`, `Latch`, `Once`, `TimeOut` and stateful sequences and selectors.

By design, RoR is not configurable; providing an option to disable RoR is possible, however this would make decorator behavior less predictable, and there is not a strong case for this.

NOTE: If you are not using Unity, reset-on-resume (RoR) requires a time manager; to provide your own time manager, implement `TimeManager` and assign `AbstractDecorator.timeManager`.
