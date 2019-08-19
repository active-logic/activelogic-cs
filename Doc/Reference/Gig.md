*Source: UGig.cs - Last Updated: 2019.8.13*

# Gigs

The `UGig` base class is a restricted `UTask` with minimal memory and performance overheads; gigs inherit from `MonoBehaviour`; they are intended for subclassing.

To use a `UGig` as root within a stepper (such as `Agent` or `PhysicsAgent`) override the `status Step()` method. To invoke a UGig as a subtask, prefer the implicit notation to `gig.Step()`

```cs
status x = Strike() && myTask;  // instead of `myTask.Step()`
```

To parameterize a gig, prefer indexers to explicit `Step()` variants.

```cs
status x = Strike() && myTask[params];
```

*Note: C# allows overloading indexers with distinct parameter sets; however optional parameters should not be used*

## Class UGig

### Field and Properties

### Methods

`public virtual status Step()`
/ Stub for your step method

`protected action Do(object arg) (params object[] args)`
/ Convert the result of a non void expression to `done`.
Example: `status Step() => Do( myVar = 34 )`

`protected status Pause(float duration)`
/ Pause the underlying stepper for a specified duration (seconds)

`protected status Resume()`
/ Resume execution while paused or suspended

`protected status Suspend()`
/ Suspend the underlying stepper indefinitely

### Type Conversions

`UGig` is implicitly convertible to `status`, and `Func<status>` which is equivalent to calling or retrieving the `Step` function; if `Step` is not implemented, a runtime error occurs.
