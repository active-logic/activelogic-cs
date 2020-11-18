*Last Updated: 2020.11.18*

# Active Logic Reference

## Content

### Core AL

1. [Status and status expressions](Status.md)
2. [Certainties](Certainties.md)
3. [Status keywords and logging calls](Constants-and-logging)
4. [Gigs](Gig.md) and [Tasks](Task.md)
5. [Decorators](Decorators.md)
    - [Built-in decorators](Decorators-Builtin.md)
    - [Custom decorators](Decorators-Custom.md)
    - [Reset blocks](Reset-Management.md) [beta]
6. [Ordered composites](OrderedComposites.md)
7. [Mutable composites](MutableComposites.md)
8. [Memory and performance](MemoryAndPerformance.md)
9. [Extending the API](Extensions.md)

### Unity Integration

1. [Steppers, agents and tickers](Unity/Steppers.md)
2. [UGig & UTask](Unity/Tasks.md)
3. [Logging](Unity/Logging.md)
4. [Visual History](Unity/Visual-History.md)
5. [Soft breakpoints](Unity/Soft-Breakpoints.md)
6. [Animation drivers](Unity/Animation-Drivers.md)

Note: *Unity Integration is available [via the Unity Asset Store](
http://u3d.as/1AZ8)*

## Engine & Language support

Active Logic requires C# 7.2 or later.

The library is tested against the following Unity Versions:
- Unity 2019 and later (tested: 2019.1.9f1)
- Unity 2018 LTS (tested: 2018.4.4f1)
- Verified with Mono and IL2CPP backends

Configuration (under Project Settings > Player)
- Scripting Runtime Version: `.NET 4.x Equivalent`
- API Compatibility Level: `.NET 4.x`

NOTE: In Unity 2019 and later you needn't tweak .NET configuration.

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
