*Source: Decorator.cs - Last Updated: 2019.7.30*

# Decorators

This section introduces decorators; builtin decorators are documented [here](Decorators-Builtin.md); read [Custom Decorators](Decorators-Custom.md) if you wish to implement your own.

A decorator modulates the behavior of a target task. Common decorators are implemented Via the `status` class: unary operations let you invert (!), condone (~), promote (+, ++) and demote (-, --) statuses; if this is not enough, use `Status.Map()`.

Beyond, the API provides a set of powerful, easy to use constructs properly known as 'decorators'. Mostly, these decorators follow a common syntax:

```
Decorator( Params ) ? [ StatusExp ]
```

Under the hood, most decorators are *stateful conditionals*:
- They require data storage - be it a cooldown or latch, data is stored to keep track of the current state; for this reason, syntax does vary a little depending on whether you inline decorators (and let the API manage the data on your behalf) or declare it first.
- Depending on how `Decorator( Params )` evaluates, `[ StatusExp ]` may be evaluated, or skip.

## Life cycle

Since decorators are stateful, on occasion it is appropriate to reset their state. In order to reset a decorator, invoke `Reset()`. Oftentimes, however, a decorator's life-cycle is managed on your behalf, and `Reset()` is called implicitly:

- Inline decorators are automatically reset when the underlying task is reset.
- Many decorators automatically reset 'on resume'. A resume occurs when control returns to a previously active execution path.

If you are not using Unity, reset-on-resume requires a time manager; to provide your own time manager, implement `TimeManager` and assign `AbstractDecorator.timeManager`.

## Inline decorators

Within a `Task/UTask`, you may use decorators inline. Let's apply a 0.1 seconds cooldown to a `Fire()` action:

```cs
status s = Cooldown(0.1f)?[ Fire() ];
```

In the above example *Task* (or *UTask*) manage the underlying data (the timestamp) on your behalf. This is convenient, but comes with small memory/performance overheads. You also cannot reset the decorator on its own (but you can reset all decorators associated with a task Via its `Reset()` method).

## Explicit storage

Without a task context (or if you do not wish to use the inline syntax), declare the decorator before use:

```cs
// Field declaration
public Cooldown overheat = 0.1f;
// Anywhere in your code
status s = overheat.pass?[ Fire() ];   // (1)
// or...
status s = overheat[0.2f]?[ Fire() ];  // (2)
```

In the above example, (1) depicts the parameterless syntax; (2) depicts the parameterized syntax; decorators implementing the `OptionalArguments` interface support the parameterless syntax.

Making the decorator public allows customization Via the Unity inspector. You then manage the decorator's life cycle:

1) Manually, by invoking `overheat.Reset()` or `null`-ing the decorator.

2) If you are using `Task` or `UTask`, you can still register the decorator:

```cs
class Gunner
{
    public Cooldown overheat = 0.1f;
    protected void Start() => Register(overheat);
}
```

3) Implicitly, by letting the decorator 'expire' when the owning object is destroyed.
