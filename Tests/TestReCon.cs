using System;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestReCon : TestBase{

    ReCon rec;

    [SetUp] public void Setup() => rec = new ReCon();

    [Test] public void Enter(){
        var z = rec.Enter(forward: false);
        o( rec.Count, 1 );
        o( z is ReCon.Context );
    }

    [Test] public void Add(){
        rec.Enter(forward: false);
        var dec = new Dec();
        rec.Add(dec);
        o( rec.Peek()[0], dec);
    }

    [Test] public void Add_no_context(){
        rec.Add(new Dec());
    }

    [Test] public void NewContext(){
        var cx = new ReCon.Context(rec, forward: false);
    }

    [Test] public void ContextExit(){
        var cx = rec.Enter(forward: false);
        cx.Exit();
    }

    [Test] public void ContextExit_stackError(){
        var a = rec.Enter(forward: false);
        var b = rec.Enter(forward: false);
        Assert.Throws<InvOp>( () => a.Exit() );
    }

    [Test] public void NewCx_noRoE(){
        ReCon.Context cx;
        Assert.Throws<NullReferenceException>(
                  () => cx = new ReCon.Context(null, forward: false));
    }

    [Test] public void CxIndexer_noDecs([Range(-1, 1)] int val){
        var cx = rec.Enter(forward: false);
        status s = status.@unchecked(val);
        s = cx[s];
        o(rec.Count, 0);
    }

    [Test] public void CxIndexer_didReset([Range(-1, 1)] int val){
        var cx = rec.Enter(forward: false);
        var dec = new Dec();
        rec.Add( dec );
        status s = status.@unchecked(val);
        s = cx[s];
        o(dec.didReset, !s.running);
        o(rec.Count, 0);
    }

    [Test] public void CxIndexer_stackEmpty([Range(-1, 1)] int val){
        var cx = new ReCon.Context(rec, forward: false);
        status s = status.@unchecked(val);
        Assert.Throws<InvOp>(
            () => s = cx[s] );
    }

}
