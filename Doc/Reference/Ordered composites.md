*Sources: Sequence, Selector - Last Updated: 2019.7.30*

# Ordered Composites

Ordered composites implement a stateful control model. A tracking index is maintained, thereby:

- A stateful sequence does not revisit previous steps. At every iteration, processing iterates the current subtask, optionally moving to the next subtask, until the sequence is exhausted, or a failing subtask is found.
- A stateful selector does not retry previous steps. At every iteration, processing iterates the current subtask, until a succeeding subtask is found, or the selector fails.

Stateful composites are designed to support specific notations. Without the prescribed notations, behavior is undefined.

## Ordered composites in a UTask context

In a task context, use the following notation:

```cs
status s = COMPOSITE()[   
    and ? EXP_1  :
    and ? ...    :
    and ? EXP_N  : TERMINAL];
```

Where:

- *COMPOSITE* is either `Sequence` or `Selector`
- *TERMINAL* is either `loop` or `end`
- *EXP_1 ... EXP_N* are status expressions.

## Ordered composites without a UTask context

Sequences and selectors are stateful; without a task context, they must be defined and declared before use.

```cs
private Sequence s = new Sequence();  // or `Selector()`
```

A sequence (or selector) is then associated with a unique pattern in your code: do not reuse a `Sequence` or `Selector` object for multiple occurrences of the pattern):

### Ternary notation

```cs
// Anywhere in your code...
var i = s.iterator(c);             // s: previously declared sequence or
status k = i[ i ? EXP_1  :         // selector; EXP_1 ... EXP_N
              i ? ...    :         // are status expressions.
              i ? EXP_N  : i.end]; // use `end` or `loop` here.
```

Across iterations, the iterator should not be stored, or reused (it is cached and managed on your behalf by the sequence/selector object).

How it works:
- Within the square brackets, every occurrence of the iterator `i` increments a counter
- The counter is checked against the sequence index.
- At each iteration, only *one* of EXP_1 ... EXP_N is invoked.
- Whenever an expression within {EXP1, ..., EXP_N} returns a value other than `running`, the selector/sequence either advances to the next subtask, or fails. This is checked by 'piping' the resulting value through the `i[...]` indexer.

### Case notation

The case notation is faster, especially for long sequences/selectors.

```cs
// Anywhere in your code...
status k;
switch(s){
    case  0   : k = s[ EXP_1 ]; break;
    case ...  : k = s[  ...  ]; break;
    case  N   : k = s[ EXP_N ]; break;
    default   : k = s.end;      break;
}
```

Often you return a status right away:

```cs
// Anywhere in your code...
switch(s){
    case  0  : return s[ EXP_1 ];
    case ... : return s[  ...  ];
    case  N  : return s[ EXP_N ];
    default  : return s.loop;      
}
```

How it works:
- `s` implicitly evaluates to the index of the current subtask.
- Accordingly, the matching `case` is evaluated.
- Whenever an expression within {EXP1, ..., EXP_N} returns a value other than `running`, the selector/sequence either advances to the next subtask, or fails. This is checked by 'piping' the resulting value through the `s[...]` indexer.

Generally, the case notation should be avoided (see notes, below)

## Notes

1 - With either the case or ternary notations, the `loop/end` tokens must not be omitted.
2 - With the case notation, framing the `loop/end` statement in square brackets is an error.
3 - Incorrect numbering of a case pattern will cause incorrect behavior.
