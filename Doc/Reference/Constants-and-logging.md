*Sources: Status.cs, Raw.cs - Last Updated: 2020.11.15*

# Status keywords and logging calls

Since 2020.11.15 statically importing `Active.Core.status` is discouraged; keywords (or logging calls) are made available via statically importing either `Active.Raw` or `Active.Status`.

`Active.Raw` is recommended for best performance or if you are not using the logging API.
With regard to performance logging overheads are small and the tradeoff is only meaningful when optimising critical sections.

`Active.Status` enables concise logging calls, as described in the [logging API documentation](Logging.md).

Using `Active.Raw` does not disable logging. It only provides alternative semantics. The gist of it is you can't really have `done` and `done()` in the same namespace. Even with `Active.Raw` you can still issue a logging call via `status.done(..)`.

Importing `static Active.Raw` in one file and `static Active.Status` in another is also not a problem.

## Statuses and certainties via `Active.Raw`

If you statically import `Active.Raw`, status constants do not emit logging information and the following keywords are available:

```
done, cont, fail, @void, @false, forever, pending_cont, pending_done,
impending_cont, impending_fail
```

## Statuses and certainties via `Active.Status`

If you statically import `Active.Status`, status constants always emit logging information and the following calls are available:

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
