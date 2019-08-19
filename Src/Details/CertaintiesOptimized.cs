// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if AL_OPTIMIZE

using Active.Core.Details;
using InvOp = System.InvalidOperationException;

namespace Active.Core{

public readonly partial struct action{  // Always 'done' (a typed 'void')

    internal static readonly action _void = new action();

    public status now => new status(1);

    public static action operator % (action x, action y) => _void;
    public static failure operator ! (action s) => failure._flop;

}

public readonly partial struct failure{  // Always 'fail'

    internal static readonly failure _flop = new failure();

    public status fail => new status(-1);

    public static failure operator % (failure x, failure y) => _flop;
    public static action  operator ! (failure s) => new action();

}

public readonly partial struct loop{  // Ever running status

    internal static readonly loop _forever = new loop();

    public status ever => new status(0);

    public static loop operator % (loop x, loop y) => _forever;

}

public readonly partial struct pending{  // Never failing status

    internal static readonly pending _cont = new pending( 0),
                                     _done = new pending(+1);
    internal readonly int ω;

    internal pending(int val) { ω = val; }

    public bool running  => ω ==  0;
    public bool complete => ω >=  1;
    public status due => new status(ω);

    public static pending cont(ValidString reason = null)
    => _cont;

    public static pending done(ValidString reason = null)
    => _done;

    public static pending operator & (pending x, pending y) => y;
    public static pending operator | (pending x, pending y) => y;

    public static impending operator !(pending s)
    => new impending(-s.ω);

    public static bool operator true  (pending s)
    => throw new InvOp("pending is always 'true'");

    public static bool operator false (pending s) => s.ω == 0;

}

public readonly partial struct impending{  // Never done status

    internal readonly int ω;

    internal impending(int val) { ω = val; }

    public status undue => new status(ω);

    internal static readonly impending _cont = new impending( 0);
    internal static readonly impending _doom = new impending(-1);

    public bool failing  => ω <= -1;
    public bool running  => ω ==  0;

    public static impending doom(ValidString reason = null) => _doom;

    public static impending operator & (impending x, impending y) => y;
    public static impending operator | (impending x, impending y) => y;

    public static pending operator ! (impending s)
    => new pending(-s.ω);

    public static bool operator true  (impending s) => s.ω == 0;

    public static bool operator false (impending s)
    => throw new InvOp("impending is always 'false'");

}

// ----------------------------------------------------------------------------

} // end-namespace

#endif  // AL_OPTIMIZE
