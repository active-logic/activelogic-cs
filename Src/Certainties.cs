// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;

namespace Active.Core{

public readonly partial struct action{
    internal static readonly action _void = new action();
  #if AL_OPTIMIZE
    public status now => status._done;
  #endif  // AL_OPTIMIZE
    public static action operator % (action x, action y) => _void;
  #if AL_OPTIMIZE
    public static failure operator ! (action s) => failure._flop;
  #endif
  #if !AL_STRICT
    public static implicit operator status(action self) => self.now;
  #endif

}

public readonly partial struct failure{
    internal static readonly failure _flop = new failure();
  #if AL_OPTIMIZE
    public status fail => status._fail;
  #endif  // AL_OPTIMIZE
    public static failure operator % (failure x, failure y) => _flop;
  #if AL_OPTIMIZE
    public static action  operator ! (failure s) => action._void;
  #endif
  #if !AL_STRICT
    public static implicit operator status(failure self) => self.fail;
  #endif
}

public readonly partial struct loop{
    internal static readonly loop _forever = new loop();
  #if AL_OPTIMIZE
    public status ever => status._cont;
  #endif  // AL_OPTIMIZE
    public static loop operator % (loop x, loop y) => _forever;
  #if !AL_STRICT
    public static implicit operator status(loop self) => self.ever;
  #endif  // !AL_STRICT
}

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

  #if AL_OPTIMIZE
    public status due => new status(ω);
  #endif
    public bool running  => ω ==  0;
    public bool complete => ω >=  1;

    public static pending operator & (pending x, pending y) => y;
    public static pending operator | (pending x, pending y) => y;
  #if AL_OPTIMIZE
    public static impending operator !(pending s) => new impending(-s.ω);
  #endif

    public static bool operator true  (pending s)
    => throw new InvalidOperationException("pending is always 'true'");

    public static bool operator false (pending s) => s.ω == 0;

  #if !AL_STRICT
    public static implicit operator status(pending self) => self.due;
  #endif  // !AL_STRICT

}

public readonly partial struct impending{

    internal readonly int ω;
    internal static readonly impending _cont = new impending( 0);
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

  #if AL_OPTIMIZE
    public status undue => new status(ω);
  #endif
    public bool failing  => ω <= -1;
    public bool running  => ω ==  0;

    public static impending operator & (impending x, impending y) => y;
    public static impending operator | (impending x, impending y) => y;
  #if AL_OPTIMIZE
    public static pending operator ! (impending s) => new pending(-s.ω);
  #endif

    public static bool operator true  (impending s) => s.ω == 0;

    public static bool operator false (impending s)
    => throw new InvalidOperationException("impending is always 'false'");

  #if !AL_STRICT
      public static implicit operator status(impending self) => self.undue;
  #endif  // !AL_STRICT

}

} // end-namespace
