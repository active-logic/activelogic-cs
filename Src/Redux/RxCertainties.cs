// Doc/Reference/Certainties.md

using InvOp = System.InvalidOperationException;

namespace Active.Rx{


public readonly struct action{  // Always 'done' (a typed 'void')

    public static readonly action @void = new action();

    public status now => status.done;

    public static action operator % (action x, action y) => @void;
    public static failure operator ! (action s) => failure.flop;

}

public readonly struct failure{  // Always 'fail'

    public static readonly failure flop = new failure();

    public status fail => status.done;

    public static failure operator % (failure x, failure y) => flop;
    public static action  operator ! (failure s) => action.@void;

}

public readonly struct pending{  // Never failing status

    readonly bool ω;

    pending(bool doneYet) => ω = doneYet;

    public static readonly pending cont = new pending(doneYet: false),
                                   done = new pending(doneYet: true);

    public static pending operator & (pending x, pending y) => y;
    public static pending operator | (pending x, pending y) => y;
    public static impending operator ! (pending s)
    => s.ω ? impending.doom : impending.cont;

    public static bool operator true  (pending s)
    => throw new InvOp("pending is always 'true'");
    public static bool operator false (pending s) => !s.ω;

    public status due => ω ? status.done : status.cont;

}

public readonly struct impending{  // Never done status

    readonly bool ω;

    impending(bool failedYet) => ω = failedYet;

    public static readonly impending cont = new impending(failedYet: false);
    public static readonly impending doom = new impending(failedYet: true);

    public static impending operator & (impending x, impending y) => y;
    public static impending operator | (impending x, impending y) => y;

    public static pending operator ! (impending s)
    => s.ω ? pending.done : pending.cont;

    public static bool operator true  (impending s) => !s.ω;
    public static bool operator false (impending s)
    => throw new InvOp("impending is always 'false'");

    public status never => ω ? status.fail : status.cont;

}

public readonly struct loop{  // Ever running status

    public static readonly loop forever = new loop();

    public status ever => status.cont;

    public static loop operator % (loop x, loop y) => forever;

}

// ----------------------------------------------------------------------------

} // end-namespace
