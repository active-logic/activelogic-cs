// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System; using InvOp = System.InvalidOperationException;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct impending{

    public static impending[] values = {_fail, _cont};

    internal readonly int ω;
    internal static readonly impending _cont = new impending( 0);
    internal static readonly impending _fail = new impending(-1);

    [Obsolete("Use fail instead", true)]
    internal static readonly impending _doom = new impending(-1);

    #if !AL_OPTIMIZE
    readonly Meta meta;
    #endif

    internal impending(int val) {
        ω = val;
        #if !AL_OPTIMIZE
        meta = new Meta();
        #endif
    }

    public bool failing  => ω <= -1;
    public bool running  => ω ==  0;

    // --------------------------------------------------------------

    public static impending operator | (impending x, impending y) => y;

    // Disable `impending && status` via implicit conversion
    public static impending operator & (impending x, status y)
    => throw new InvOp("{x} & {y} is not allowed");

    public static bool operator true (impending s)
    => s.ω != -1;

    // Disable `impending && T`, even when right hand is dynamic
    public static bool operator false(impending s)
    => throw new InvOp("Cannot test falsehood (impending)");

    public override bool Equals(object x)
    => x is impending && Equals((impending)x);

    public bool Equals(in impending x) => true;

    override public int GetHashCode() => ω;

    override public string ToString()
    => ω == -1 ? "impending.fail" : "impending.cont";

    #if AL_OPTIMIZE  // ----------------------------------------------

    public status undue => new status(ω);

    public static pending operator ! (impending s)
    => new pending(-s.ω);

    public static impending cont(ValidString reason = null) => _cont;
    public static impending fail(ValidString reason = null) => _fail;

    [Obsolete("Use fail instead", false)]
    public static impending doom(ValidString reason = null) => _doom;

    public static implicit operator status(impending self)
    => new status(self.ω);

    #else  // !AL_OPTIMIZE

    public static implicit operator status(impending self)
    => new status(self.ω, self.meta);

    #endif

}}
