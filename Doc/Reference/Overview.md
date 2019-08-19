# Active Logic Reference

## content

1. Macros and Language support
2. Status and status expressions
3. Certainties
4. Gigs and Tasks
5. Steppers, Agents and Tickers
6. Logging
7. Decorators and Custom decorators
8. Ordered composites
9. Mutable composites
10. Memory and performance

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

`AL_THREAD_SAFE`
Disable non thread-safe APIs, except logging support.
