// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using System;
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
using static Active.Status;

namespace Active.Core{

partial struct action{

    readonly Meta meta;

    internal action(Meta meta) { this.meta = meta; }

    //public status now => new status(1, meta);

    public static failure operator ! (action s) => new failure(s.meta);

    public action Via(V reason = null,
                      [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Action(reason, p, m, l);

    public static action done(V reason = null,
                              [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Action(reason, p, m, l);

}

partial struct failure{

    readonly Meta meta;

    //public status never => new status(-1, meta);

    //public status fail => new status(-1, meta);

    public static action operator ! (failure s) => new action(s.meta);

    internal failure(Meta meta) { this.meta = meta; }

    public static failure fail(ValidString reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Failure(reason, p, m, l);

    public failure Via(V reason = null,
                       [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Failure(reason, p, m, l);

}

partial struct loop{

    readonly Meta meta;

    public loop(Meta meta) { this.meta = meta; }

    public status ever => new status(0, meta);

    public static loop cont(ValidString reason = null,
                            [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Forever(reason, p, m, l);

    public loop Via(V reason = null, [P]S p="", [M]S m="", [L]int l=0)
    => Lg.Forever(reason, p, m, l);

}

partial struct pending{

    internal pending(int val, Meta m) { ω = val; meta = m; }

    public status due => new status(ω, meta);

    public static impending operator !(pending s)
    => new impending(-s.ω, s.meta);

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

    public static pending operator !(impending s)
    => new pending(-s.ω, s.meta);

    public static impending cont
      (ValidString reason = null, [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(impending._cont, reason, p, m, l);

    public static impending fail
      (ValidString reason = null, [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(impending._fail, reason, p, m, l);

    [Obsolete("Use fail instead", false)]
    public static impending doom
      (ValidString reason = null, [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(impending._fail, reason, p, m, l);

    public impending Via
      (V reason = null, [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Impending(this, reason, p, m, l);

}

}  // Active.Core

#endif  // !AL_OPTIMIZE
