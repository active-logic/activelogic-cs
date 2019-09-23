# Active Logic FAQ

## What is main difference between active-logic-cs (on Github) and the Unity Asset store package?

The Github repository is licensed via GPL-Affero, whereas the store package is covered by the store's end user license agreement (EULA); if your project is not open source, get the store package.

## Technically speaking, is the Github version different from the asset store package?

The Github version does not depend on the Unity platform. Its purpose is to avail core functionality to the C# community at large.

The Unity integration offers additional features:

- Interactive visualization of behavior trees at runtime, provided your code is correctly annotated, and your behavior trees are managed via dedicated components.
- Visual history analyzes behavior over time.
- Dedicated components for rooting and managing your behavior trees. This includes 'steppers' for update loops (Agent, PhysicsAgent and Stepper) and task components (UGig, UTask) derived from MonoBehaviour

The Gihub repository is not intended as a 'light version' of the store package; compatibility across Mono, .NET and .NET Core enables other integrations.

## With UGig and UTask, do I need to make every task a component?

No. You should use status expressions and status functions alongside Gig, Task (objectified tasks not inherited from MonoBehavior) and UGig, UTask.

To understand how this all fits together, consider that a behavior tree is a hierarchic structure. A tree has roots, branches and leaves.

In Active Logic you do not need a component or objectified task to form a behavior tree, however:
- If you have a behavior tree not associated with a component or stepper, you cannot directly attach it to a game object, and no output is seen in the log-tree view.
- Gigs and tasks provide features which cannot be availed using only status expressions. For example you can use a decorator in a status function, but for most decorators they are managing data, which needs to be stored somewhere.

## What is the difference between Active Logic's logger/debugger and Prolog?

[Prolog](https://github.com/active-logic/prolog) is a general purpose, automated logger for C# and Unity. This tool depends on Mono.Cecil, and Unity's build system. Prolog visualizes function calls chronologically, either globally, or on a game object basis. As such its usefulness goes well beyond the Active Logic library.

![Prolog output](Images/prologOutput.png)

The integrated logger/debugger relies on explicit annotations in your code. As such it does not have any dependencies; tree view visualizes AIs and controllers as hierarchies of tasks and subtasks. Currently, tree view generates output that is more suitable for analyzing behavior trees and supports breakpoints.

![Active Logic logger output](Images/activeLogicTreeView.png)

The AL debugger has more features which aren't available in prolog just yet:

- Crosses game object boundaries; if you have squad AIs or other coordinated behaviors, tree view provides a unified view.
- Breakpoints
- Custom annotations

## What are plans for future development?

Active Logic stemmed from a desire to provide an outstanding BT library, specifically for C# programmers. The library has been very carefully designed and implemented - the obvious step now is to evangelize and listen. I want to hear what programmers have to say about this library, and this I would expect will deeply impact future development.
