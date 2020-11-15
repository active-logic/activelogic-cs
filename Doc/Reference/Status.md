*Sources: Status.cs - Last Updated: 2020.11.15*

# Status Reference

In active logic, tasks are implemented as status functions. statuses are three-valued variables; as such, a task may be `failing`, `running` or `complete`.

Repeated invocations of a status function return varying statuses over time.

## Constructors

Avoid status constructors.

- Import `static Active.Raw` and use `done`, `fail` or `cont` instead.
- For logging purposes import `static Active.Status` and use `done(..)`, `fail(..)`, `cont(..)`.
- Where static import is undesirable, use `status.done(..)`, `status.fail(..)`, `status.cont(..)`

For more information about status constants and logging calls, read [Status keywords and logging calls](Constants-and-logging.md).

## Fields & Properties

`failing`, `running` and `complete` indicate whether a status is failing, running, or complete.

## Methods

`static status done()`, `fail()`, `cont()`.
Issue a complete, failing or running status, including caller information.

`status Map(in status failTo, in status contTo, in status doneTo)`
Transform a status value using the specified mapping; use this function when no unary operator fits your needs.

`status AssertComplete()`, `AssertFailing()`, `AssertRunning()`, `AssertPending()`, `AssertImpending()`, `AssertImmediate()`- verify a status; if matching, return the status; otherwise, raise `StatusAssert`.

## Operators

### `||` - Selector

The left hand side (LHS) evaluates first.
- If LHS is `running` or `done`, the right hand side (RHS) does not evaluate and the result of LHS is returned.
- Otherwise, RHS is evaluated and its result is returned.

### `&&` - Sequencer

LHS evaluates first.
- If LHS is `running` or `failing`, RHS does not evaluate and the result of LHS is returned.
- Otherwise, RHS evaluates and its result is returned.

### Parallel combinators

`+` Lenient combinator - Evaluate both subtasks; complete if either did succeed; continue if either is running, else fail.

`*` Strict combinator - Evaluate both subtasks; fail if either did fail; continue if either is running, else succeed.

`%` Neutral combinator - Evaluate both operands and returns the left hand.

### Unary operators

`!` Inverter - Toggles between failing and succeeding states.

`+` Promoter - Transform a failing status into a running status, and a running status into a succeeding status; returns `pending`

`-` Demoter - Transform a succeeding status into a running status, and a running status into a failing status; returns `impending`

`~` Condoner - Transform a failing status into a succeeding status; returns `pending`

## Conversions

`bool` is implicitly convertible to `status`.

You cannot directly obtain a boolean from status (narrowing conversion), however you may obtain a `bool` by querying the `running`, `failing` and `complete` properties of a status.
