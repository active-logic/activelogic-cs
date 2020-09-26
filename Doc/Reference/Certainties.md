*Source: Certainties.cs - Last Updated: 2019.7.30*

# Certainties

When the output of a status function is restricted to a subset of values, use certainties.

Below, the `Strike` function would always return true; in lieu of a status, an action is used:

```cs
action Strike() => @void;
```

Callers acknowledge the `Strike()` function using the `now` attribute.

```cs
status Step(){  
    return Defend() || Strike().now;
}
```

Functions that return immediately should return `bool`; other cases are handled using dedicated semantics.

## Struct `action`

`action` is the type of functions that always complete immediately; actions return `@void`.
Promote `action` to `status` Via the `now` attribute.
Integrate `void` functions using `action F() => @void` instead of void `F() => {}`

Combine actions using the neutral combinator `%`:

```cs
action BunnyHop() => Jump() % Shoot();`
```

Inverting `action` using `!` returns `failure` (and vice-versa).

Like `status`, action supports the `-` operator.

## Struct `failure`

`failure` is the type of functions that always fail immediately; failures return `failure.broken`.
Promote `failure` to `status` Via the `fail` attribute.

Combine failures using the neutral combinator `%`:

Like `status`, action supports the `+` operator.

## Struct `pending`

`pending` is the type of functions that never fail; said functions return either `pending.cont` or `pending.done`.
Promote `pending` to `status` Via the `due` attribute.

Combine `pending` expressions using `&&`.
Inverted, `pending` returns `impending` (and vice-versa)

## Struct `impending`

`impending` is the type of functions that will not ever succeed; said functions return `impending.cont` or `impending.doom`.
Promote `impending` to `status` Via the `never` attribute.

Combine `impending` expressions using `||`.

## Struct `loop`

`loop` is the type of functions which never succeed or fail; loops return `forever`.
Promote `forever` to `status` Via the `ever` attribute.

Combine loops using the neutral combinator `%`:

```cs
loop Meditate() => Breath() % BeStill();`
```

## Operators support note

Certainties may support more operators. As an example, the following might be supported:

```cs
public static status operator * (action x, status y) => x.now * y;
```

There are a couple of reasons why this approach is not implemented:

- Deciding which operators should or shouldn't be supported is tedious, and the projected benefit is relatively low.
- In C#, compiler support for the `&&` and `||` is restrictive. Heterogenous combinations are not allowed; neither can we choose
(as we would in certain cases) to only implement `&` or `|`.
- As depicted above, the requirement that certainties be made explicit when used in status expressions is not fulfilled. An intermediary would be needed to combine operability with said requirement.

Meanwhile, same type operations are supported on a best effort basis.
