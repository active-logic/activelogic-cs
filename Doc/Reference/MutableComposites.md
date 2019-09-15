*Source: MComposite - Last Updated: 2019.7.30*

# Mutable Composites

Status expressions and ordered composites are designed to handle static graphs, where the number of nodes, and their nature, are known before runtime. Mutable composites let you build and modify logic graphs at runtime.

## Class MComposite

### Factory Methods

```
static Selector(params Func<status>[] tasks)
static Selector(IEnumerable<Func<status>> tasks)
```

Create a new selector.

```
static Sequence(params Func<status>[] tasks)
static Sequence(IEnumerable<Func<status>> tasks)
```

Create a new sequence.

### Fields & Properties

`bool loop`
/ Loop over after execution?

`status current`
/ Current status; initial status is `done` for sequences, `failing` for selectors

`bool dither` [PLANNED]
/ Enable/disable switching to another task before the current task has completed.

`Composite.Flow flow`
/ Get/Set the execution model; one of `Flow.Progressive`, `Flow.Ordered` or `Flow.Concurrent`.

`bool isSequence`
/ Is this composite a sequence? (read-write)

`bool isSelector`
/ Is this composite a selector? (read-write)

`public bool concurrent`
/ Set this flag to execute concurrently (shorthand for `flow = Flow.Concurrent`)

`bool ordered`
/ Set this flag to execute step by step (shorthand for `flow = Flow.Ordered`)

`bool progressive`
/ Set this flag to execute like a status expression (shorthand for `flow = Flow.Progressive`)

### Methods

`status Step()`
/ Iterate the composite once. Note: `var s = composite.Step()` is equivalent to `status s = composite` (see conversions).

`action Reset()`
/ Reset the composite.

### Type conversions

`MComposite` is implicitly convertible to `status`; this conversion is equivalent to invoking `Step()`.

`MComposite` is implicitly convertible to `Func<status>`. The conversion returns (but does not invoke) the `Step` method.
