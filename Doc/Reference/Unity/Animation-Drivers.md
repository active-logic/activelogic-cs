# Animation Drivers [beta]

Active Logic provides sample integrations for both [Mecanim](https://docs.unity3d.com/460/Documentation/Manual/MecanimAnimationSystem.html) and Unity's [legacy animation system](https://docs.unity3d.com/Manual/Animations.html).

Animation may be as simple as calling `Play(anim)` or as complex as setting up blend trees, layers and hierarchic state machines. As such the provided animation drivers only offer a starting point. Indeed both drivers are very simple, and a unified interface is provided.

Import `Active.Util` to use the animation integration features.

- Add the legacy driver via `AddComponent<LegacyAnimationDriver>()`
- Add the Mecanim driver via `AddComponent<MecanimDriver>()`

With behavior trees, animation drivers are useful because animation systems do not natively represent playing animations as *tasks*. There are two cases where you may *not* wish to integrate animations as BT-styled tasks.

- If your animations are 100% model driven. In this case you issue animation commands but do not care to know the state of an animation.
- If you are comfortable with Mecanim, then perhaps your animation system integrates with the BT without explicitly returning statuses.

The sample integration assumes that animations are, as often as not, driving your agents, so that:

- You directly initiate animation playback from code (vs complex state machines).
- An agent may be waiting for an animation to complete.
- An agent may or may not be allowed to interrupt animations.

Additionally, to play an animation *while* another action is happening, consider using the `While` decorator; here is an example:

```cs
While( transform.RotateTowards(that, rotationSpeed) )
                                     ?[ driver.Loop("Rotating") ];
```

In this case, the returning status matches `EXP_1` in `While( EXP_1 )?[ EXP_2 ]` (see [builtin decorators](../Decorators-Builtin.md) for information about this and other decorators).

The [Kabuki project](https://github.com/eelstork/Kabuki/) illustrates using the animation drivers to implement game actions.

# AnimationDriver (abstract class)

`float fadeLength` - Duration for tweening between animation clips.
`pending Play(string anim)` - Play the designated animation or (mecanim) enter the specified animation state. Return `cont` while the animation is playing and `done` when/after the animation has completed.
`loop Loop(string anim)` - Loop the specified animation; always return `forever`.
