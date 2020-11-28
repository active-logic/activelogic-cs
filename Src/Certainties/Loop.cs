// Doc/Reference/Certainties.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using InvOp = System.InvalidOperationException;
using Active.Core.Details;

namespace Active.Core{
public readonly partial struct loop{

    internal static readonly loop _cont = new loop();

    #if AL_OPTIMIZE

    public status ever => status._cont;

    public static loop cont(ValidString reason = null) => _cont;

    #endif

    public static loop operator % (loop x, loop y) => _cont;

    public static loop operator & (loop x, status y)
    => throw new InvOp($"({x} & {y}) is not allowed");

    public static loop operator | (loop x, status y)
    => throw new InvOp($"({x} | {y}) is not allowed");

    public static bool operator true  (loop s)
    => throw new InvOp("truehood cannot be tested (loop)");

    public static bool operator false (loop s)
    => throw new InvOp("falsehood cannot be tested (loop)");

    public override bool Equals(object x) => x is loop;

    override public int GetHashCode() => 0;

    public static implicit operator impending(loop self)
    => impending._cont;

    public static implicit operator pending(loop self)
    => pending._cont;

    public static implicit operator status(loop self)
    => status._cont;

}}
