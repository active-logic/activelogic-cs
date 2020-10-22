*Source: Active Logic/Decorators - Last Updated: 2019.7.30*

# Built-in decorators

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

**Every/Interval**

Evaluates periodically; use as a scheduler.

```cs
status s =  Every(2.5f)?[ Beep() ];
// or...
Interval i = 2.5f;
status s = i.pass?[ Beep() ];
````

*Other constructors*

`Interval()` - Create an interval with period 1 (seconds); fire on start.

`Interval(bool fireOnStart)` - Create an interval with period 1.

`Interval(float period, float offset=0f, bool fireOnStart=true)` - Create an interval

*Parameters*

`bool catchup` - When the time gap is large, apply the subtask repeatedly to catchup (default: disabled)

`float offset` - Time offset to effect the subtask (default: 0)

`period` - The period (default: 1)

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

Evaluate the subtask until it completes or fails. Thereafter, skip evaluation and return the final status (stateful, RoR, thread unsafe).

```cs
status s =  Once()?[ animator.SetTrigger("Strike") ];
// or ..
Once once = new Once();
status s = once.pass?[ animator.SetTrigger("Strike") ];
````

Use the *Once* decorator when re-iterating a task after it has completed is not desired. For example, in order to give an item, an agent first should be holding the item. However this action must not reiterate (or the agent won't ever let go of said item).

Where you have a mostly stateless composite, and wish only a couple of tasks to not re-iterate, prefer this decorator to using an  [ordered composite](OrderedComposites.md); as an example, this:

```cs
public status Throw(Transform item, Vector3 dir) => Sequence()[
    and ? Hold(item) :
    and ? Face(dir) && Play("Throw")
                     + After(0.5f)?[ Impel(that, dir) ] : end];
```

May be written as:

```cs
public status Throw(Transform that, Vector3 dir)
    => Once()?[Hold(that)]
    && Face(dir) && this["Throw"]
                  + After(0.5f)?[ Impel(that, dir) ];
```

**Timeout**

Evaluates the subtask until it completes or fails. If, however, the subtask doesn't complete or fail within the specified time frame, subsequent evaluations are blocked until `Reset()`.

```cs
status s = Timeout(5f)?[ Idle() ];
// or ..
Timeout t = 5f;
status s = t.pass?[ Idle() ];
````

**While and Tie (Drive)**

*While* drives a subtask while a control task is running. With `While`, the subtask is treated as a side effect, and its return state is ignored.

```cs
While( x )?[ y ]
// or ...
Drive @while = new Drive();
status s = @while[ x, crit: false ]?[ y ];
```

Similar to *While*, *Tie* also drives a subtask while a control task is running; however, while the driving task is running, `Tie` returns the value of the subtask.

```cs
Tie( x )?[ y ]
// or ...
Drive @tie = new Drive();
status s = @tie[ x, crit: true ]?[ y ];
```

An example using both:

```cs
// Inside "Vehicle class"
status Navigate => Tie( engine.Run() )?[ Move() ];

// Inside "Engine class"
status Run => Drive( Burn().never )?[ Play("Noise") ];

impending Burn(){
    if(oil > 0){
        oil--; return cont();
    }else{
        return fail(log && "Out of oil");
    }
}
```

In this case:
- While the engine is running, moving is enabled; `Tie` is used because moving may fail whether the engine is running or not.
- Since the engine noise is a side effect, `Drive` is used.
- `Burn` is impending because it never returns the "done" status.

`While` and `Tie` allow a boolean argument as left hand. Often the left hand represents consuming a resource, or refers to a binary state. In the above example, the `Burn()` function may be written as:

```cs
bool Burn(){
    if(oil > 0){
        oil--; return true;
    }else{
        return false;
    }
}
```

Notes:

- The *Tie* decorator (eval rh while running) naturally complements the && (eval rh while succeeding) and || (eval rh while failing) operators. Since C# does not allow yet another short-circuiting operator, tie is implemented as a decorator.
- Although `Drive` is stateless, decorator semantics require an object. Reusing this decorator should be safe and (unlike stateful decorators), placing several `Tie` / `Drive` invocations on the same line of code should be safe.

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
