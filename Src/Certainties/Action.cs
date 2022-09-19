// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using InvOp = System.InvalidOperationException;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct action{

    public static action[] values = {_done};

    internal static readonly action _done = new action();

    // --------------------------------------------------------------

    public static action operator % (action x, action y) => _done;
    public static action operator & (action x, action y) => y;

    public static action operator | (action x, status y)
    => throw new InvOp($"({x} | {y}) is not allowed");

    public static loop operator - (action x) => loop._cont;

    // NOTE: required since `false` is implemented; there is not
    // a known instance where this function would be called.
    public static bool operator true(action s)
    => throw new InvOp("truehood cannot be tested (action)");

    // NOTE: used in action && action
    public static bool operator false (action s) => false;

    public override bool Equals(object x) => x is action;

    override public int GetHashCode() => 1;

    public static implicit operator bool(action self) => true;

    #if AL_OPTIMIZE   // --------------------------------------------

    public static action done(ValidString reason = null) => _done;

    public static failure operator ! (action s) => failure._fail;

    public static implicit operator status(action self)
    => status._done;

    public static implicit operator pending(action self)
    => pending._done;

    #endif

}}
