*Sources: Status.cs, Raw.cs - Last Updated: 2020.12.04*

# Status keywords, logging calls and expression wrappers

Keywords and logging calls are made available via statically importing `Active.Raw` or `Active.Status`.

`Active.Raw` is recommended for best performance or if you are not using the logging API.
With regard to performance logging overheads are small and the tradeoff is only meaningful when optimising critical sections.

`Active.Status` enables logging calls, as described in the [logging API documentation](Logging.md).

Using `Active.Raw` does not disable logging. It only provides alternative semantics. Even when using `Active.Raw` you may still issue a logging call via `status.done(..)`.

Importing `static Active.Raw` in one file and `static Active.Status` in another is also not a problem.

## Active.Raw

If you statically import `Active.Raw`, status constants do not emit logging information; keywords:

```
done, cont, fail, @void, @false, forever, pending_cont, pending_done,
impending_cont, impending_fail
```

## Active.Status

If you statically import `Active.Status`, status constants always emit logging information; keywords:

```
done(..), cont(..), fail(..), @void(..), @false(..), forever(..),
pending.cont(..), pending.done(..), impending.cont(..),
impending.fail(..)
```

## Statuses and certainties without static import

If you prefer not using static import, use `status`, `action` or any other status type, and invoke `done(..)`, `cont(..)` or `fail(..)`, as illustated by a few examples:

```
status.done()
status.done(log && "reason")
loop.cont()
impending.done() // will not compile since 'impending' never succeeds
```

## Expression wrappers

Via `Active.Raw` or `Active.Status`, use a wrapper to include any expression within a status expression, then return an arbitrary status constant; here is an example:

```cs
// Without an expression wrapper
status Collect(string label){
    target = FindNearest(label);
    return DoCollect(target);
}

// With an expression wrapper
status Collect(string label)
    => Do(target = FindNearest(label)) && DoCollect(target);
```

All expression wrappers:

`static action Do(object arg)` - returns `@void`.

`static loop Cont(object arg)` - returns `forever`.

`static failure Fail(object arg)` - returns `@false`.
