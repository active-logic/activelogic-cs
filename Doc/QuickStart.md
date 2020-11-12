
*Reading time: 10 minutes - Last Updated: 2020.11.13*

# Active Logic Quick Start Guide

Be welcome! Here is a hands-on introduction to Active Logic. Let's get started.

## Status expressions

First we consider a basic 'soldier' AI. A soldier may attack, defend or retreat; in AL this is modeled using a *status expression*:

```cs
status s = Attack() || Defend() || Retreat();
```

The above realizes a *fallback strategy* (commonly referred to as a *selector*), reading:

```
If possible, attack;
otherwise, defend;
otherwise, retreat.
```

A `status` may be `complete`, `failing` or `running` (for 'continue'). a value of *running* causes execution to *yield* until the next iteration.

Status expressions are invoked *frequently* - usually, within update loops. The next example shows using status expressions within a vanilla `MonoBehaviour` (a base class for behaviors in the Unity game engine):

```cs
using UnityEngine;
// Typical AL imports
using Active.Core;                // Import core APIs
using static Active.Core.status;  // Type 'done()' instead of
                                  // 'status.done()' ...

public class Soldier : MonoBehaviour{

    public status state;

    void Update(){
        state = Attack() || Defend() || Retreat();
        if(!state.running){
            enabled = false;
        }
    }

}
```

The above iterates frame by frame until `s` changes to *failing* or *complete*.

The Active Logic calculus is not restricted to Unity or `MonoBehaviour`, so this may be considered a sketch for your own integration. Indeed, AL does provide powerful base classes advantageously replacing `MonoBehaviour`:

```cs
public class Soldier : UGig{  // or `Gig`

    override public status Step(){
        return Attack() || Defend() || Retreat();
    }

}
```

Note: `UGig` and `UTask` partake the integration available from the [Unity Asset Store](https://assetstore.unity.com/packages/tools/ai/active-logic-151850). If you are using another game engine, `Gig` and `Task` provide a template/starting point for your own integration.

When assigning or returning a status, use `done()`, `fail()` or `cont()`:

```cs
status Attack() => hasWeapon ? fail() : Play("Strike");
```

AL does not restrict status expressions to sequences and selectors: we just used the ternary conditional `x ? y : z`

*Note: status does not convert to bool; query the 'running', 'failing' and 'complete' properties.*

In a status expression, terms evaluating to a status are referred as *tasks* or *subtasks*:

```cs
A && B || (C && D)
```

Equivalent BT:

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

In the above diagram, `C && D` realize a subtask. Following operator precedence, `B || (C && D)` are also a task.

Initially, mixing `&&` and `||` may be confusing. If so, write simple status expressions and create new C# functions for subtasks.

A key to understanding behavior trees is that a later task (such as `Retreat`) - does not get evaluate before traversing prior tasks (in our example, `Attack` and `Defend`). For this reason, a dependable status function often requires guard conditions

```cs
status DoSomething(){
    if( CANNOT_DO    ) return fail();  // guard A
    if( ALREADY_DONE ) return done();  // guard B
    return REALLY_DO_SOMETHING;
}
```

While perhaps surprising, *over-exercising* (traversing a complete task) and *over-checking* (traversing a previously failing) task are integral to BT, ensuring *responsiveness*.

Let's say a soldier dropped their sword. In BT, how would the following sequence handle this?

```cs
status Attack() => Strike() || EquipWeapon() || PickupWeapon();
```

Gotcha! They will just pick up the sword again (or perhaps draw a dagger). This is neat and depends on correctly implementing guard conditions.

Guard conditions work best when you leverage agent/world state (is a weapon equiped? Is it in the inventory? Is there one within a 3 meter range?).

Status expressions implement *stateless* control. In our toy model/example this approach is beneficial: depending on how `Attack()` is implemented, a wounded agent may also receive healing and get back on the offense. Here is another possible implementation of the `Attack()` function:

```cs
status Attack(){
    if(health < 25) return fail();
    if(!threat)     return fail();
    return MoveTo(threat) && Strike(threat);
}
```

Notice the `MoveTo(threat) && Strike(threat)` idiom. The conditional operator AND (`&&`) behaves differently from the conditional OR (`||`). In this case an attack will not complete until *both* `MoveTo` and `Strike` did succeed.

When correctly implemented (using guard conditions as above), ticking (aka traversing) `MoveTo` while within striking range will not interfere with the strike task.

Expressions of the form `EXP_1 && ... && EXP_n` are known as *sequences*; use a sequence when each task is a prerequisite to the following task.

More generally, selectors and sequences are known as *composites*.

In all, status expressions and status functions combine to form behavior trees.

## Decorators

In the above example, we've hinted at a `Strike()` task. Effective control requires a variety of small 'utilities' used to modulate behavior. A staple of video game design, the humble *cooldown*, is one such thing:

```cs
status Attack(){
    // guards omitted for concision ...
    return Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ];
}
```

This literally is AL magic. Normally you'd have to declare a variable to store a time stamp for the cooldown. AL manages this data (aka control state) on your behalf.

`Gig`, `UGig` (and of course `MonoBehaviour`) do not support the above syntax. This only works within a behavior inheriting from `Task` or `UTask` ('stateful context').

The API offers several [built-in decorators](Reference/Decorators-Builtin.md) and you may also [craft your own](Reference/Decorators-Custom.md).

## Ordered Composites

Stateless control encourages you to leverage world/agent state as the primary drive for agent behavior. This works well, and often reduces bugs.

However some design problems do not fit this approach; consider this:

```cs
status s = Taunt(target) && Reach(target);
```

A *taunt* is air. It may instill fear in the enemy's heart, or could be a thing your game designer want for added emphasis. Either way your model may or may not be tracking state changes related to uttering warcries.

When an actor are intended to follow a sequence of steps *by design*, use an ordered composite:

```cs
status s = Seq()
    + @do?[ Taunt(target) ]
    + @do?[ Reach(target) ]
    + /* ... */ ;
```

The above syntax is available in stateful contexts (`Task` or `UTask`). Once again this is because ordered composites use control state (here, an integer tracks the currently running task)

By default, an ordered sequence runs until either a failure is encountered, or all tasks have succeeded. Thereupon the ordered sequence resets.

If you do not wish the sequence to reset, use `Seq(repeat: false)`

In the above example the `@do?[ EXP ]` node spans a unique function call. You may also embed a stateless selector/sequence or indeed any other status expression.

Ordered selectors use a similar syntax:

```cs
status s = Sel()
    - @do?[ Taunt(target) ]
    - @do?[ Reach(target) ]
    - /* ... */ ;
```

## Tasks and frame agents

We have hinted at `UTask`. In Active Logic, `UTask` is a handy base class derived from `MonoBehaviour`. Let's define a task for the `Soldier` role:

```cs
using Active.Core;

public class Soldier : UTask{

    float health = 100;

    override protected status Step()
        => Attack() || Defend() || Retreat();

    status Attack() => threat && health > 25
        ? Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
        : fail();

    status Defend() => undef();

    status Retreat() => undef();

}
```

Because it inherits from `UTask`, `Soldier` defines a Unity component which may be added to the Unity inspector. In the same way that `Update()` is present in many subclasses of `MonoBehaviour`, `Step()` is our entry point for BT-style update loops.

A task, however, **does not run on its own**.

To make the above runnable, add an `Agent`. Then, in the agent inspector, add `Soldier` as root (if you forget, this happens automatically).

Note: *`undef()` denotes an unimplemented status function. While debugging, undef randomizes its return value, which is useful for testing incomplete models; undef() is only allowed in debug mode.*

## Logging and the Log-Tree

Active Logic's logging features are helpful, in that the behavior of complex agents is rarely transparent, while tracing loops running at 10~60Hz is not practical.

In Unity, access the Log-Tree window from the *Window menu*; then, enable logging from the Agent inspector.

For useful output, we still need to annotate the soldier script:

```cs
using Active.Core;

public class Soldier : UTask{

    float health = 100;

    override protected status Step() => Eval(
        Attack() || Defend() || Retreat()
    );


    status Attack() => Eval(
        !threat     ? fail(log && "No enemy around") :
        health < 25 ? fail(log && "Health too low")  :
        Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
    );

    status Defend()  => undef();

    status Retreat() => undef();

    status Engage(Transform threat){
        var dist = Vector3.Distance(transform.position, threat.position);
        return Eval(
            dist < 1f ? done(log && "In range") : Approach()
        )[log && $"At {dist:0.#}meters from target"];
    }

    status Approach() => undef;

}
```

With the above annotations, the log tree window may appear as follows:

![Active Logic logger output](Images/activeLogicTreeView.png)

This example illustrates everything you need to know to get started with logging:

- Wherever a status is returned, use `Eval()`.
- `done()`, `cont()` and `fail()` may receive a custom log message as argument.
- You may attach a custom message to any status, like so: `status[log && $"Custom"]`
- String interpolation is preferred.
- The `log && message` idiom ensures log messages won't be processed in production builds; this means that you do not need to remove these messages before shipping your game.

## Going further

The quick start guide is intended as a short introduction to AL; to learn more, check the [API reference](Reference/Overview.md).
