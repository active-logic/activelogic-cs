// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public readonly partial struct action{

    internal static readonly action _done = new action();

    public static action operator % (action x, action y) => _done;

    public static action    operator & (action x, action    y) => y;
    public static status    operator & (action x, status    y) => y;
    public static failure   operator & (action x, failure   y) => y;
    public static loop      operator & (action x, loop      y) => y;
    public static pending   operator & (action x, pending   y) => y;
    public static impending operator & (action x, impending y) => y;
    public static bool      operator & (action x, bool      y) => y;

    public static loop operator - (action x) => loop._cont;

    public static bool operator true (action s) => true;
    public static bool operator false(action s) => false;

    #if AL_OPTIMIZE   // --------------------------------------------

    public static action done(ValidString reason = null) => _done;

    public static failure operator ! (action s) => failure._fail;
    #endif

    public static implicit operator bool(action self)
    => true;

    public static implicit operator status(action self)
    => status._done;

    public static implicit operator pending(action self)
    => pending._done;

}}
