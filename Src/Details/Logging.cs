// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using Active.Core;
using Active.Core.Details;
using F  = Active.Core.Details.StatusFormat;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using V  = Active.Core.Details.ValidString;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using S  = System.String;
using X  = Active.Core.status;
using Lg = Active.Core.Details.Logging;
using static Active.Core.status;

namespace Active.Core.Details{
public static class Logging{

    internal static status Status(in status @base, V reason,
                                  S p, S m, int l)
    => log ? ViaScope(@base, F.SysTrace(p, m, l), reason) : @base;

    internal static action Action(V reason, S p, S m, int l) => log
    ? new action(ViaScope(_done, F.SysTrace(p,m,l), reason).meta)
    : action._void;

    internal static failure Failure(V reason, S p, S m, int l) => log
    ? new failure(ViaScope(_fail, F.SysTrace(p,m,l), reason).meta)
    : failure._false;

    internal static loop Forever(V reason, S p, S m, int l) => log
    ? new loop(ViaScope(_cont, F.SysTrace(p,m,l), reason).meta)
    : loop._cont;

    internal static pending Pending(pending @base, V reason,
                                    string p, string m, int l) => log
    ? new pending(@base.ω, ViaScope(@base.due,
                                    F.SysTrace(p,m,l), reason).meta)
    : @base;

    internal static impending Impending(impending @base, V reason,
                                        string p, string m, int l) => log
    ? new impending(@base.ω, ViaScope(@base.undue,
                                      F.SysTrace(p,m,l), reason).meta)
    : @base;

    internal static status ViaScope(in status s, object scope,
                                                 string reason)
    => new status(s.ω, s.meta.ViaScope(s, scope, reason));

}}  // Active.Core.Details

#endif  // !AL_OPTIMIZE
