*Source: Certainties.cs - Last Updated: 2020.11.15*

# Certainties

When the output of a status function is restricted to a subset of values, use certainties.

Below, the `Strike` function would always return `done`; in lieu of a status, an `action` is used:

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

Return a boolean if a would-be status function will always complete immediately (`true` for `done`, `false` for `failing`).

`loop` represents unending tasks. Widening conversions to `status` are not allowed here, but you may extract a status via the `.ever` property.

Likewise, `pending` is used for tasks that never fail and `impending` is used for tasks that never succeed.

The following table summarizes the semantics API:

```
+ --------- + ---------------- + ------------ +
| TYPE      | constants        | Conversions  |
+ --------- + ---------------- + ------------ +
| action    | @void            | implicit     |
+ --------- + ---------------- + ------------ +
| failure   | @false           | implicit     |
+ --------- + ---------------- + ------------ +
| loop      | forever          | .ever        |
+ --------- + ---------------- + ------------ +
| pending   | pending_cont     | .due         |
|           | pending_done     |              |
+ --------- + ---------------- + ------------ +
| impending | impending_cont   | .undue       |
|           | impending_fail   |              |
+ --------- + ---------------- + ------------ +
```

The above constants are available via `static Active.Raw`; without static import, similar to status constants, use `action.done()`, `failure.fail()`, `loop.cont()`, ... and so forth.

Via `static Active.Status` you may *motivate* the use of a constant as illustrated below (see [Logging](Logging.md) for more information).

```cs
using static Active.Raw;
// Unmotivated use, no logging support (faster)
status Promptly => @void;

using static Active.Status;
// Motivated use of the `@void` constant.
status Promptly() => @void("Done in no time");
// Unmotivated but information is recorded for logging purposes.
status Promptly() => @void();
```

Certainties support the active logic calculus. As an example, you may invert (`!`) or compose certainties (`&&`, `||`).

Not all types support all operations. For example, you may AND actions but you cannot OR them, because an action is always *done*.

## Deprecation notes

Compared with earlier versions, several APIs have been deprecated. The new certainties API strikes a better balance between usability and type safety, and the `AL_STRICT` definition is no longer supported.
