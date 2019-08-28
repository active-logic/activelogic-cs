# Extending the API

If you wish to extend the API without touching the original source, use class extensions, subclassing, or partial classes.

Many classes in the API are defined as *partial*. Compared with class extensions, partials are less restricted.

In a typical usage scenario, you might want to add functionality to `Gig` or `UGig`. Then, define a partial class, as follows:

```cs
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

namespace Active.Core
{
    partial class UGig
    {
        // Your members here
    }
}
```

*NOTE: the name of the source file can be anything; this does not impact partials resolution.*

If you are using *Unity* or otherwise defining your own assemblies, ensure that all partials belong to the same assembly as the API itself. This may be achieved by defining partial classes under *ActiveLogic/Src/User/*
