*Source: Task.cs, UTask.cs - Last Updated: 2021.8.2*

# Tasks

Derived from gigs (`Gig` or `UGig`) tasks are intended for subclassing; in addition to logging features availed in gigs, tasks easily leverage stateful composites and decorators.

## Class Task : Gig

*In Unity: Task and UTask*

### Methods

`Register(Resettable rsc)`
/ Register the specified resource; typically the resource is a un-managed composite or decorator

`action Reset()`
/ Reset the task. Inline or explicitly registered composites and decorators will be reset.

`action Release()`
/ Reset the task and release memory resources.

### Type Conversions

Like `UGig`, `UTask` is implicitly convertible to `status`, and `Func<status>`.
