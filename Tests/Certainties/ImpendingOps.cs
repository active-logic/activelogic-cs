using System; using InvOp = System.InvalidOperationException;
using NUnit.Framework; using static Active.Raw; using Active.Core;

namespace Unit.Certainties{ public class ImpendingOps : OpsTest{

    [Test] public void DisallowAND([Range(-1, 0)] int x)
    => AND_CS217_InvOp(i(x), @void, @false, forever, true, false,
            impending_fail, impending_cont, pending_done,
            pending_cont, fail, cont, done);

    [Test] public void Impending_OR_Status([Range(-1, 0)] int x,
                                           [Range(-1, 1)] int y)
    => o(i(x) || s(y), s(x) || s(y));

    [Test] public void Impending_OR_Failure([Range(-1, 0)] int x)
    => o( i(x) || @false, i(x) );

    [Test] public void Impending_OR_Loop([Range(-1, 0)] int x)
    => o(i(x) || forever, impending_cont);

    [Test] public void Impending_OR_Impending([Range(-1, 0)] int x,
                                              [Range(-1, 0)] int y)
    { impending w = i(x) || i(y); }

    // Should be allowed, however no type conversions.
    [Test] public void _Impending_OR_Disjoint([Values(0, -1)] int x)
    => OR__CS19_Disjoint(i(x), @void, true, false, pending_cont,
                                pending_done);

    [Test] public void nsImpending_and_Status([Range(-1, 0)] int x,
                                          [Range(-1, 1)] int y)
    => Assert.Throws<InvOp>( () => z = i(x) & s(y) );

}}
