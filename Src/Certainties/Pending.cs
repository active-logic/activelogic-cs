// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System; using InvOp = System.InvalidOperationException;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct pending{

    internal readonly int ω;
    internal static readonly pending _cont = new pending( 0),
                                     _done = new pending(+1);
    #if !AL_OPTIMIZE
    readonly Meta meta;
    #endif

    internal pending(int val) {
        ω = val;
        #if !AL_OPTIMIZE
        meta = new Meta();
        #endif
    }

    public bool running  => ω ==  0;
    public bool complete => ω >=  1;

    // --------------------------------------------------------------

    public static pending operator & (pending x, pending y) => y;

    // Disable `pending || status` via implicit conversion
    public static pending operator | (pending x, status y)
    => throw new InvOp("{x} | {y} is not allowed");

    public static bool operator true (pending s)
    => throw new InvOp("Cannot test truehood (pending)");

    public static bool operator false(pending s) => s.ω !=  1;

    public override bool Equals(object x)
    => x is pending && Equals((pending)x);

    public bool Equals(in pending x) => this.ω == x.ω;

    override public int GetHashCode() => ω;

    public override string ToString()
    => ω == 0 ? "pending.cont" : "pending.done";

    #if AL_OPTIMIZE   // ---------------------------------------------

    public status due => new status(ω);

    public static impending operator !(pending s)
    => new impending(-s.ω);

    public static pending cont(ValidString reason = null) => _cont;

    public static pending done(ValidString reason = null) => _done;

    public static implicit operator status(pending self)
    => new status(self.ω);

    #else  // !AL_OPTIMIZE

    public static implicit operator status(pending self)
    => new status(self.ω, self.meta);

    #endif

}}
