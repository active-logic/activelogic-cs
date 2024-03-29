# Active Logic

[![Build Status](https://travis-ci.com/active-logic/activelogic-cs.svg?branch=master)](https://travis-ci.com/active-logic/activelogic-cs)
[![codecov](https://codecov.io/gh/active-logic/activelogic-cs/branch/master/graph/badge.svg)](https://codecov.io/gh/active-logic/activelogic-cs)

Easy to use, comprehensive Behavior Tree (BT) library built from the ground up for C# programmers:

- Game logic, game AI (autonomous agents)
- Asynchronous control applications
- No DSL/builder pattern (tight integration)
- Use standalone or borrow the "ticker" from a visual BT solution

=> [Quick-start guide](Doc/QuickStart.md) || [Unity quick-start guide](Doc/QuickStart-Unity.md) || [API reference](Doc/Reference/Overview.md) || [FAQ](Doc/FAQ.md)

## Introduction

Active Logic seamlessly integrates with C#:

```cs
class Duelist : UTask{

    float     health = 100;
    Transform threat;

    // BT selectors and sequences via || and &&
    override public status Step()
        => Attack()
        || Defend()
        || Retreat();

    // Conditionals without 'conditional nodes'
    status Attack() => (threat && health > 25) ?
        Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
      : fail(log && $"No threat, or low hp ({health})");

    status Defend() => ...;

    // ...

}
```
[> Complete sample](https://gist.github.com/eelstork/08b8fff3b776e8a9faa262a60a9a183b)

Active logic is the first BT library providing tight integration with the host language
(no DSL, no builder pattern, no lambdas or coroutines, no blackboard).
This means better performance (low/no GC), cleaner syntax and the freedom to structure your code as you wish.

## Installation

Engine/platform agnostic:
- `dotnet add package ActiveLogic` (via [NuGet](https://www.nuget.org/packages/ActiveLogic/))

Unity 3D:
- Standard Edition [via the Unity Asset Store](http://u3d.as/1AZ8)
- Active-LT [via UPM on Github](https://github.com/active-logic/active-lt)

From source: no dependencies.

## Language support

- C# 7.2 or later

## License

- GNU Affero GPL v3.0. TLDR, use the software freely, *provided derivative works are free, open source*.
- Unity Store Assets are covered by their [end user license agreement](https://unity3d.com/legal/as_terms).
- Pending a standalone commercial license, read [here](Doc/Commercial.md)

## Where next?

- Read the [Quick start Guide](Doc/QuickStart.md) and the [FAQ](Doc/FAQ.md)
- Check the [API reference](Doc/Reference/Overview.md)
- Questions? Drop by the [discord forum](https://discord.gg/Jn9TQRR).
