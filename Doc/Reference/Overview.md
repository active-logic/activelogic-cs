# Active Logic Reference

## Content

1. [Status and status expressions](Status.md)
2. [Certainties](Certainties.md)
3. [Gigs](Gig.md) and [Tasks](Task.md)
4. [Steppers, agents and tickers](Steppers.md)
5. [Logging](Logging.md)
6. [Decorators](Decorators.md)
    - [Built-in decorators](Decorators-Builtin.md)
    - [Custom decorators](Decorators-Custom.md)
7. [Ordered composites](OrderedComposites.md)
8. [Mutable composites](MutableComposites.md)
9. [Memory and performance](MemoryAndPerformance.md)
10. [Extending the API](Extensions.md)

## Engine & Language support

The C# implementation requires C# 7.2.
Active Behaviors is compatible with the following Unity Versions:
- Unity 2019 (tested: 2019.1.9f1)
- Unity 2018 LTS (tested: 2018.4.4f1)

Configuration (under Project Settings > Player)
- Scripting Runtime Version: `.NET 4.x Equivalent`
- API Compatibility Level: `.NET 4.x`

Verified with both Mono and IL2CPP backends (2019.7.21)

## Macros

The following macros are supported, and may be defined in `csc.rsp`

`AL_BEST_PERF`
Disable slower APIs, leaving only the fastest alternatives.
This flag does not perform any optimizations; it is intended for performance conscious conscious developers who wish to mechanically restrict access to slower APIs for which a fastest alternative is available.

`AL_OPTIMIZE`
Run in optimized mode.
In deployment, optimizations are performed, resulting in better performance at the cost of type safety and runtime error checks; set this flag to preview these optimizations. NOTE: logging is inactive in optimized mode.

`AL_STRICT`
Disable implicit conversions; recommended for collaborative workflows.
- From certainties to status
- From bool to status
- From string to ValidString

`AL_THREAD_SAFE`
Disable non thread-safe APIs, except logging support.
