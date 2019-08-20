// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core;
using Active.Core.Details;
using F  = Active.Core.Details.StatusFormat;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using V  = Active.Core.Details.ValidString;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using S  = System.String;
using X  = Active.Core.status;
#if !AL_OPTIMIZE
using Lg = Active.Core.Details.Logging;
#endif
using static Active.Core.status;

#if !AL_OPTIMIZE

namespace Active.Core{

partial struct action{

    readonly Meta meta;

    internal action(Meta meta) { this.meta = meta; }

    public status now => new status(1, meta);

    public action Via(V reason = null, [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Action(reason, p, m, l);

}

partial struct failure{

    readonly Meta meta;

    public status fail => new status(-1, meta);

    internal failure(Meta meta) { this.meta = meta; }

    public failure Via(V reason = null, [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Failure(reason, p, m, l);
}

partial struct loop{

    readonly Meta meta;

    public loop(Meta meta) { this.meta = meta; }

    public status ever => new status(0, meta);

    public loop Via(V reason = null, [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Forever(reason, p, m, l);

}

partial struct pending{

    internal pending(int val, Meta m) { ω = val; meta = m; }

    public static pending cont(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Pending(pending._cont, reason, p, m, l);

    public static pending done(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Pending(pending._done, reason, p, m, l);

    public pending Via(V reason = null, [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Pending(this, reason, p, m, l);

}

partial struct impending{

    internal impending(int val, Meta m) { ω = val; meta = m; }

    public status undue => new status(ω, meta);

    public static impending cont(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(impending._cont, reason, p, m, l);

    public static impending doom(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(impending._doom, reason, p, m, l);

    public impending Via(V reason = null, [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(this, reason, p, m, l);

}

public static class BoolExt{

    public static X status(this bool self, ValidString reason = null,
                          [P]string path="", [M]string member="", [L]int line=0)
    => X.log ? Lg.Status(self ? X._done : X._fail, reason, path, member, line)
             : self ? X._done : X._fail;

}

}  // Active.Core

// ============================================================================

namespace Active.Core.Details{
public static class Logging{

    internal static status Status(in status @base, V reason, S p, S m, int l)
    => log ? ViaScope(@base, F.SysTrace(p, m, l), reason) : @base;

    internal static action Action(V reason, S p, S m, int l) => log ?
    new action(ViaScope(_done, F.SysTrace(p,m,l), reason).meta) : action._void;

    internal static failure Failure(V reason, S p, S m, int l) => log ?
    new failure(ViaScope(_fail, F.SysTrace(p,m,l), reason).meta):failure._flop;

    internal static loop Forever(V reason, S p, S m, int l) => log ?
    new loop(ViaScope(_cont, F.SysTrace(p,m,l), reason).meta):loop._forever;

    internal static pending Pending(pending @base, V reason,
       string p, string m, int l) => log ? new pending(
         @base.ω, ViaScope(@base.due, F.SysTrace(p,m,l), reason).meta) : @base;

    internal static impending Impending(impending @base, V reason,
       string p, string m, int l) => log ? new impending(
       @base.ω, ViaScope(@base.undue, F.SysTrace(p,m,l), reason).meta) : @base;

    internal static status ViaScope(in status s, object scope, string reason)
    => new status(s.ω, s.meta.ViaScope(s, scope, reason));

}}  // Active.Core.Details

#endif  // !AL_OPTIMIZE
