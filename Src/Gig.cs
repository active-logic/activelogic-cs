// Doc/Reference/UGig.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;
using static Active.Core.status;

namespace Active.Core{
public abstract partial class Gig {

    protected static readonly LogString log = null;

    public virtual status Step()
    => status.fail(log && "`Step` is not implemented");

    protected action Do(params object[] x) => @void();

    public static implicit operator Func<status> (Gig self) => self.Step;
    public static implicit operator status       (Gig self) => self.Step();

}}
