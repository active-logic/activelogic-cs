*Source: Certainties.cs - Last Updated: 2020.11.15*

# Certainties

When the output of a status function is restricted to a subset of values, use certainties.

Below, the `Strike` function would always return `done`; in lieu of a status, an `action` is used and the `@void` keyword is applied (if `void` were an actual type in C#, we'd be using that).

```cs
action Strike() => @void;
```

Likewise, `loop` represents unending tasks whereas `pending` is used for tasks that never fail and `impending` is used for tasks that never succeed; finally `failure` is used for immediately failing tasks (if a would-be status function never returns `cont`, use `bool`).

`action` and other certainties are convertible to status:

```cs
status Step() => Defend() || Strike();
```

Likewise, `action` is also convertible to `bool`, `pending` and generally, widening conversions are implicitly performed.

The following table summarizes the semantics API:

```
+ --------- + ---------------------- + -------------------------- +
| TYPE      | Keywords (Active.Raw)  | Semantics (Active.Status)  |
+ --------- + ---------------------- + -------------------------- +
| action    | @void                  | @void(..)                  |
+ --------- + ---------------------- + -------------------------- +
| failure   | @false                 | @false(..)                 |
+ --------- + ---------------------- + -------------------------- +
| loop      | forever                | forever(..)                |
+ --------- + ---------------------- + -------------------------- +
| pending   | pending_cont           | pending.cont(..)           |
|           | pending_done           | pending.done(..)           |
+ --------- + ---------------------- + -------------------------- +
| impending | impending_cont         | impending.cont(..)         |
|           | impending_fail         | impending.fail(..)         |
+ --------- + ---------------------- + -------------------------- +
```

Without static import, similar to status constants you may use `action.done()`, `failure.fail()`, `loop.cont()`, ... and so forth.

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

## Certainties and the AL calculus

Certainties support sequences and selectors, and avoid logical errors:

```cs
status  Ride()           => MerryGoesRound() && Disembark();
loop    MerryGoesRound() => ...;
pending Disembark()      => ...;
```

In the above example, `Disembark()` will not be called since `MerryGoesRound()` doesn't ever succeed or fail; in AL this and other logic errors rouse the compiler (CS0217, CS0034).

## Limitations

Inherent to how C# overloads the logical operators, there's a few(\*) corner cases where an operation should be allowed, but isn't (CS0019 a symptom).

In such cases promote either of the offending types:

```cs
@void && forever;           // Does not compile
(pending)@void && forever;  // OK!
(status)@void && forever;   // Also good
@void & forever.ever;       // Put this bunny back in the hat!
```

For readability you may promote `pending` to status via `.due` and `impending` to status via `.undue`.

(\*) *Specifically 10 out of 98.*

## Deprecation note

Compared with earlier versions, several APIs have been deprecated. The new certainties API strikes a better balance between usability and type safety, and the `AL_STRICT` definition is no longer supported.
