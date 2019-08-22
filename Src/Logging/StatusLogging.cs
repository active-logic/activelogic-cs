// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE
using Active.Core.Details;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using V  = Active.Core.Details.ValidString;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using S  = System.String;
using Lg = Active.Core.Details.Logging;
using X  = Active.Core.status;

namespace Active.Core{

partial struct status{

    public status this[ValidString reason]
    => log ? new status( this, new LogTrace(trace.scope, trace.next, reason))
           : this;

    public static status Eval(status s,
                             [P] S path="", [M] S member="", [L] int line=0)
    => log ? Lg.Status(s, null, path, member, line) : s;

    public status Via(ValidString reason = null,
                     [P] string path="", [M] string member="", [L] int line=0)
    => log ? Lg.Status(this, reason, path, member, line) : this;

    public status ViaDecorator(IDecorator scope, ValidString reason=null)
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
    [P] S p="", [M] S m="", [L] int l=0) => Lg.Action(reason, p, m, l);

    public static failure flop(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0) => Lg.Failure(reason, p, m, l);

    public static loop forever(ValidString reason = null,
    [P] S p="", [M] S m="", [L] int l=0) => Lg.Forever(reason, p, m, l);

}  // partial status

public static class BoolExt{

    public static X status(this bool self, ValidString reason = null,
                          [P]string path="", [M]string member="", [L]int line=0)
    => X.log ? Lg.Status(self ? X._done : X._fail, reason, path, member, line)
             : self ? X._done : X._fail;

}

}  // Active.Core

#endif  // !AL_OPTIMIZE
