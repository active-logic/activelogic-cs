#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif
#if !AL_OPTIMIZE

using Active.Core.Details;
using ArgEx = System.ArgumentException;
using S     = Active.Core.Strings;

namespace Active.Core{
public readonly partial struct status{

    internal static bool log = true;
    internal static readonly status
        _done = new status(+1, @unchecked: true),
        _fail = new status(-1, @unchecked: true),
        _cont = new status( 0, @unchecked: true);
    internal readonly Meta meta;
    internal readonly int ω;

    internal status(int value, in Meta meta)
    { this.ω=value; this.meta=meta; }

    internal status(int value, LogTrace trace=null,
                               status[] children = null){
        ω    = Validate(value);
        meta = status.log
            ? new Meta(trace == null ? throw new ArgEx()
                                     : trace, children)
            : new Meta();
    }

    internal status(StatusValue value, LogTrace trace=null){
        ω    = (int)value;
        meta = new Meta(trace);
    }

    // Note: Dead param prevents confusion with opt param alts.
    status(int value, bool @unchecked){
        ω    = value;
        meta = new Meta();
    }

    internal status(in status s, LogTrace trace){
        this = s;
        meta = new Meta(meta, trace);
    }

    status(in status s, int value){ this = s; ω = value; }

    status(in status s, in status prev){
        this = s;
        meta = log ? new Meta(meta, prev: new Ref(prev))
                   : new Meta(meta);
    }

    status(in status s, in status prev, int value){
        this = s; ω = value;
        meta = log ? new Meta(meta, prev: new Ref(prev))
                   : new Meta(meta);
    }

    internal LogTrace trace       => meta.trace;
    internal object   targetScope => trace?.scope;

    public static status operator & (status x, status y)
    => (x.ω < +1) ? throw new ArgEx(S.UnexpectedValue)
                  : new status(y, x);

    public static status operator | (status x, status y)
    => (x.ω > -1) ? throw new ArgEx(S.UnexpectedValue)
                  : new status(y, x);

    public static implicit operator status(bool state)
    => state ? done() : fail();

    public static status operator ++ (status s) => +s;

    public static status operator -- (status s) => -s;

    public static bool operator == (in status x, in status y)
    => x.Equals(y);

    public static bool operator != (in status x, in status y)
    => !x.Equals(y);

    public static bool operator true (status s) => s.ω != -1;

    public static bool operator false(status s) => s.ω !=  1;

    public override bool Equals(object x)
    => x is status && Equals((status)x);

    public bool Equals(in status x) => this.ω == x.ω;

    public override int GetHashCode() => ω;

    public override string ToString() => StatusFormat.ToString(this);

    internal static status @unchecked(int value)
    => new status(value, @unchecked: true);

    static int Validate(int ω)
    => (ω < -1 || ω > +1) ? throw new ArgEx(ω.ToString()) : ω;

    internal class Ref{
        public status value;
        public Ref(in status s){ value = s; }
    }

}}

#endif  // AL_OPTIMIZE
