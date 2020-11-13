
*Reading time: 10 minutes - Last Updated: 2020.11.13*

# Active Logic quick start guide

Be welcome! Here is a hands-on introduction to Active Logic. Let's get started.

This introduction is engine agnostic; if you are using Unity, read the [Unity Quick Start Guide](QuickStart-Unity.md).

## Status expressions and the update loop

Throughout this guide we'll consider the example of a basic 'soldier' AI or agent. A soldier may attack, defend or retreat; in AL this is modeled using a *status expression*:

```cs
status s = Attack() || Defend() || Retreat();
```

The above realizes a *fallback strategy* (aka *selector*), reading:

```
If possible, attack;
otherwise, defend;
otherwise, retreat.
```

`status` may be `complete`, `failing` or `running`; a value of *running* causes execution to *yield* until the next iteration.

Status expressions are invoked *frequently* - usually, within update loops. As an example, the above may (for testing or simulation purposes) simply run within a `while()` loop:

```cs
using Active.Core;                // Core API.
using static Active.Core.status;  // Static import for 'done()'
                                  // instead of 'status.done()', and
                                  // so forth.

public static class Soldier{

    static status state = cont();

    static void Main(){
        while(state.running){
            state = Attack() || Defend() || Retreat();
        }
    }

    static status Attack(){
        // ... implementation details omitted.
    }

}
```

The above iterates until `state` changes to *failing* or *complete*.

Unlike other BT libraries, the AL calculus fits right within your update loop; your engine/framework will provide its own update loop manager (not a while loop!) and as such a practical implementation may look like this:

```cs
using Active.Core;                
using static Active.Core.status;  

public class Soldier{

    public status Step(){
        return Attack() || Defend() || Retreat();
    }

    // ...

}
```

The update manager will then invoke `Step()` at every frame, perhaps with a policy to determine whether a failing/complete agent should repeat execution or stop.

In AL, `Gig` and `Task` are engine-agnostic base classes you may use to implement this approach. `Gig` only provides basic logging support while `Task` benefits additional features covered later in the guide.

```cs
public class Soldier : Gig{

    override public status Step(){
        return Attack() || Defend() || Retreat();
    }

}
```

When assigning or returning a status, use `done()`, `fail()` or `cont()`:

```cs
status Attack() => hasWeapon ? fail() : Play("Strike");
```

AL does not restrict status expressions to sequences and selectors
(see how we used the ternary conditional `x ? y : z`).

**NOTE**: *status does not convert to `bool`; instead, query the `running`, `failing` and `complete` status properties; booleans do convert to status (where* true *is* done, false *is* failing *).*

Within any expression, terms evaluating to `status` are referred as *tasks* or *subtasks*:

```cs
A && B || (C && D)  // A status expression
```

Visual BT presentation:

```
    ⍈
    |
+---+----+
|        |
A        ⍰
         |
     +---+---+
     |       |   
     B       ⍈
             |
         +---+---+
         |       |
         C       D
```

In the above diagram, `C && D` is a subtask. Following operator precedence, `B || (C && D)` is also a task/subtask.

**NOTE**: *Initially, mixing `&&` and `||` may feel confusing; start with simple status expressions, defining new C# functions for subtasks*

**A key to understanding behavior trees** is that a later task (such as `Retreat`) - does not evaluate before ticking (traversing) prior tasks (in our example, `Attack` and `Defend`); well designed status functions use guard conditions:

```cs
status DoSomething(){
    if( CANNOT_DO    ) return fail();  // guard A
    if( ALREADY_DONE ) return done();  // guard B
    return REALLY_DO_SOMETHING;
}
```

While perhaps surprising, *over-exercising* (ticking a done task) and *over-checking* (ticking a failing task) are integral to BT, ensuring *responsiveness*.

Let's say a soldier dropped their sword. In BT, how would the following selector handle the situation?

```cs
status Attack() => Strike() || EquipWeapon() || PickupWeapon();
```

Gotcha! They will pick up their sword again (or perhaps unsheath a dagger).

If guard conditions are not correctly implemented, pseudo-concurrency may arise, with your BT agents doing several things at once.

*TIP*: *If this seems like a lot of work, do notice that, provided the component tasks (strike, equip, pickup) are correctly implemented, the `Attack()` function itself does not require guard conditions.*

Let's consider another, possible implementation of the `Attack()` function:

```cs
status Attack(){
    if(health < 25) return fail();
    if(!threat)     return fail();
    return MoveTo(threat) && Strike(threat);
}
```

Here, notice the `MoveTo(threat) && Strike(threat)` idiom. The conditional operator AND (`&&`) behaves differently from the conditional OR (`||`). In this case an attack will not complete until *both* `MoveTo` and `Strike` did succeed.

If correctly implemented (using guard conditions), ticking `MoveTo` while within striking range will not interfere with the strike task.

Expressions of the form `EXP_1 && ... && EXP_n` are known as *sequences*; use a sequence when each task is a prerequisite to the following task.

More generally, selectors and sequences are known as *composites*.

In AL, status expressions and the [status calculus](Reference/Status.md) implement *stateless* control. Status expressions and status functions combine to form the behavior tree.

With regard to BT as an established paradigm, **the AL calculus constitutes an orthodox, concise and correct implementation**.

The recommendation here is to familiarize yourself with stateless BT and the AL calculus before diving into decorators and stateful/ordered composites.

## Decorators

In the above example, we have hinted at a `Strike()` task. Effective control does require a variety of small 'utilities' used to modulate behavior. A staple of video game design, the *cooldown* is one such thing:

```cs
status Attack()
    => Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ];
```

This literally is AL magic. Whereas a cooldown should normally require a variable storing a time stamp and the target duration, AL manages this data (aka control state) on your behalf.

`Gig` does not support the above syntax. Decorators and other stateful constructs are availed via the `Task` base class:

```cs
public class Soldier : Task{

    // ...

    status Attack()
        => Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ];

}
```

AL offers several [built-in decorators](Reference/Decorators-Builtin.md) and you may also [craft your own](Reference/Decorators-Custom.md).

**IMPORTANT**: do not invoke the same decorator several times on the same line of code:

```cs
// No good
status Attack()
    => Cooldown(0.5f)?[ Warmup() ] && Cooldown(1.0f)?[ Strike() ];
```

Under the hood, decorators leverage site binding and [null-conditional operators](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-). Site binding in AL is (conceptually) similar to lambdas (anon functions / anon storage); the 'one per line' limitation is a language issue (upvote/discuss [#2824 on csharplang](https://github.com/dotnet/csharplang/discussions/2824)).

Site bound decorators are optional. The alternative (declaring a field in your class) is [documented here](Reference/Decorators.md), and it is also useful if you wish to expose decorator parameters.

## Ordered Composites

Stateless control encourages you to leverage world/agent state as the primary drive for agent behavior. This works well, and often reduces bugs. However some design problems do not fit this approach:

```cs
status Charge() => Taunt(target) && Attack(target);
```

A *taunt* is air. It may instill fear in the enemy's heart, or a thing your game designer want for added emphasis. Either way your model might not be tracking state changes related to uttering warcries.

When an actor are intended to follow a sequence of steps *by design*, use an ordered composite:

```cs
status Charge => Seq()
    + @do?[ Taunt(target) ]
    + @do?[ Reach(target) ];
```

By default an ordered sequence runs until either a failure is encountered, or all tasks have succeeded, thereupon the ordered sequence resets (if you do not wish to reset, use `Seq(repeat: false) + ...`).

Above, the `@do?[ EXP ]` node spans a unique function call. You may embed a stateless selector/sequence or indeed any other status expression.

Similar to decorators, ordered sequences store state, and are made available in a task context (extend `Task`).

Ordered selectors use a similar syntax:

```cs
status Fallback() => Sel()
    - @do?[ EXP_1 ]
    - @do?[ EXP_2 ]
    ... ;
```

**NOTE**: `@do?[ ... ]` nodes are not site-bound so the 'one per line' quirk does not apply, except perhaps for readability.

## Tasks and Gigs

We have already encountered `Task` and `Gig`. In AL, task objects are used in several ways.

When writing a low level module (such as locomotion, or simple actions), you do not override the `Step()` function. Instead you provide a collection of status functions such as `status Walk(...)`,  `status Jump(...)` and so forth... another task will then invoke these functions directly.

When designing higher level behaviors, such as representing *roles* (say `Farmer` or `Soldier`), override `status Step()`. In such cases there is no need to explicitly invoke the step function:

```cs
class Citizen : Gig{  // just a gig since no decorators

    bool employed;
    GateKeeper gateKeeper;  // derived from Task or Gig
    Thief      thief;       // derived from Task or Gig

    // gateKeeper.Step(), thief.Step() via implicit conversion
    override public status Step() => employed ? gateKeeper : thief;

}
```

The above example illustrates designing complex agents by assembling ever larger BTs, combining OOP's delegation pattern with BT's modular control.

## Logging

While the Active Logic logging/history tracing APIs are available in the Github repository, visual logging typically requires engine/editor integration. For an overview of how visual logging works in Unity, refer to the [Unity Quick Start Guide](QuickStart-Unity.md).

**NOTE**: *AL is debugger friendly. Getting productive without visual logging is possible, notably if you are using unit/functional tests.*

## Going further

The quick start guide is intended as a short introduction to AL; to learn more, check the [API reference](Reference/Overview.md).
