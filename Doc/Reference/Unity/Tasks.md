*Sources: UGig, UTask - Last Updated: 2020.11.18*

# UGig and UTask

Once you have added an [Agent or Stepper](Steppers.md) to the inspector, you subclass either `UGig` or `UTask` to create your behavior tree(s). As an example, a simple game agent may be setup in the following way:

```
GameObject (component inspector)
+ Agent
+ MyGameActor (derived from UTask)
```

Now lets say your game actor is physics-driven. In this case, it may be setup like this:

```
GameObject (component inspector)
+ Agent (→ MyGameActor)
+ PhysicsAgent (→ MyPhysics)
+ MyGameActor (derived from UTask)
+ MyPhysics (derived from UGig)
```

In the above setup, `MyGameActor` is manually designated as the root in task in `Agent`; likewise `MyPhysics` is assigned to `PhysicsAgent`. This is because we have several root tasks in the inspector.

UGig and UTask provide the same interface as [Gig](../Gig.md) and [Task](../Task.md), however since they are derived from `MonoBehaviour`, they may be added to the component inspector, and give access to the usual component properties (`transform`, `GetComponent`, `AddComponent` and so forth).

Although a stepper can only refer one root task at any given time, one `UTask` may call into another `UTask` within or without the parent game object. Additionally, if you need not expose functionality in the Unity inspector, you may delegate functionality to a plain `Task` or `Gig`.
