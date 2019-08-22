#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif
#if AL_OPTIMIZE

using Active.Core.Details;

namespace Active.Core{
public readonly partial struct status{

    readonly int ω;

    internal status(int value, LogTrace trace=null, status[] children = null)
    => ω = value;

    internal status(StatusValue value, LogTrace trace=null) => ω = (int)value;

    status(int value)                              => ω = value;
    status(in status s, LogTrace trace)            => this = s;
    status(in status s, int value)                 => ω = value;
    status(in status s, in status prev)            => this = s;
    status(in status s, in status prev, int value) => ω = value;

    public static status operator & (status x, status y) => y;
    public static status operator | (status x, status y) => y;

    public static implicit operator status(bool state)
    => state ? done() : fail();

    // TODO: use raw
    public static status operator ++ (status s) => (+s).due;
    public static status operator -- (status s) => (-s).undue;

    public static bool   operator == (in status x, in status y) =>  x.Equals(y);
    public static bool   operator != (in status x, in status y) => !x.Equals(y);
    public static bool operator true (status s) => s.ω != -1;
    public static bool operator false(status s) => s.ω !=  1;

    public override bool   Equals(object x) => x is status && Equals((status)x);
    public          bool   Equals(in status x) => this.ω == x.ω;
    public override int    GetHashCode() => ω;
    internal static status @unchecked(int value)
    => new status(value);

    internal status ViaScope(object scope, string reason=null) => this;

    static LogTrace LogTrace(object scope, string reason=null) => null;

}}

#endif  // AL_OPTIMIZE
