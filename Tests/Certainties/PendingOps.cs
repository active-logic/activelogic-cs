using System; using InvOp = System.InvalidOperationException;
using NUnit.Framework; using static Active.Raw; using Active.Core;

namespace Unit.Certainties{ public class PendingOps : OpsTest{

    [Test] public void DisallowOR([Range(0, 1)] int x)
    => OR__CS217_InvOp(p(x), @void, @false, forever, true, false,
            impending_fail, impending_cont, pending_done,
            pending_cont, fail, cont, done);

    [Test] public void Pending_AND_Status([Range( 0, 1)] int x,
                                          [Range(-1, 1)] int y)
    => o(p(x) && s(y), s(x) && s(y));

    [Test] public void Pending_AND_Action([Range(0, 1)] int x)
    => o( p(x) && @void, p(x) );

    [Test] public void Pending_AND_Loop([Range(0, 1)] int x)
    => o(p(x) && forever, pending_cont);

    [Test] public void Pending_AND_Pending([Range(0, 1)] int x,
                                           [Range(0, 1)] int y)
    { pending w = p(x) && p(y); }

    // Should be allowed, however no type conversions.
    [Test] public void _Pending_AND_Disjoint([Values(0, 1)] int x)
    => AND_CS19_Disjoint(p(x), @false, true, false, impending_cont,
                               impending_fail);

    [Test] public void nsPending_or_Status([Range( 0, 1)] int x,
                                         [Range(-1, 1)] int y)
    => Assert.Throws<InvOp>( () => z = p(x) | s(y) );

}}
