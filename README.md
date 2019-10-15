# Active Logic

A library for everyday case logic, solving problems where *time* is an essential element: game development, game AIs, interactive applications and control systems.

Active Logic implements discrete, iterative control. It is grounded in [Behavior trees](https://en.wikipedia.org/wiki/Behavior_tree_(artificial_intelligence,_robotics_and_control)) (BT) and three-valued logic.

[![Build Status](https://travis-ci.com/active-logic/activelogic-cs.svg?branch=master)](https://travis-ci.com/active-logic/activelogic-cs)

[![codecov](https://codecov.io/gh/active-logic/activelogic-cs/branch/master/graph/badge.svg)](https://codecov.io/gh/active-logic/activelogic-cs)

## License

- GNU Affero GPL v3.0. TLDR, use the software freely, *provided derivative works are free, open source*.
- Unity integration [via the Asset Store](https://www.assetstore.unity3d.com/#!/content/151850)
- Pending a standalone commercial license, read [here](Doc/Commercial.md)

## Patches

If you are using **Unity** and experience issues, you may find patches [here](Patches/); only uses patches for a matching asset version.

## Introduction

BT implementations available for general purpose languages typically involve:

**Explicit data structures** - This approach is flexible but the resulting programs may be harder to read, slower, and definitely require more memory.

**Visual programming** - This simply doesn't suit everybody's taste. Rather than a visual tool integrating existing code Via complex interfaces and/or blackboards, some of us prefer writing computer programs.

Active Logic seamlessly integrates with the host language:

```cs
class Duelist : UTask{

    float health = 100;

    Transform threat => null;

    override public status Step() => Attack() || Defend() || Retreat();

    status Attack() => threat && health > 25
        ? Engage(threat) && Cooldown(1.0f)?[ Strike(threat) ]
        : fail(log && $"No threat, or low hp ({health})");

    pending   Defend()                  => + undef();
    status    Engage(Transform threat)  =>   undef();
    impending Retreat()                 => - undef();
    action    Strike(Transform threat)  =>   @void();

}
```
[> View complete sample](https://gist.github.com/eelstork/08b8fff3b776e8a9faa262a60a9a183b)

Active Logic programs look mundane: no callbacks, lambdas or co-routines. The temporal dimension is factored into the `status` type; benefits:

- **Simplicity**: Active Logic programs have a clear and concise, inductive structure.
- **Resilience**: statelessness ensures that, confronted with changing conditions, active logic programs respond in a timely, predictable manner.

## Installation

The library is self-contained. As such no particular install steps are required. Checkout the repo (or just the `Src` directory) inside your source tree.

## Language support

- The library requires C# 7.2
- C++ and Swift versions are being worked on.

Whether you're hoping for a back-port, or would like support for another language, scratch an itch:
- Open/upvote an issue, and motivate your request
- Submit a diff

## Where next?

- Read the [Quick start Guide](Doc/QuickStart.md) and the [FAQ](Doc/FAQ.md)
- Check the [API reference](Doc/Reference/Overview.md)
- For questions pertaining game development, reach the [discord forum](https://discord.gg/Jn9TQRR).
- For other inquiries, open an issue and we'll be in touch.
