*Source: Active Logic/Decorators - Last Updated: 2019.7.30*

## Built-in decorators

The API ships with several built-in decorators.

**After/Delay** (Only available in Unity)

Consume the specified delay before evaluating the subtask; after the subtask has completed or failed, reset.

```cs
status s = After(5f)?[ Idle() ];
// or ..
Delay delay = 5f;
status s = delay.pass?[ Idle() ];
````

NOTE: a delay is consumed on a frame basis, whenever control traverses the decorator.

**Cooldown**

Prevents re-iterating a subtask for a specified duration after the subtask has executed or failed;

```cs
status s = Cooldown(5f)?[ Strike() ];
// or...
Cooldown c = 0.1f;
status s = c.pass?[ Strike() ];
```

**Interval**

Evaluates periodically; use as a scheduler.

```cs
status s =  Every(2.5f)?[ Beep() ];
// or...
Interval i = 2.5f;
status s = i.pass?[ Beep() ];
````

**InOut**

Uses paired conditions to determine when to allow or block a subtask. When the first condition is true, InOut 'enters' the subtask and will then continue evaluating said subtask (even if the first condition subsequently fails), until the second condition becomes false.
InOut resets when the subtask succeeds or fails.

```cs
status s =  InOut(d < 5, d > 10)?[ Chase() ];
// or ..
InOut cond = new InOut();
status s = cond[d < 5, d > 10]?[ Beep() ];
````

NOTE: with *InOut*, both conditions evaluate at every iteration, disregarding the status of the target task.

**Latch**

Blocks the subtask until its condition clears; subsequently the subtask evaluates (disregarding the latch condition) until the subtask completes or fails.

```cs
status s =  Latch(rage >= 100)?[ Berserk() ];
// or ..
Latch l = new Latch();
status s = l[ rage >= 100 ]?[ Berserk() ];
````

NOTE: with *Latch*, the condition evaluates at every iteration, even if the latch is 'open'.

**Once**

Evaluates the subtask until it completes or fails. Subsequent evaluations are blocked until `Reset()`.

```cs
status s =  Once()?[ animator.SetTrigger("Strike") ];
// or ..
Once once = new Once();
status s = once.pass?[ animator.SetTrigger("Strike") ];
````

**Timeout**

Evaluates the subtask until it completes or fails. If, however, the subtask doesn't complete or fail within the specified time frame, subsequent evaluations are blocked until `Reset()`.

```cs
status s = Timeout(5f)?[ Idle() ];
// or ..
Timeout t = 5f;
status s = t.pass?[ Idle() ];
````

**With**

Use `With` when data needs to be initialized, then re-initialized whenever an associate subtask has completed or failed.


```cs
var s = With()?[ EXP ] % TASK;
var s = With()?[ EXP ] + TASK;
var s = With()?[ EXP ] - TASK;
// or...
Init cond = new Init();
status s = cond.pass?[ EXP ] % TASK;  // or replace '%' with '+/-'
```

Initially, `EXP` is evaluated; subsequently, do not evaluate EXP, unless:

- `%` is used and TASK is complete or failing (re-init on end)
- `+` is used and TASK is complete (re-init on complete)
- `-` is used and TASK is failing (re-init on fail)

`TASK` is always evaluated.
