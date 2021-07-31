*Sources: SimTime.cs - Last Updated: 2021.07.18*

# SimTime Reference

In order for time-reliant decorators (cooldown, delay, interval, timeout, wait and perhaps others) to work as expected, the time should be correctly set.

- In a video game, usually you want game time; this (ordinarily) is different from real time.
- When evaluating a simulation, set the time at the beginning of each iteration.
- For some applications, real time may be just what you need.

## Setting the time

```
SimTime.time = 34.14f;  // Set the current time (unitless/client specific)
```

When do we need to set the time?

- If all the agents/tickers in your game use the same time, consider setting `SimTime.time` at the beginning of each iteration (or game/physics frame)

- In less common cases, different agents may each use their own time. For example, in a turn based game, 'tactical time' (round count) may be used to evaluate some agents while other parts of your game may use real time for animation.

**Sim time is not thread safe** - you may use sim time in a multi-threaded environment, provided all agents use the same time, and all threads complete work within the current iteration.

Note: in *Unity*, time defaults to `UnityEngine.Time.time`; you may change this on a stepper basis (see [Steppers](Unity/Steppers.md))

## Fields & Properties

`time` - get/set the current time.
