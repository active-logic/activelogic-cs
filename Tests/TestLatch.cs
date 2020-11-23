using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestLatch : DecoratorTest<Latch> {

    [Test] public void Reset(){
        x.passing = true;
        x.Reset();
        o(x.passing, false);
    }

    [Test] public void OnStatus_cont(){
        x.OnStatus(cont);
        o(x.passing, x.passing);
    }

    [Test] public void OnStatus_done_or_fail(){
        x.OnStatus(done);
        o(x.passing, false);
        x.OnStatus(fail);
        o(x.passing, false);
    }

    // =============================================================

    [Test] public void BasicTest(){
        o (x.passing, false);
        var s = x[true];
        o (s != null, true);
        o (x.passing, true);
    }

    [Test] public void Initial(){
        o ( new Latch()[true ] != null);
        o ( new Latch()[false] == null );
    }

    [Test] public void CycleResetsOnTargetdone(){
        StatusRef.checkLogData = false;
        o (x[false] == null);           // fail until...
        o (x[true] != null);            // ...the condition passes,
        o (x[false] != null);           // then Pass regardless of condition
        var s = x[false]?[done.Via()];  // until done
        o (x[false] == null);
	}

    [Test] public void CycleResetsOnTargetFails(){
        o (x[true] != null);            // Trigger the latch
        var s = x[false]?[done.Via()];  // Latch passes until the target fails
        o (x[false] == null);           // Latch did reset
    }

}
