// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;
using System;

namespace Active.Core{
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

    public bool failing  => ω <= -1;
    
    public bool running  => ω ==  0;

    public static impending operator & (impending x, impending y) => y;

    public static impending operator | (impending x, impending y) => y;

    public static bool operator true  (impending s) => s.ω == 0;

    public static bool operator false (impending s)
    => throw new InvalidOperationException("impending is always 'false'");

    #if AL_OPTIMIZE  // -------------------------------------------------------

    public status undue => new status(ω);

    public static pending operator ! (impending s) => new pending(-s.ω);

    public static impending cont(ValidString reason = null) => _cont;

    public static impending doom(ValidString reason = null) => _doom;

    #endif
    #if !AL_STRICT  // --------------------------------------------------------

    public static implicit operator status(impending self) => self.undue;

    #endif

}}
