// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct failure{

    internal static readonly failure _flop = new failure();

    public static failure operator % (failure x, failure y) => _flop;

    public static loop operator + (failure x) => loop._forever;

  #if AL_OPTIMIZE
    public status fail => status._fail;

    public static action  operator ! (failure s) => action._void;
  #endif

  #if !AL_STRICT
    public static implicit operator status(failure self) => self.fail;
  #endif

}}
