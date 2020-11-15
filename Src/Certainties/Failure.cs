// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System; using Active.Core.Details;

namespace Active.Core{
public readonly partial struct failure{

    [Obsolete("Use _fail instead", true)]
    internal static readonly failure _flop = new failure();
    internal static readonly failure _fail = new failure();

    public static failure   operator | (failure x, failure   y) => y;
    public static status    operator | (failure x, status    y) => y;
    public static action    operator | (failure x, action    y) => y;
    public static loop      operator | (failure x, loop      y) => y;
    public static pending   operator | (failure x, pending   y) => y;
    public static impending operator | (failure x, impending y) => y;

    public static failure operator % (failure x, failure y) => _fail;

    public static loop operator + (failure x) => loop._cont;

    public static bool operator true  (failure s) => false;
    public static bool operator false (failure s) => true;

    #if AL_OPTIMIZE   // --------------------------------------------

    public static failure fail(ValidString reason = null) => _fail;

    public static action operator ! (failure s) => action._done;

    #endif

    public static implicit operator impending(failure self)
    => impending._fail;

    public static implicit operator status(failure self)
    => status._fail;

}}
