*Sources: Logging.cs - Last Updated: 2020.11.15*

# Logging

The log-tree lets you visualize control state in real time. This section describes logging related APIs.

The logging feature solves three problems commonly associated with logging:

- **Readability** - in general console output is unstructured and hard to read. Using console output to debug loops running at 60Hz is ineffective.
- **Performance** - the API provides safe idioms ensuring minimal overheads, so that you needn't remove traces before shipping.
- **Best practice** - the logging API helps you design self-explanatory control programs with minimal semantic overheads.

## Logging within gigs and tasks

### Logging statuses

Often logging is used in the context of `UGig` and `UTask` subclasses.

Wherever `done`, `fail`, `cont` and other status keywords appear in your program, adding `()` generates caller information and you may attach a custom message to *motivate* the emitted status:

```cs
done();                        // no custom message
done(log && message);          // custom messages appended
pending.cont(log && message);  // ...
```

Here is an example:

```cs
cont(log && $"Searching within a {dist:0.#}m range");
```

To avail short-form logging calls (such as `done()` instead of `status.done()`), statically import `Active.Status`.

String interpolation is recommended; the `log && message` syntax ensures no string formatting is applied in release builds, which is required for best performance. In contrast the following approach is strongly discouraged:

```cs
var msg = string.Format("Searching within a {0:0.#}m range", dist);
cont(log && msg);
```

With the above syntax, string formatting overheads cannot be avoided.

Using the above syntax, you may also attach messages to `@void()`, `failure()`, `forever()`, `impending.cont()`, `impending.doom()`, `pending.cont()` and `pending.fail()`.

### Logging expressions

To correctly log status expressions, the `Eval()` function is used. Here is an example.

```cs
public status Defend(){
    return Eval(
        Strike
        || Parry
        || Explode
        || fail(log && "No options left")
    );
}
```

- Without `Eval`, only the latest executing node will be logged.
- With `Eval`, all nodes up to the latest executing node will be logged.

**TIP:** Always use `Eval` in combination with `return`.

Whenever logging a status, you may also attach a custom message using the indexing syntax:

```cs
public status Defend(){
    return Eval(
        Strike
        || Parry
        || Explode
        || fail(log && "No options left")
    )[log && $"HP left: {health}"];
}
```

***Why 'Eval' is necessary***

As your code executes, the AL library accumulates logging information pertaining to status expressions. As an example consider the following function:

public ABC() => A && B && C;

Upon evaluating `C`, A && `B` have already evaluated, and the associate logging information is preserved. However, without `Eval`, AL cannot determine that we are exiting ABC.

Because of that, although you might get output without using `Eval`, the structure of such output may be incorrect.

If you do not consistently use `Eval` the log-tree view will display partial/incorrectly structured output.

## Logging using `Via`

*[DOC REVIEW NOTE] in recent versions of AL, it appears that Eval is now a static keyword.*

Without a `UGig` or `UTask` context, all the above idioms remain available except `Eval()`. In place of `Eval()` use `Via()`:

```cs
public status Defend(){
    return (
        Strike
        || Parry
        || Explode
        || fail(log && "No options left")
    ).Via(log && $"HP left: {health}");
}
```

If you prefer a consistent semantic marker (to identify every logging message within your application), use `Via` everywhere:

```cs
action Reset() => done().Via(log && $"All clear");
```

## Logging and the information chain

Building log-trees requires propagating annotated statuses up the call stack; orphaned statuses do not appear within the log-tree; the following example illustrates this:

```cs
public status TwoStep(){
    status s = Preliminary()[log && "Orphaned"];
    if(s){
        return done(log && "So good so far");
    }else{
        return fail(log && "Not so quick");
    }
}
```

Above, a logging message is attached to 's'. However there is no continuity between 's' and the returned statuses. As a result, 's' will not appear in the log tree. If you want `s` to partake the log tree, here is the corrected syntax.

```cs
public status TwoStep(){
    status s = Preliminary()[log && "Remembered"];
    if(s){
        return Eval(+s)[log && "Not so quick"];
    }else{
        return Eval(-s)(log && "Not so quick");
    }
}
```

In the above example, the unary `+` and `-` are used to promote/demote the carried over status, thereby ensuring a return value of `done` or `fail`.
