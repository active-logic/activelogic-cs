// Doc/Reference/UGig.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;
using static Active.Status;

namespace Active.Core{
public abstract partial class Gig {

    protected static readonly LogString log = null;

    public virtual status Step()
    #if AL_OPTIMIZE
    => status._fail;
    #else
    => status.fail(log && "`Step` is not implemented");
    #endif

    public action Do(params object[] x) => @void();

    public static implicit operator Func<status> (Gig self) => self.Step;
    public static implicit operator status       (Gig self) => self.Step();

}}
