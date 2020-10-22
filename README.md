# Active Logic

[![Build Status](https://travis-ci.com/active-logic/activelogic-cs.svg?branch=master)](https://travis-ci.com/active-logic/activelogic-cs)
[![codecov](https://codecov.io/gh/active-logic/activelogic-cs/branch/master/graph/badge.svg)](https://codecov.io/gh/active-logic/activelogic-cs)

Easy to use, comprehensive Behavior Tree (BT) library, built from the ground up for C# programmers:

- Game AI (intelligent agents)
- Simplify, fix and condense your game logic

[Quick start Guide](Doc/QuickStart.md) || [API reference](Doc/Reference/Overview.md) || [FAQ](Doc/FAQ.md)

## License

- GNU Affero GPL v3.0. TLDR, use the software freely, *provided derivative works are free, open source*.
- Unity integration [via the Asset Store](https://www.assetstore.unity3d.com/#!/content/151850)
- Pending a standalone commercial license, read [here](Doc/Commercial.md)

<!---
## Patches

If you are using **Unity** and experience issues, you may find patches [here](Patches/); only uses patches for a matching asset version.
--->

## Introduction

Active Logic seamlessly integrates with C#:

```cs
class Duelist : UTask{

    float health = 100;

    Transform threat => null;

    // BT selectors via `||`! No more DSLs/builder pattern
    override public status Step() => Attack() || Defend() || Retreat();

    // Conditionals without 'conditional nodes'
    status Attack() => threat && health > 25
        ? Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
        : fail(log && $"No threat, or low hp ({health})");

    // Special statuses (pending, impending, action, ...) are used when
    // you already know a taks only returns one or two states
    // (optional but type safe!)
    pending   Defend()                  => + undef();
    status    Engage(Transform threat)  =>   undef();
    impending Retreat()                 => - undef();
    action    Strike(Transform threat)  =>   @void();

}
```
[> View complete sample](https://gist.github.com/eelstork/08b8fff3b776e8a9faa262a60a9a183b)


No callbacks, lambdas or co-routines; unlike via DSLs or the 'builder pattern', you get the best of BT, and structure your code as you please!

## Installation

Self-contained; no particular install steps required. Checkout the repo (or just the `Src` directory) inside your source tree.

## Language support

- The library requires C# 7.2
- C# only (still considering a C++ implementation)

## Where next?

- Read the [Quick start Guide](Doc/QuickStart.md) and the [FAQ](Doc/FAQ.md)
- Check the [API reference](Doc/Reference/Overview.md)
- Questions? Drop by the [discord forum](https://discord.gg/Jn9TQRR).
