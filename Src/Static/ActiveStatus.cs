// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core;
using V = Active.Core.Details.ValidString;
using LS = Active.Core.Details.LogString;

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

    public static status    Eval(status    s) => s;
    public static action    Eval(action    s) => s;
    public static failure   Eval(failure   s) => s;
    public static loop      Eval(loop      s) => s;
    public static pending   Eval(pending   s) => s;
    public static impending Eval(impending s) => s;
    public static bool      Eval(bool      s) => s;

    public static status    ε(status    s) => s;
    public static action    ε(action    s) => s;
    public static failure   ε(failure   s) => s;
    public static loop      ε(loop      s) => s;
    public static pending   ε(pending   s) => s;
    public static impending ε(impending s) => s;
    public static status    ε(bool      s) => s;

    public static action  Do   (object arg) => action._done;
    public static loop    Cont (object arg) => loop._cont;
    public static failure Fail (object arg) => failure._fail;

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

    // ==============================================================

    public static status undef(status @value,
                               [P] S p="", [M] S m="", [L] int l=0)
    => status.log ? Lg.Status(@value,
                             new LS("undef", true), p, m, l)
                  : @value;

    public static status undef([P] S p="", [M] S m="", [L] int l=0)
    => status.log ? Lg.Status(status._fail,
                             new LS("undef", true), p, m, l)
                  : status._fail;

    // ==============================================================

    public static status Eval(status s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Status(s, null, path, member, line) : s;

    public static action Eval(action s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Action(null, path, member, line) : s;

    public static failure Eval(failure s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Failure(null, path, member, line) : s;

    public static loop Eval(loop s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Forever(null, path, member, line) : s;

    public static pending Eval(pending s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Pending(s, null, path, member, line) : s;

    public static status Eval(bool s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Status(s, null, path, member, line) : s;

    public static impending Eval(impending s,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Impending(s, null, path, member, line) : s;

    public static action Do(object arg,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Action(null, path, member, line) : action._done;

    public static loop Cont(object arg,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Forever(null, path, member, line) : loop._cont;

    public static failure Fail(object arg,
                       [P] S path="", [M] S member="", [L] int line=0)
    => status.log ? Lg.Failure(null, path, member, line) : failure._fail;

    public static status ε(status s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static action ε(action s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static failure ε(failure s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static loop ε(loop s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static pending ε(pending s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static impending ε(impending s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    public static status ε(bool s, [P] S path="", [M] S member="",
    [L] int line=0) => Eval(s, path, member, line);

    #endif  // (!)AL_OPTIMIZE

}}
