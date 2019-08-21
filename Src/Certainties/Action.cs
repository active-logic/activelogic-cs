// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public readonly partial struct action{

    internal static readonly action _void = new action();

    public static action operator % (action x, action y) => _void;

  #if AL_OPTIMIZE
    public status now => status._done;

    public static failure operator ! (action s) => failure._flop;
  #endif

  #if !AL_STRICT
    public static implicit operator status(action self) => self.now;
  #endif

}}
