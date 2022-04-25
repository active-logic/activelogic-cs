*Source: Gig.cs, UGig.cs - Last Updated: 2021.8.2*

# Gigs

The `Gig` base class is a restricted `Task` with minimal memory and performance overheads; this provides a template for your tasks and behavior trees:

```
public class MyTask : Gig{

    override public status Step
    =>  /* your implementation */

}
```

*NOTE: when using Active Logic, implementing a comprehensive BT as a single class is not uncommon; bearing in mind that a complex status expression is already a BT, good OOP should guide your design.*

To invoke a gig, prefer the implicit notation to `gig.Step()`

```cs
status x = Strike() && myTask;  // instead of `myTask.Step()`
```

To parameterize gigs, prefer indexers to explicit `Step()` variants.

```cs
status x = Strike() && myTask[params];
```

*Note: C# allows overloading indexers with distinct parameter sets; however optional parameters should not be used*

**In Unity** `UGig` inherits from `MonoBehaviour`; to use a gig as root within a stepper (such as `Agent` or `PhysicsAgent`) override its `status Step()` method as above.

## Class Gig/UGig

### Methods

`public virtual status Step()`<br>
Stub for your step method

`protected action Do(params object[] args)`<br>
Convert the result of a non void expression to `done`.
Example: `status Step() => Do( myVar = 34 )`

### Additional methods (UGig)

`protected status Do<T>() where T : UGig`<br>
Retrieve/create and evaluate a component task of type T.

`protected loop Pause(float duration)`<br>
Pause the underlying stepper for a specified duration (seconds)

`protected loop Push(Func<status> φ)`
Execute φ as BT root ('stack φ') until it has completed or failed, then resume running this gig.

`protected status Resume()`<br>
Resume execution while paused or suspended

`protected status Suspend()`<br>
Suspend the underlying stepper indefinitely

### Properties (UGig)

`protected loop this[Func<status> φ]`<br>
Sames as `Push` (see above)

`public bool suspended`<br>
True if the underlying stepper is suspended or paused

### Type Conversions

Gigs are implicitly convertible to `status`, and `Func<status>` which is equivalent to calling or retrieving the `Step` function; if `Step` is not implemented, a runtime error occurs.
