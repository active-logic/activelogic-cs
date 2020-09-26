*Sources: Agent, PhysicsAgent, Stepper, Ticker - Last Updated: 2019.7.29*

# Steppers, Agents and Tickers Reference

Tasks and status functions are intended to run *frequently*; `Agent`, `PhysicsAgent`, `Ticker` and `Stepper` help you integrate tasks by controlling these updates; agents and tickers also benefit history, logging and debugging features.

A stepper runs a root task. If no root task is initially assigned the stepper will attempt to retrieve and root a `UTask` instance via `GetComponent`. If you added several tasks to the game object, assign the root in the stepper inspector.

## Class Agent : Stepper

A stepper updated every game frame.

## Class PhysicsAgent : Stepper

A stepper updated every physics frame.

## Class Stepper : MonoBehaviour

Use this class when you need to manually trigger updates Via the `Step()` function (for example, to control the flow of a turn based game).

### Inspector

**Enable Logging**
Enables logging (visible in LogTree window); also required for history and breakpoints.

**Use History**
Record an agent's recent history.

**Span**
Number of seconds to record (requires history).

**Size**
Size of history widgets (requires history).

**Height**
Offset history widgets by this amount (requires history).

**Use Breakpoints**
Pause execution when breakpoints are encountered.

**timeScale**
Accelerate or slow down time while breakpoints are enabled for this agent (requires breakpoints).

**step**
Pause execution whenever the log-tree changes (requires breakpoints).

**breaks**
List of string fragments to match against the log-tree (requires breakpoints).

### Properties

`UGig root`
The gig or task controlled by this agent.

`bool loop`
Unset if you don't want the agent to loop over (default is `true`)

`status state`
Returning state after `Step()`

### Methods

`T Do<T>() where T : UGig`
Create and root a task of type T; also enable the agent, if currently disabled (`Agent` only; provisional).

`void Push(Func<status> φ)`
Override the current task/behavior tree with a task φ. The argument task will then execute until completing or failing. After φ has stopped, the overriden task resumes normally.

`void Run(Func<status> φ)`
Run the specified task, wrapped via an adapter. Mainly for testing purposes.

`status Step()`
Iterate the stepper.

`Pause(duration)`, `Suspend()` and `Resume()`
Pause execution for the specified duration, or suspend until `Resume()` is called.

## Class Ticker : Stepper

A stepper updated at custom intervals.

`float firstTime`
Delay before the first update after enabling or re-enabling.

`float repeatRate`
Delay between updates
