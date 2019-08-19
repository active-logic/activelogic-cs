// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using System;
using Active.Core.Details;

namespace Active.Core{

public readonly partial struct action{  // Always 'done' (a typed 'void')

    internal static readonly action _void = new action();

    public static action operator % (action x, action y) => _void;
    public static failure operator ! (action s) => failure._flop;

}

public readonly partial struct failure{

    internal static readonly failure _flop = new failure();

    public static failure operator % (failure x, failure y) => _flop;
    public static action  operator ! (failure s) => new action(s.meta);

}

public readonly partial struct loop{

    internal static readonly loop _forever = new loop();

    public static loop operator % (loop x, loop y) => _forever;

}

public readonly partial struct pending{

    internal readonly int ω;
    readonly Meta meta;

    internal static readonly pending _cont = new pending( 0),
                                     _done = new pending(+1);

    internal pending(int val) { ω = val;  meta = new Meta(); }

    public bool running  => ω ==  0;
    public bool complete => ω >=  1;
    public status due => new status(ω, meta);

    public static pending operator & (pending x, pending y) => y;
    public static pending operator | (pending x, pending y) => y;

    public static impending operator !(pending s)
    => new impending(-s.ω, s.meta);

    public static bool operator true  (pending s)
    => throw new InvalidOperationException("pending is always 'true'");

    public static bool operator false (pending s) => s.ω == 0;

}

public readonly partial struct impending{

    internal readonly int ω;
    readonly Meta meta;

    internal static readonly impending _cont = new impending( 0);
    internal static readonly impending _doom = new impending(-1);

    internal impending(int val) { ω = val; meta = new Meta(); }

    public bool failing  => ω <= -1;
    public bool running  => ω ==  0;

    public static impending operator & (impending x, impending y) => y;
    public static impending operator | (impending x, impending y) => y;

    public static pending operator ! (impending s)
    => new pending(-s.ω, s.meta);

    public static bool operator true  (impending s) => s.ω == 0;

    public static bool operator false (impending s)
    => throw new InvalidOperationException("impending is always 'false'");

}

} // end-namespace

#endif  // !AL_OPTIMIZE
