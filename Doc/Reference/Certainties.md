*Source: Certainties.cs - Last Updated: 2020.10.31*

# Certainties

When the output of a status function is restricted to a subset of values, use certainties.

Below, the `Strike` function would always return true; in lieu of a status, an `action` is used:

```cs
action Strike() => @void;
```

`action` is implicitly convertible to status:

```cs
status Step() => Defend() || Strike();
```

When using an action as left hand in a sequence, use `&`; this is because an action is always *done*, so the right hand must always evaluate; likewise with the `failure` type.

```cs
action  a;  status b;  status c = a & b;
failure a;  status b;  status c = a | b;
```

Functions that return immediately (always `done` or `fail`) should return `bool`.

`loop` is used for tasks that are always `running`. Widening conversions to `status` are not allowed here, but you may extract a status via the `.ever` property.

Likewise, `pending` is used for tasks that never fail and `impending` is used for tasks that never succeed.

The following table summarizes the semantics API:

```
+ --------- + ---------------- + ------------ +
| TYPE      | status constants | Conversions  |
+ --------- + ---------------- + ------------ +
| action    | @void()          | implicit     |
+ --------- + ---------------- + ------------ +
| failure   | @false()         | implicit     |
+ --------- + ---------------- + ------------ +
| loop      | loop.cont()      | .ever        |
+ --------- + ---------------- + ------------ +
| pending   | pending.cont()   | .due         |
|           | pending.done()   |              |
+ --------- + ---------------- + ------------ +
| impending | impending.cont() | .undue       |
|           | impending.fail() |              |
+ --------- + ---------------- + ------------ +
```

Certainties support the active logic calculus. For example, you may invert (`!`) or compose certainties (`&&`, `||`).

Not all types support all operations. For example, you may AND actions but you cannot OR them, because an action is always *done*.

## Deprecation notes

Compared with earlier versions, several APIs have been deprecated. The new certainties API strikes a better balance between usability and type safety, and the `AL_STRICT` definition is no longer supported.
