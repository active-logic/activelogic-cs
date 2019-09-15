
*Reading time: 10 minutes - Last Updated: 2019.8.1*

# Active Logic Quick Start Guide

Be welcome! This document is a hands on introduction to Active Logic. Let's get started.

## Status expressions

First, consider a basic 'soldier' AI. A soldier may attack, defend or retreat; this can be modeled using a status expression:

```cs
status s = Attack() || Defend() || Retreat();
```

The above realizes a *fallback strategy* (commonly referred to as a *selector*), reading:

```
If possible, attack;
otherwise, defend;
otherwise, retreat.
```

A `status` may be `complete`, `failing` or `running` (for 'continue'). a value of *running* causes execution to yield until the next iteration.

Status expressions must be invoked *frequently* - usually, within update loops:

```cs
status s = cont();
while( s.running ){
    s = Attack() || Defend() || Retreat();
}
```

In the active form - when assigning or returning a status, use `done()`, `fail()` or `cont()`. Here, the while loop iterates until the status 's' changes to *failing* or *complete*.

In a status expression, terms which return a status are usually referred to as 'tasks' or 'subtasks'.

*Note: status does not convert to bool. Instead, query the 'running', 'failing' and 'complete' properties.*

A key to understanding status expressions is that a later task - here: *Retreat()* - does not get evaluated before prior tasks (*Attack* and *Defend* are traversed. For this reason, a status function is often implemented as follows:

```cs
status DoSomething(){
    if( GUARD_CONDITIONS ) return fail();
    REALLY_DO_SOMETHING;
}
```

If any of the guard conditions restricts the generality of a status function, move them up the call stack:

```cs
// Using the conditional 'AND'...
status s = ( CONDITION && DoSomething() ) || SomethingElse();  
// The ternary conditional...
status s = CONDITION ? DoSomething() : SomethingElse();
// ...or even like this:
status s = ( CONDITION ? DoSomething() : fail() ) || SomethingElse();
```

Status expressions implement *stateless control*. In our model this approach is beneficial: depending on how `Attack()` is implemented, a wounded agent may receive healing and get back on the offense. Here is a possible implementation of the `Attack()` function:

```cs
status Attack(){
    if(health < 25) return fail();
    if(!threat)     return fail();
    return Engage(threat) && Strike(threat);
}
```

Notice the `Approach(threat) && Strike(threat)` idiom. The conditional  operator AND (&&) behaves differently from the conditional OR (||). In this case, an attack will not complete unless both `Engage` and `Strike` do succeed.

Expressions of the form `EXP_1 && ... && EXP_n` are known as *sequences*; use a sequence when each step is prerequisite to the next.

More generally, selectors and sequences are known as *composites*.

In all, status expressions and status functions combine to form behavior trees. A key difference with most BT implementations is that the behavior trees are statically wired in program memory, instead of using explicit data structures (where useful, the latter is also supported).

## Decorators

In the above example, we've hinted at a `Strike()` action. Generally, effective control requires a variety of small 'utilities' used to modulate behavior. A staple of video game design, the humble cooldown is one such thing:

```cs
status Attack(){
    // ... Omitted for space ...
    return Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ];
}
```

Now, if you've ever implemented a cooldown, you should be asking 'where is the time stamp'? The `Dec(...)?[ EXP ]` syntax is hiding data, which is managed on your behalf. For this reason, the above syntax only works in a `UTask` context - that is, a class derived from `UTask` - and we'll get back to this momently.

The API offers a few built-in decorators [REF], and you may also create your own, custom decorators [REF].

*NOTE: Decorator syntax uses the null-conditional operator [REF]*

## Ordered Composites

Status expressions are great but there is a utilitarian aspect to them which does not fit every use. Let's take an example:

```cs
status s = Reach(A) && Reach(B);
```

Applied repeatedly, assuming A and B are navigation waypoints, this causes an actor to jitter around A - because moving towards B undoes the first step.

Where an actor are intended to follow a sequence of steps *by design*, use an ordered composite:

```cs
status s = Seq()[           // Use 'Sel' for selectors instead
    and ? Reach(A) :        // Repeat `and ? EXP :` to you heart's content
    and ? Reach(B) : end ]; // use 'end' or 'loop'
```

The above syntax is available in a `UTask` context. Once again, this is because ordered composites are stateful - here, an integer tracks the currently executing subtask.

- Upon failure the sequence is aborted and no further action is taken until `UTask.Reset()` is called.
- Upon success, subsequent iterations skip the first term and evaluate `Reach(B)` instead.

Ordered sequences and selectors may use or embed within, status expressions.

As specified by the `end` keyword, once an ordered composite has fully evaluated, control skips over. To loop over, use the `loop` keyword - or re-init Via `UTask.Reset()`.

## Tasks and frame agents

We've already hinted at the `UTask` context. In Active Logic, `UTask` is a handy base class derived from `MonoBehaviour`. Let's define a task for the `Soldier` role:

```cs
using Active.Core;

public class Soldier : UTask{

    float health = 100;

    override protected status Step()
    => Attack() || Defend() || Retreat();


    status Attack() => threat && health > 25
        ? Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
        : fail();

    status Defend()  => undef();

    status Retreat() => undef();

}
```

As above, the `Soldier` task may be added to a Unity game object. A task, however, does not run on its own.

To make this runnable, add a `FrameAgent`. Then, in the frame agent inspector, add `Soldier` as root.

*Note: `undef()` denotes an unimplemented status function. While debugging, undef randomizes its return value, which is useful for testing incomplete models; undef() is only allowed in debug mode*

## The Log-Tree

Active Logic offers a log-tree feature; this is helpful, in that the behavior of complex agents is rarely transparent, and tracing update loops running at 10~60Hz is markedly inconvenient.

In Unity, access the Log-Tree window from the Window menu; then, enable logging from the Frame Agent inspector.

For useful output, annotate the soldier script:

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

With the above annotations, the log tree window may look as follows:

![Active Logic logger output](Images/activeLogicTreeView.png)

This example illustrates everything you need to know to get started with logging.

- Wherever a status is returned, use `Eval()`.
- `done()`, `cont()` and `fail()` may receive a custom log message as argument
- You can attach a custom message to any status, like this: `status[log && $"Custom"]`
- String interpolation is recommended.
- The `log && message` idiom ensures no formatting in production builds (performance); if you wish to strictly enforce this, enable `AL_STRICT`.

## Going further

The quick start guide is intended as an introduction. Reference documentation is available online.
