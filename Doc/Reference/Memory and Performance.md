# Memory & Performance

The C# implementation is designed with performance in mind:
- No GC beyond initial allocations
- `status` is a small struct which lives on the stack
- Optimized mode reduces status footprint (4 bytes) and increases performance (+25%)
- Performance/memory aware logging APIs. In optimized mode (deployment), at least 95% of logging overheads are avoided. In log-less mode (any behavior with 'logging' turned off), around 80% of logging overheads are avoided, with no string formatting, and no allocations.

With this in mind, if you have a performance intensive application, certain APIs should be avoided.

The API provides several 'implied storage' APIs. Stateful constructs do require storage which, under the hood, need hashing (see `Store.cs` and `Hashstore.cs` for implementation details). Declaring and defining decorators and composites explicitly is faster.

Before switching the `BEST_PERF` flag (see macros in overview) use a profiler and focus on specific areas that need optimizing. Avoiding implied storage in a few key places may be all you need.

Where memory is a concern, avoid mutable composites, if you can. Also avoid implied storage, since hashing has a significant memory cost.

The remainder of this section demonstrates syntactic differences between implied storage and explicit storage APIs.

## Implied vs explicit storage: decorators

With implied storage:

```cs
status s = Cooldown(0.1f)?[Fire()];
```

Without implied storage:

```cs
// Field declaration
public Cooldown overheat = 0.1f;
// In the update loop
status s = overheat.pass?[Fire()];
```

## Implied vs explicit storage: ordered composites

With implied storage:

```cs
// In the update loop
status k = Sequence()[
    and ? EXP_1  :         
    and ? ...    :         
    and ? EXP_N  :  i.end];
```

Without implied storage:

```cs
// Field declaration
private Sequence s = new Sequence();
// In the update loop
var i = s.iterator(c);             
status k = i[ i ? EXP_1  :         
              i ? ...    :         
              i ? EXP_N  : i.end];
```

## Case notation applied to sequences and selectors

With ordered composites, you can squeeze more performance using the case notation, instead of the ternary notation. The `BEST_PERF` flag does not disable ternary notation, because the case notation isn't a real substitute for that.

```cs
// Field declaration
private Sequence s = new Sequence();
// In the update loop
status k;
switch(s){
    case  0   : k = s[ EXP_1 ]; break;
    case ...  : k = s[  ...  ]; break;
    case  N   : k = s[ EXP_N ]; break;
    default   : k = s.end;      break;
}
```
