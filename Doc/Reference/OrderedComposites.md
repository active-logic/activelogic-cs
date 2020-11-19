*Sources: Seq, Sel - Last Updated: 2020.11.19*

# Ordered Composites [beta]

Ordered composites run tasks sequentially, without reiterating complete/failing tasks; since tasks do not reiterate, they always run in the same order.

After an ordered composite has completed or failed, by default, it will reset and repeat (or set `repeat` to false).

Without an object context, ordered composites are thread safe.

## Syntax (object context)

Within a `Task/UTask`, ordered sequences use the following syntax:

```cs
status s = Seq(repeat: true|false)   
    + @do?[ TASK_1 ]
    + @do?[ TASK_2 ]
    + ...;
```

Ordered selectors then use the following syntax:

```cs
status s = Sel(repeat: true|false)   
    - @do?[ TASK_1 ]
    - @do?[ TASK_2 ]
    - ...;
```

## Syntax (without an object context)

Without an object context, ordered sequences use the following syntax:

```cs
// Declare and allocate
Seq s = new Seq();
// Once only, anywhere in your code
status s = s.Repeat()  // or Once()
    + s.@do?[ TASK_1 ]
    + s.@do?[ TASK_2 ]
    + ...;
```

Ordered selectors then use the following syntax:

```cs
// Declare and allocate
Sel s = new Sel();
// Once only anywhere in your code
status s = s.Repeat()  // or Once()
    - s.@do?[ TASK_1 ]
    - s.@do?[ TASK_2 ]
    - ...;
```

Reusing ordered sequence/selector objects is an error; without an object context, if you want two ordered composites in the same class, declare and allocate two separate variables.

NOTE: *`Seq` and `Sel` replace `Sequence`, `Selector`; the previous implementation did not support greedy evaluation, which conflicted with the Reset-on-Resume (RoR) feature; legacy documentation is available [here](OrderedComposites_deprecated.md)* (since 11.11.2020).
