# Active Logic Reference

## Content

1. [Status and status expressions](Status.md)
2. [Certainties](Certainties.md)
3. [Status keywords and logging calls](Constants-and-logging)
4. [Gigs](Gig.md) and [Tasks](Task.md)
5. [Steppers, agents and tickers](Steppers.md)
6. [Logging](Logging.md)
7. [Decorators](Decorators.md)
    - [Built-in decorators](Decorators-Builtin.md)
    - [Custom decorators](Decorators-Custom.md)
    - [Reset blocks](Reset-Management.md) [beta]
8. [Ordered composites](OrderedComposites.md)
9. [Mutable composites](MutableComposites.md)
10. [Memory and performance](MemoryAndPerformance.md)
11. [Extending the API](Extensions.md)

## Engine & Language support

The C# implementation requires C# 7.2 or later
The library is tested against the following Unity Versions:
- Unity 2019 and later (tested: 2019.1.9f1)
- Unity 2018 LTS (tested: 2018.4.4f1)
- Verified with Mono and IL2CPP backends

Configuration (under Project Settings > Player)
- Scripting Runtime Version: `.NET 4.x Equivalent`
- API Compatibility Level: `.NET 4.x`

## Macros

The following macros are supported, and may be defined in `csc.rsp`

`AL_BEST_PERF`
Disable slower APIs, leaving only the fastest alternatives.
This flag does not perform any optimizations; it is intended for performance conscious conscious developers who wish to mechanically restrict access to slower APIs for which a fastest alternative is available.

`AL_OPTIMIZE`
Run in optimized mode.
In deployment, optimizations are performed, resulting in better performance at the cost of type safety and runtime error checks; set this flag to preview these optimizations. NOTE: logging is inactive in optimized mode.

`AL_THREAD_SAFE`
Disable non thread-safe APIs, except logging support.
