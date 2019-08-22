// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;
using System;

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

    public static pending operator & (pending x, pending y) => y;

    public static pending operator | (pending x, pending y) => y;

    public static bool operator true  (pending s)
    => throw new InvalidOperationException("pending is always 'true'");

    public static bool operator false (pending s) => s.ω == 0;

    #if AL_OPTIMIZE   // ------------------------------------------------------

    public status due => new status(ω);

    public static impending operator !(pending s) => new impending(-s.ω);

    public static pending cont(ValidString reason = null) => _cont;

    public static pending done(ValidString reason = null) => _done;

    #endif
    #if !AL_STRICT  // --------------------------------------------------------

    public static implicit operator status(pending self) => self.due;

    #endif

}}
