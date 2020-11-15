// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct loop{

    internal static readonly loop _cont    = new loop();

    #if AL_OPTIMIZE

    public status ever => status._cont;

    public static loop cont(ValidString reason = null) => _cont;

    #endif

    public static loop operator % (loop x, loop y) => _cont;

    public static implicit operator impending(loop self)
    => impending._cont;

    public static implicit operator pending(loop self)
    => pending._cont;

    // public static implicit operator status(loop self)
    // => status._cont;

}}
