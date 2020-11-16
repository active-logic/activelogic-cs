using NUnit.Framework;

using Active.Core;
using InvOp = System.InvalidOperationException;

public class TestCertainties{

public class TestAction : CoreTest{

    [Test] public void Action()
    => o ( (status)action._done, status._done );

    [Test] public void InvertAction()
    => o( !action._done, failure._fail );

    [Test] public void CombineActions()
    { action x = action._done % action._done; }

    [Test] public void Action_and_Action()
    => o(action._done && action._done, action._done);

    [Test] public void Action_and_Status()
    => o(action._done & status._done, status._done);

    [Test] public void Status_and_Action()
    => o(status._done && action._done, status._done);

    [Test] public void Action_and_Failure()
    => o(action._done & failure._fail, false);

    [Test] public void ToStatus(){
        status s = action._done;
        o( s, status._done );
    }

}

public class TestFailure : CoreTest{

    [Test] public void Failure_internal()
    => o ( (status)failure._fail, status._fail  );

    [Test] public void Failure()
    => o ( (status)failure._fail, status._fail  );

    [Test] public void InvertFailure()
    => o( !failure._fail, action._done );

    [Test] public void CombineFailures()
    { failure x = failure._fail % failure._fail; }

    [Test] public void OrFailures()
    => o(failure._fail || failure._fail, failure._fail);

}

public class TestLoop : CoreTest{

    [Test] public void CombineLoops()
    { loop x = loop._cont % loop._cont; }

}

public class TestPending : CoreTest{

    [Test] public void PendingDone()
    => o ( pending.done().due.complete);

    [Test] public void PendingCont()
    => o ( pending.cont().due.running);

    [Test] public void Pending_Done()
    => o ( pending._done.due.complete);

    [Test] public void Pending_Cont()
    => o ( pending._cont.due.running);

    [Test] public void InvPending_Done()
    => o (!pending._done, impending._fail);

    [Test] public void InvPending_Cont()
    => o (!pending._cont, impending._cont);

    [Test] public void AndPending(){
        o(pending._cont && pending._cont, pending._cont);
        o(pending._cont && pending._done, pending._cont);
        o(pending._done && pending._cont, pending._cont);
        o(pending._done && pending._done, pending._done);
    }

}

public class TestImpending : CoreTest{

    [Test] public void ImpendingFail()
    => o( impending.fail().undue.failing );

    [Test] public void ImpendingCont()
    => o( impending.cont().undue.running );

    [Test] public void Impending_Fail()
    => o( impending._fail.undue.failing );

    [Test] public void Impending_Cont()
    => o( impending._cont.undue.running );

    [Test] public void InvImpendingFail()
    => o(!impending._fail, pending._done);

    [Test] public void InvImpendingCont()
    => o(!impending._cont, pending._cont);

    [Test] public void OrImpending(){
        o(impending._cont || impending._cont, impending._cont);
        o(impending._cont || impending._fail, impending._cont);
        o(impending._fail || impending._cont, impending._cont);
        o(impending._fail || impending._fail, impending._fail);
    }

}

} // TestCertainties
