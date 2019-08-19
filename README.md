:warning: **Github migration in progress**

# Active Logic

A library for everyday case logic, solving problems where *time* is an essential element - including game development, game AIs, interactive applications and control systems.

Active Logic implements discrete, iterative control. It is grounded in Hierarchic State machines (HSM), Behavior trees (BT) and Many-valued logic.

## License information

- Active Logic is currently licensed via GNU Affero GPL v3.0. Please read the license to understand whether this is suitable for you. TLDR this allows you to use the software freely, provided derivative works are free, open source.
- The library will be free for: personal and educational uses, indies and early stage startups; pending license.
- Business licenses (non free) will be made available too; pending license.
- An augmented version of the library will be sold through the Unity Asset Store; pending submission.

## Introduction

Effective, dependable control is easier if you:

- Frequently re-evaluate inputs and reassert outputs.
- Keep stateless: avoid storing information, or issuing standing orders
- Follow the D.R.Y principle
- Do not make promises: notably, avoid alloc/dealloc, enable/disable, register/deregister and other 'two step syndrome' strategies.
- Avoid concurrent and untimely writes (unity of command)
- Process coherent, comprehensive and structured input

As a paradigm, BT is relatively sound. It is leaning towards (1-5). (6) results from good practice in how you structure the input fed into behavior trees.

BT implementations are available to most general purpose languages; these implementations, more often than not, involve at least one of the following:

1 - *Explicit data structures*. This approach is flexible but the resulting programs may be harder to read, slower, and definitely require more memory.
2 - *Visual programming*. This simply doesn't suit everybody's taste. Rather than a visual tool integrating existing code via complex interfaces and/or blackboards, some of us prefer writing computer programs.

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

Active Logic programs look mundane: no callbacks, lambdas or co-routines. The temporal dimension is factored into the `status` type, with two consequences:

- **Simplicity**: Active Logic programs have a clear and concise, inductive structure.
- **Resilience**: statelessness ensures that, confronted with changing conditions, active logic programs respond in a timely, predictable manner.

## Language support

While Active Logic programs do look simple, offering a clean, concise syntax without compromising performance and thread safety is not.

The library requires C# 7.2; uniquely (if indirectly), C# permits overloading the short-circuit operators. Other languages (Notably: C, C++, Python, Rust, Swift) may not have this (or leave shorting behind).

C++ and Swift versions are being worked on. For some languages, direct support (upgrade the language) is considered.

C# back-ports are considered; no ETA.

Whether you're hoping for a C# back-port, or would like support for another language, scratch an itch:
- Open/upvote an issue, and motivate your request
- Submit a diff (an incomplete diff is better than just asking for help)
- Offer a [sponsorship](Licenses.md)

## Where next?

- [Download the library](http://TODO) (GPL and commercial [licenses](Licenses.md))
- Read the [Quick start Guide](Doc/Quick Start Guide.md)
- Check the [API reference](Doc/Reference/Overview.md)
- For questions pertaining game development, reach the [discord forum](http://TODO).
- For other inquiries, open an issue and we'll be in touch.
- If you would like to sponsor a port or feature, [read here](Licenses.md)
