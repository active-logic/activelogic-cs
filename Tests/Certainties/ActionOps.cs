using System; using InvOp = System.InvalidOperationException;
using NUnit.Framework; using static Active.Raw; using Active.Core;

namespace Unit.Certainties{ public class ActionOps : OpsTest{

    // Dynamically, bool conversion precedes truehood check so
    // this test will fail. Still works statically (CS217)
    // [Test] public void DisallowOR()
    // => OR__CS217_InvOp(@void, @false, forever, true, false,
    //        impending_fail, impending_cont, pending_done,
    //        pending_cont, fail, cont, done);

    [Test] public void Action_AND_Status([Range(-1, 1)] int y)
    => o( @void && s(y), done && s(y));

    [Test] public void Action_AND_Action()
    { action w = @void && @void; }

    // Side effect of implicit boolean conversion, so although the
    // types are disjoint, still works.
    [Test] public void Action_AND_Failure()
    => o(@void && @false, false);

    [Test] public void Action_AND_Pending([Range(0, 1)] int y)
    => o( @void && p(y), p(y) );

    // Disjoint from loop and impending
    [Test] public void _Action_AND_Disjoint()
    => AND_CS19_Disjoint(@void, forever, impending_cont,
                                         impending_fail);

    [Test] public void nsAction_or_Status([Range(-1, 1)] int y)
    => Assert.Throws<InvOp>( () => z = @void | s(y) );

}}
