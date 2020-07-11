using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestRoR : TestBase{

    [SetUp] public void Setup(){
        RoR.enabled = true; RoR.owner = null; RoR.frame = 0;
    }

    // enabled mode ------------------------------------------------

    [Test] public void Enter(){
        RoR.Enter(this, 10);
        o(RoR.owner, this);
        o(RoR.frame, 10);
    }

    [Test] public void Enter_no_reassign(){
        RoR.owner = "Nemo";
        Assert.Throws<InvOp>( () => RoR.Enter("Foo", 0) );
    }

    [Test] public void Enter_reject_null_context()
    => Assert.Throws<InvOp>( () => RoR.Enter(null, 0) );

    [Test] public void Exit(){
        int φ = 10;
        RoR.Enter(this, φ);
        RoR.Exit(this, ref φ);
        o(RoR.owner, null);
        o(φ, 11);
    }

    [Test] public void Exit_match_context(){
        int φ = 10;
        RoR.Enter("Nemo", φ);
        Assert.Throws<InvOp>(() => RoR.Exit("Foo", ref φ));
    }

    [Test] public void Exit_match_frame(){
        int φ = 10;
        RoR.Enter("Nemo", 8);
        Assert.Throws<InvOp>(() => RoR.Exit("Nemo", ref φ));
    }

    [Test] public void OnResume_continue(){
        int φ = 10;
        int γ = 9;
        bool flag = true;
        RoR.Enter("Nemo", φ);
        RoR.OnResume(ref γ,
            () => { flag = false; return status.@void(); });
        o(flag, true);
    }

    [Test] public void OnResume_reset(){
        int φ = 15;
        int γ = 10;
        bool flag = true;
        RoR.Enter("Nemo", φ);
        RoR.OnResume(ref γ,
            () => { flag = false; return status.@void(); });
        o(flag, false);
    }

    [Test] public void OnResume_reject_null_context(){
        int γ = 10;
        Assert.Throws<InvOp>( () => RoR.OnResume(ref γ, null) );
    }

    // disabled mode -----------------------------------------------

    [Test] public void Enter_disabled(){
        RoR.enabled = false;
        RoR.Enter(this, 0);
        o(RoR.owner, null);
        o(RoR.frame, 0);
    }

    [Test] public void Exit_disabled(){
        int φ = 10;
        RoR.enabled = false;
        RoR.Exit(this, ref φ);
        o(RoR.owner, null);
        o(RoR.frame, 0);
    }

    [Test] public void OnResume_disabled(){
        RoR.enabled = false;
        int frame = 12;
        RoR.OnResume(ref frame, null);
        o(RoR.owner, null);
        o(RoR.frame, 0);
        o(frame, 12);
    }

}
