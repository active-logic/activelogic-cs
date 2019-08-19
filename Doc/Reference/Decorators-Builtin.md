*Source: Active Logic/Decorators - Last Updated: 2019.7.30*

## Built-in decorators

The API ships with several built-in decorators.

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

**Wait**

Wait for a set period of time, then return `failing` until reset.

```cs
status s = Wait(5f) && BeDone();
// or ..
Wait delay = 1f;
status s = delay(5f) && BeDone();
````

*NOTE: `Wait` does not command a subtask.*

**With** [DRAFT]

Use `With` when data needs to be initialized, then re-initialized whenever an associate subtask has completed or failed; this decorator is not thread safe.

Syntax:

```
With()?[ PREREQ ]?[ TASK ]            // (1)
```

Initially, PREREQ is evaluating; PREREQ may represent a status, or an object.
TASK does not evaluate until PREREQ (through repeated invocations) becomes *complete* or non null. Subsequently, only the TASK is evaluated; when TASK status becomes *complete* or *failing* the decorator reverts to its initial state.

Example 1:

```cs
status Move => With()?[ PathFind() ]?[ FollowPath() ];
```

In this example, *Move* alternates between path-finding and path-following.

Example 2:

```cs
status Attack => With()?[ target = DiscoverThreat() ]?[ Shoot(target) ];
```

In this example, a target is acquired. Thereafter, the `Shoot` subtask is executed. Let's compare the above with a similar pattern:

```cs
status Attack => Do(target = DiscoverThreat()) && [ Shoot(target) ];
```

As above, `Shoot(target)` will not execute until a target has been found; using *With*, however, the target will not re-evaluate until *Shoot* has completed or failed.

Use the above syntax when PREREQ represents a time consuming or potentially failing step. Otherwise, use the alternative syntax (below).

Alternative syntax:

```
With()?[ EXP ] % TASK
With()?[ EXP ] + TASK
With()?[ EXP ] - TASK
```

Through the alternative syntax, 'With' always evaluates TASK. Initially, `EXP` is evaluated; subsequently, do not evaluate EXP, unless:

- `%` is used and TASK is complete or failing (re-init on end)
- `+` is used and TASK is complete (re-init on complete)
- `-` is used and TASK is failing (re-init on fail)

*NOTE: This decorator has been drafted; this means that the decorator is not fully tested, and may not behave as expected; use this caution.*
