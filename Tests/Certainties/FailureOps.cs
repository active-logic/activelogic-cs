using System; using InvOp = System.InvalidOperationException;
using NUnit.Framework; using static Active.Raw; using Active.Core;

namespace Unit.Certainties{ public class FailureOps : OpsTest{

    // Dynamically, bool conversion precedes falsehood check so
    // this test will fail. Still works statically (CS217)
    // [Test] public void DisallowAND()
    // => AND_CS217_InvOp(@false, @void, @false, forever, true, false,
    //        impending_fail, impending_cont, pending_done,
    //        pending_cont, fail, cont, done);

    [Test] public void Failure_OR_Status([Range(-1, 1)] int y)
    => o( @false || s(y), fail || s(y));

    // Side effect of implicit boolean conversion, so although the
    // types are disjoint, still works.
    [Test] public void Failure_OR_Action()
    => o(@false || @void, true);

    [Test] public void Failure_OR_Failure()
    { failure w = @false || @false; }

    [Test] public void Failure_OR_Impending([Range(-1, 0)] int y)
    => o( @false || i(y), i(y) );

    // Disjoint from loop and pending
    [Test] public void _Failure_OR_Disjoint()
    => OR__CS19_Disjoint(@false, forever, pending_cont, pending_done);

    [Test] public void nsFailure_and_Status([Range(-1, 1)] int y)
    => Assert.Throws<InvOp>( () => z = @false & s(y) );

}}
