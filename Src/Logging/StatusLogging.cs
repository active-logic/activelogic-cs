// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE
using System;
using S  = System.String;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;
using V  = Active.Core.Details.ValidString;
using Lg = Active.Core.Details.Logging;
using X  = Active.Core.status;

namespace Active.Core{

partial struct status{

    public status this[ValidString reason]
    => log ? new status(ω, Meta.From(meta, reason)) : this;

    [Obsolete("Use via Active.Raw or Active.Status", false)]
    public static status ε(status s, [P] S path="", [M] S member="",
                                     [L] int line=0)
    => Eval(s, path, member, line);

    [Obsolete("Use via Active.Status or Active.Raw", false)]
    public static status Eval(status s, [P] S path="", [M] S member="",
                                        [L] int line=0)
    => log ? Lg.Status(s, null, path, member, line) : s;

    public status Via(ValidString reason = null,
                     [P] string path="", [M] string member="",
                     [L] int line=0)
    => log ? Lg.Status(this, reason, path, member, line) : this;

    public status ViaDecorator(IDecorator scope, V reason=null)
    => log ? Lg.ViaScope(this, scope, reason) : this;

    public static status done(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => log ? Lg.Status(_done, reason, p, m, l) : _done;

    public static status fail(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => log ? Lg.Status(_fail, reason, p, m, l) : _fail;

    public static status cont(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0)
    => log ? Lg.Status(_cont, reason, p, m, l) : _cont;

    public static action @void(ValidString reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Action(reason, p, m, l);

    [Obsolete("Use '@false' instead", false)]
    public static failure flop(ValidString reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Failure(reason, p, m, l);

    [Obsolete("Use 'loop.cont' instead", false)]
    public static loop forever(ValidString reason = null,
                               [P] S p="", [M] S m="", [L] int l=0)
    => Lg.Forever(reason, p, m, l);

}  // partial status

public static class BoolExt{

    public static X status(this bool self, ValidString reason = null,
                          [P]string path="", [M]string member="",
                          [L]int line=0) => X.log
    ? Lg.Status(self ? X._done : X._fail, reason, path, member, line)
    : self ? X._done : X._fail;

}

}  // Active.Core

#endif  // !AL_OPTIMIZE
