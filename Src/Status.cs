// Doc/Reference/Status.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public readonly partial struct status{

  #if !AL_OPTIMIZE

    public   bool failing  => ω <= -1;
    public   bool running  => ω ==  0;
    public   bool complete => ω >=  1;
    internal int  raw      => ω;

    public status Map(in status failTo, in status contTo, in status doneTo){
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

    public static pending operator ~ (in status s)
    { if(log) s.trace?.Prefix('~'); return new pending(s.ω * s.ω, s.meta); }

    public static pending operator + (in status s){
        if(log) s.trace?.Prefix('+');
        return new pending(System.Math.Min(s.ω + 1, +1), s.meta);
    }

    public static impending operator - (in status s){
        if(log) s.trace?.Prefix('-');
        return new impending(System.Math.Max(s.ω - 1, -1), s.meta);
    }

  #if !AL_STRICT
    public static implicit operator status(bool that) => that?done():fail();
  #endif

  #else  // !AL_OPTIMIZE <> AL_OPTIMIZE

    public static status Eval(status s) => s;

    public static status  done   (ValidString reason = null) => _done;
    public static status  fail   (ValidString reason = null) => _fail;
    public static status  cont   (ValidString reason = null) => _cont;
    public static action  @void  (ValidString reason = null) => action._void;
    public static failure flop (ValidString reason = null) => failure._flop;
    public static loop    forever(ValidString reason = null) => loop._forever;

  #endif  // AL_OPTIMIZE

}

#if AL_OPTIMIZE

public static class BoolExt{

    public static Active.Core.status status(this bool self,
                                            ValidString reason = null)
    => self ? Active.Core.status._done : Active.Core.status._fail;

}

#endif  // AL_OPTIMIZE

}  // Active.Core
