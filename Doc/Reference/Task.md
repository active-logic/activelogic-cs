*Source: UTask.cs - Last Updated: 2019.7.30*

# Tasks

Derived from `UGig` and `MonoBehaviour`, tasks are intended for subclassing. Generally speaking, a `UTask` subclass constitutes a dependable, productive environment for using decorators, composites, and logging features.

## Class UTask : UGig

### Methods

`Register(Resettable rsc)`
/ Register the specified resource; typically the resource is a un-managed composite or decorator

`action Reset()`
/ Reset the task. Inline or explicitly registered composites and decorators will be reset.

`action Release()`
/ Reset the task and release memory resources.

### Type Conversions

Like `UGig`, `UTask` is implicitly convertible to `status`, and `Func<status>`.
