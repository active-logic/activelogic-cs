*Sources: Logging.cs - Last Updated: 2019.7.30*

# Logging

The log-tree lets the user visualize control state in real time. To generate a log tree, the user rely on specific idioms, detailed in this section.

The logging feature solves three problems commonly associated with logging:

- **Readability** - in general console output is unstructured and hard to read. A fortiori, using console output to debug loops running at 60Hz is a peculiar challenge.
- **Performance** - the API provides safe idioms ensuring minimal overheads, so that you needn't remove traces before shipping.
- **Best practice** - by providing clear attachment points, the API encourages you to build beautiful logs which improve maintainability of your programs.

## Logging within Gigs and tasks

### Logging statuses

Often, logging facilities are used in the context of `UGig` and `UTask` subclasses.

Wherever `done()`, `fail()` and `cont()` appear in your program, caller information is generated. You may attach a user defined message, using the following syntax:

```cs
done(log && message);
fail(log && message);
cont(log && message);
```

Here is an example:

```cs
cont(log && $"Searching within a {dist:0.#}m range");
```

String interpolation is recommended throughout. The `log && message` syntax ensures that no string formatting is performed in log-less mode, which is required for good performance. In contrast, the following usage is strongly discouraged:

```cs
var msg = string.Format("Searching within a {0:0.#}m range", dist);
cont(log && msg);
```

With the above syntax, string formatting cannot be avoided; this would result in significant overheads.

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

Whenever logging a status, you may attach a message using the indexing syntax:

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

## Logging using `Via`

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

If you prefer a consistent semantic marker (to identify every logging message within an application), use `Via` everywhere:

```cs
action Reset() => done().Via(log && $"All clear");
```

## Logging and the information chain

Building log-trees requires propagating annotated statuses up the call stack; orphaned statuses do not appear within the log-tree; the following example illustrates this scenario:

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
