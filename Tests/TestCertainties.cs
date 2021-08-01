// Doc/Reference/Logging.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using InvOp    = System.InvalidOperationException;
using TargetEx = System.Reflection.TargetInvocationException;

public class TestCertainties{

public class TestAction : CoreTest{

    #if !AL_OPTIMIZE
    LogString log = null;
    [Test] public void Via()
    => o( action._done.Via(log && "Test"), action._done );
    #endif

    [Test] public void Action()
    => o ( (status)action._done, status._done );

    // Implicit conversion to bool prevails, does not invoke `true`
    [Test] public void @true_dyn(){
        var z = (dynamic)action._done || action._done;
        o(z is action);
    }

    [Test] public void @true_ref(){
        var ex = Assert.Throws<TargetEx>(
            () => typeof(action).GetMethod("op_True")
                  .Invoke( null, new object[]{action._done} )
        );
        o( ex.InnerException.GetType(), typeof(InvOp) );
    }

    [Test] public void @false(){
        var z = action._done && true;
    }

    [Test] public void Equals() => o(action._done, action._done);

    [Test] public void GetHashCode_()
    => o(action._done.GetHashCode(), 1);

    [Test] public void InvertAction()
    => o( !action._done, failure._fail );

    [Test] public void Demote()
    => o( -action._done, loop._cont );

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

    #if !AL_OPTIMIZE
    LogString log = null;
    [Test] public void Via()
    => o( failure._fail.Via(log && "Test"), failure._fail );
    #endif

    [Test] public void Failure()
    => o ( (status)failure._fail, status._fail  );

    [Test] public void Failure_internal()
    => o ( (status)failure._fail, status._fail  );

    [Test] public void @true(){
        var z = failure._fail || true;
        o(z, true);
    }

    // Implicit conversion to bool prevails, does not invoke `false`
    [Test] public void @false_dyn(){
        var z = (dynamic)failure._fail && true;
        o(z, failure._fail);
    }

    [Test] public void @false(){
        var ex = Assert.Throws<TargetEx>(
            () => typeof(failure).GetMethod("op_False")
                  .Invoke( null, new object[]{failure._fail} )
        );
        o( ex.InnerException.GetType(), typeof(InvOp) );
    }

    [Test] public void Equals() => o(failure._fail, failure._fail);

    [Test] public void GetHashCode_()
    => o(failure._fail.GetHashCode(), -1);

    [Test] public void InvertFailure()
    => o( !failure._fail, action._done );

    [Test] public void Promote()
    => o( +failure._fail, loop._cont );

    [Test] public void CombineFailures()
    { failure x = failure._fail % failure._fail; }

    [Test] public void OrFailures()
    => o(failure._fail || failure._fail, failure._fail);

}

public class TestLoop : CoreTest{

    #if !AL_OPTIMIZE
    LogString log = null;
    [Test] public void Via()
    => o( loop._cont.Via(log && "Test"), loop._cont );
    #endif

    [Test] public void GetHashCode_()
    => o(loop._cont.GetHashCode(), 0);

    [Test] public void Demote()  => o( -loop._cont, failure._fail );

    [Test] public void Promote() => o( +loop._cont, action ._done );

    [Test] public void CombineLoops()
    { loop x = loop._cont % loop._cont; }

    [Test] public void AND() => Assert.Throws<InvOp>( () =>
                             { var z = loop._cont & status._done; });

    [Test] public void OR() => Assert.Throws<InvOp>( () =>
                             { var z = loop._cont | status._done; });

    [Test] public void Equals() => o(loop._cont, loop._cont);

}

public class TestPending : CoreTest{

    #if !AL_OPTIMIZE
    LogString log = null;
    [Test] public void Via()
    => o( pending._done.Via(log && "Test"), pending._done );
    #endif

    [Test] public void GetHashCode_(){
        o(pending._cont.GetHashCode(), 0);
        o(pending._done.GetHashCode(), 1);
    }

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

    [Test] public void Equals(){
        o( new pending(0).Equals(pending._cont)  , true  );
        o( new pending(0).Equals(new object()   ), false );
    }

    [Test] public void ToString1()
    => o ( pending.cont().ToString(), "pending.cont");

    [Test] public void ToString2()
    => o ( pending.done().ToString(), "pending.done");

    [Test] public void AndPending(){
        o(pending._cont && pending._cont, pending._cont);
        o(pending._cont && pending._done, pending._cont);
        o(pending._done && pending._cont, pending._cont);
        o(pending._done && pending._done, pending._done);
    }

}

public class TestImpending : CoreTest{

    #if !AL_OPTIMIZE
    LogString log = null;
    [Test] public void Via()
    => o( impending._fail.Via(log && "Test"), impending._fail );
    #endif

    [Test] public void GetHashCode_(){
        o(impending._cont.GetHashCode(),  0);
        o(impending._fail.GetHashCode(), -1);
    }

    [Test] public void ImpendingFail()
    => o( impending.fail().undue.failing );

    [Test] public void ImpendingCont()
    => o( impending.cont().undue.running );

    [Test] public void Impending_Fail()
    => o( impending._fail.undue.failing );

    #pragma warning disable 618
    [Test] public void Impending_Doom()
    => o( impending.doom().undue.failing );
    #pragma warning restore 618

    [Test] public void Impending_Cont()
    => o( impending._cont.undue.running );

    [Test] public void InvImpendingFail()
    => o(!impending._fail, pending._done);

    [Test] public void InvImpendingCont()
    => o(!impending._cont, pending._cont);

    [Test] public void Equals(){
        o(impending._cont, impending._cont);
        o(impending._fail, impending._fail);
    }

    [Test] public void ToString1()
    => o ( impending.cont().ToString(), "impending.cont");

    [Test] public void ToString2()
    => o ( impending.fail().ToString(), "impending.fail");

    [Test] public void PendingDone()
    => o ( pending.done().due.complete);

    [Test] public void OrImpending(){
        o(impending._cont || impending._cont, impending._cont);
        o(impending._cont || impending._fail, impending._cont);
        o(impending._fail || impending._cont, impending._cont);
        o(impending._fail || impending._fail, impending._fail);
    }

    [Test] public void Equals_(){
        o( new impending(0).Equals( impending._cont), true  );
        o( new impending(0).Equals( new object()   ), false );
    }

}

} // TestCertainties
