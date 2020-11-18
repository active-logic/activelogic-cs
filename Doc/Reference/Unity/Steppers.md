*Sources: Agent, PhysicsAgent, Stepper, Ticker - Last Updated: 2019.7.29*

# Steppers, Agents and Tickers Reference

Tasks and status functions are intended to run *frequently*; `Agent`, `PhysicsAgent`, `Ticker` and `Stepper` help you integrate tasks by controlling these updates; agents and tickers also benefit [history, logging](Logging.md) and [debugging](Soft-Breakpoints.md) features.

Agents and steppers are Unity components added to the component inspector. A typical setup will look like this:

```
GameObject
+ Agent
+ MyTask (derived from UTask)
```

A stepper runs a root task. If no root task is assigned the stepper will find a `UTask` via `GetComponent`.
If however you have added several tasks to a game object, you should manually assign the root in the inspector.

- `Agent` ticks the behavior tree on `Update` (every game frame)
- `PhysicsAgents` ticks the behavior tree on `FixedUpdate` (every physics frames).
- To manually update the behavior tree (such as when controlling the flow of a turn based game), use `Stepper`.

Agents and steppers use a common interface, detailed in the following section.

### Inspector

**Enable Logging** - Enables logging (visible in LogTree window); also required for history and breakpoints.

**Use History** - Record an agent's recent history.

**Span** - Number of seconds to record (while history recording is enabled).

**Size** - Size of history gizmos.

**Height** - Offset history gizmos by this amount.

**Use Breakpoints** - Pause execution when breakpoints are encountered.

**Time Scale** - Accelerate or slow down time while breakpoints are enabled for this agent (requires breakpoints enabled).

**Step** - Pause execution whenever the log-tree changes (requires breakpoints enabled).

**Breaks** - List of string fragments to match against the log-tree (requires breakpoints enabled).

### Properties

`UGig root` - The gig or task controlled by this agent.

`bool loop` - Unset if you don't want the agent to loop over (default is `true`)

`status state` - Returning state after `Step()`

### Methods

`T Do<T>() where T : UGig` - Create and root a task of type T; also enable the agent, if currently disabled (`Agent` only; provisional).

`void Push(Func<status> φ)` - Override the current task/behavior tree with a task φ. The argument task will then execute until completing or failing. After φ has stopped, the overriden task resumes normally.

`void Run(Func<status> φ)` - Run the specified task, wrapped via an adapter. Mainly for testing purposes.

`status Step()` - Iterate the stepper.

`action Pause(duration)` -  Pause execution for the specified duration.

`action Suspend()` - Suspend until `Resume()` is called.

`action Resume()` - Resume after calling `Pause` or `Suspend`.

## Ticker

A ticker is a stepper updated at custom intervals.

`float firstTime` - Delay before the first update after enabling or re-enabling.

`float repeatRate` - Delay between updates
