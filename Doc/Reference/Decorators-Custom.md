*Source: Decorator.cs - Last Updated: 2019.7.30*

# Custom decorators

Implementing your own decorators is done in a few steps. We'll rewrite the `CustomCooldown` decorator as a custom decorator.

## 1. Extend Decorator

Create a file named `CustomCooldown.cs`:

```cs
using System;
using Active.Core;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

[Serializable] public class CustomCooldown : Decorator {

}
```

## 2. Declare fields and constructors

We will declare two fields for the decorator: `duration` and `stamp`.

```cs
public float duration = 1f;
float stamp = System.Single.MinValue;
```

We then declare the following constructors:

```cs
public CustomCooldown() {}

public CustomCooldown(float duration){
    this.duration = duration;
}
```

The no-arg constructor will be useful when we delegate creating the decorator to the underlying APIs.

## 3. Provide an indexer

The indexer is used to decide whether the decorator is 'passing' or 'blocking' the evaluation of the target expression.

```cs
public Nullable<Gate> this[float s]{
    get{
        return (time >= stamp + s) ? done() : fail();
    }
}
```

Invoking `done()` or `fail()` will either present the gate or return `null`. Optionally, you may provide additional information for logging purposes, for example, the below variant of `fail()` displays remaining time.

```cs
fail(log && $"[{s + stamp - time:0.0}]");  // instead of `fail()`
```

In the above expression, the square brackets around the time value let the API know that we don't want to stop the debugger or generate a history node whenever the time value is updated.

*Note 1: Using an indexer avoids having to call a separate function whenever we wish to use the decorator. When calling the decorator inline this doesn't matter (because a helper function is used anyways) but still handy everywhere else.*

*Note 2: `Gate` is an intermediate struct used to enable/disable the subtask. As such `decorator[ params ]?[ expression ]` reads "Did the decorator present the gate? if so, evaluate 'expression' and check the result*

## 4. Override `OnStatus`

A decorator must override `OnStatus(status)`, so that it can update its internal state when the status of the target subtask changes; in this case, the cooldown is initially disabled. It starts running after the subtask completes or fails; accordingly, we set the time-stamp:

```cs
override public void OnStatus(status s){
    if(!s.running) stamp = time;
}
```

## 5. Implement the `Reset()` action

The `Reset()` action allows clearing the state of the decorator. A user might call this function if they want to cancel the cooldown. Within a `UTask`, the function is called automatically.

```cs
override public action Reset(){
    stamp = 0;
    return @void;
}
```

## 6. Further integration

If you want to support inlining, there's a little code that needs to be added to each decorator. For an example of how this is done, check `Cooldown.cs` or any other decorator.

## All done!

All said, implementing decorators isn't hard! You may view the complete example [here](../../User/CustomCooldown.cs)
