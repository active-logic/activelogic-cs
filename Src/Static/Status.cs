// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core;
using V = Active.Core.Details.ValidString;

#if !AL_OPTIMIZE
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Lg = Active.Core.Details.Logging;
using S = System.String;
#endif

namespace Active{
public static class Status{

    #if AL_OPTIMIZE

    public static status done(V reason = null) => status._done;
    public static status fail(V reason = null) => status._fail;
    public static status cont(V reason = null) => status._cont;

    public static action  @void  (V reason = null) => action._done;
    public static failure @false (V reason = null) => failure._fail;
    public static loop    forever(V reason = null) => loop._cont;

    public static status Eval(status s) => s;

    public static status ε(status s) => s;

    #else  // logging variants ======================================

    internal static bool log => status.log;

    public static status done(V reason = null,
                              [P] S p="", [M] S m="", [L] int l=0)
    => status.log ? Lg.Status(status._done, reason, p, m, l)
                  : status._done;

    public static status fail(V reason = null,
                              [P] S p="", [M] S m="", [L] int l=0)
    => status.log ? Lg.Status(status._fail, reason, p, m, l)
                  : status._fail;

    public static status cont(V reason = null,
                              [P] S p="", [M] S m="", [L] int l=0)
    => status.log ? Lg.Status(status._cont, reason, p, m, l)
                  : status._cont;

    public static action @void(V reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Action(reason, p, m, l);

    public static failure @false(V reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Failure(reason, p, m, l);

    public static loop forever(V reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Forever(reason, p, m, l);

    public static status Eval(status s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Status(s, null, path, member, line) : s;

    public static status ε(status s, [P] S path="", [M] S member="",
                                     [L] int line=0)
    => Eval(s, path, member, line);

    #endif  // (!)AL_OPTIMIZE

}}
