// Doc/Reference/Status.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;
using S = Active.Core.Details.ValidString;

namespace Active.Core{
public readonly partial struct status{

    #if !AL_OPTIMIZE

    public   bool failing  => ω <= -1;
    public   bool running  => ω ==  0;
    public   bool complete => ω >=  1;
    internal int  raw      => ω;

    public status Map(in status failTo, in status contTo,
                                        in status doneTo){
        switch(ω){
            case -1: return new status(this, failTo.ω);
            case  0: return new status(this, contTo.ω);
            case +1: return new status(this, doneTo.ω);
            default: throw new System.ArgumentException();
        }
    }

    // NOTE: `&&` and `||` are not explicitly implemented.

    public static status operator + (in status x, in status y)
    => new status(y, x, System.Math.Max(x.ω, y.ω));

    public static status operator * (in status x, in status y)
    => new status(y, x, System.Math.Min(x.ω, y.ω));

    public static status operator % (in status x, in status y)
    => new status(y, x, x.ω);

    public static status operator ! (in status s)
    { if(log) s.trace?.Prefix('!'); return new status(s, -s.ω); }

    public static status operator ~ (in status s){
        if(log) s.trace?.Prefix('~');
        return new status(s.ω * s.ω, s.meta);
    }

    public static status operator + (in status s){
        if(log) s.trace?.Prefix('+');
        return new status(System.Math.Min(s.ω + 1, +1), s.meta);
    }

    public static status operator - (in status s){
        if(log) s.trace?.Prefix('-');
        return new status(System.Math.Max(s.ω - 1, -1), s.meta);
    }

    #else  // !AL_OPTIMIZE <> AL_OPTIMIZE

    public status Via(S reason = null) => this;

    public status ViaDecorator(IDecorator scope, S reason=null)
    => this;

    [Obsolete("Import Active.Status/Active.Raw", false)]
    public static status Eval(status s) => s;

    public static status done(S reason = null) => _done;
    public static status fail(S reason = null) => _fail;
    public static status cont(S reason = null) => _cont;

    [Obsolete("Import Active.Status", false)]
    public static action  @void  (S reason = null) => action._done;
    //[Obsolete("Import Active.Status", false)]
    //public static failure @false (S reason = null) => failure._fail;
    [Obsolete("Use '@false' instead", false)]
    public static failure flop (S reason = null) => failure._fail;
    [Obsolete("Use loop.cont instead", false)]
    public static loop forever(S reason = null) => loop._cont;

    #endif  // AL_OPTIMIZE

}

#if AL_OPTIMIZE

public static class BoolExt{

    public static Active.Core.status status(this bool self,
                                            S reason = null)
    => self ? Active.Core.status._done : Active.Core.status._fail;

}

#endif  // AL_OPTIMIZE

}  // Active.Core
