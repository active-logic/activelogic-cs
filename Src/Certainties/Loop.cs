// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct loop{

    internal static readonly loop _forever = new loop();

    #if AL_OPTIMIZE
    public status ever => status._cont;
    #endif

    public static loop operator % (loop x, loop y) => _forever;

    #if !AL_STRICT
    public static implicit operator status(loop self) => self.ever;
    #endif

}}
