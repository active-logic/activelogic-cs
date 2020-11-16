// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using InvOp = System.InvalidOperationException;
using System; using Active.Core.Details;

namespace Active.Core{
public readonly partial struct failure{

    [Obsolete("Use _fail instead", true)]
    internal static readonly failure _flop = new failure();
    internal static readonly failure _fail = new failure();

    // --------------------------------------------------------------

    public static failure operator % (failure x, failure y) => _fail;
    public static failure operator | (failure x, failure y) => y;

    public static failure operator & (failure x, status y)
    => throw new InvOp($"({x} & {y}) is not allowed");

    public static loop operator + (failure x) => loop._cont;

    public static bool operator true  (failure s) => false;
    public static bool operator false (failure s)
    => throw new InvOp("falsehood cannot be tested (failure)");

    #if AL_OPTIMIZE   // --------------------------------------------

    public static failure fail(ValidString reason = null) => _fail;

    public static action operator ! (failure s) => action._done;

    #endif

    public static implicit operator bool(failure self)
    => false;

    public static implicit operator impending(failure self)
    => impending._fail;

    public static implicit operator status(failure self)
    => status._fail;

}}
