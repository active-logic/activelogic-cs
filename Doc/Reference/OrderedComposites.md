*Sources: Seq, Sel - Last Updated: 2020.11.11*

# Ordered Composites [beta]

Ordered composites run tasks sequentially, without reiterating complete/failing tasks.

Since tasks do not reiterate, they always run in the same order.

After an ordered composite has completed or failed, by default, it will reset and repeat (or set `repeat` to false).

Ordered composites are not supported without a `Task`/`UTask` context.

NOTE: *`Seq` and `Sel` replace `Sequence`, `Selector`; the previous implementation did not support greedy evaluation, which conflicted with the Reset-on-Resume (RoR) feature; legacy documentation is available [here](OrderedComposites_deprecated.md)* (since 11.11.2020).

## Ordered sequence

In a `Task`/`UTask` context:

```cs
status s = Seq(repeat: true|false)   
    + @do?[ TASK_1 ]
    + @do?[ TASK_2 ]
    + ...;
```

## Ordered selector

In a `Task`/`UTask` context:

```cs
status s = Sel(repeat: true|false)   
    - @do?[ TASK_1 ]
    - @do?[ TASK_2 ]
    - ...;
```
