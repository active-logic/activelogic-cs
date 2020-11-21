// Doc/Reference/Reset-Management.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestResetCriterion : TestBase{

    ResetCriterion x;
    ReCon stack;

    [SetUp] public void Setup(){
        x = new ResetCriterion();
        stack = new ReCon();
    }

    [Test] public void Id(){ int z = ResetCriterion.id; }

    #if !AL_OPTIMIZE
    [Test] public void Check_3arg(){
        x.Check(null, stack, ("path", "method", 0));
        o(x.context, null);
        o(x.hold, null);
    }
    #endif

    [Test] public void Check_bypass(){
        x.Check(null, stack);
        o(x.context, null);
        o(x.hold, null);
    }

    [Test] public void Check_trigger(){
        x.Check("something", stack);
        o(x.context != null);
        o(x.hold, "something");
    }

    [Test] public void Check_nullable(){
        int? a = 1;
        int  b = 1;
        x.Check(a, stack);
        o(x.context != null);
        o(x.hold, 1);
        x.context = null;
        //
        x.Check(b, stack);
        o(x.context == null);
        o(x.hold, 1);
    }

    [Test] public void Indexer([Range(-1, 1)]        int val,
                               [Values(null, "foo")] string crit){
        var dec = new Dec();
        x.Check(crit, stack);                 // trigger x (enter cx)
        x.context?.Traverse( dec );           // traverse a decorator
        var s0 = status.@unchecked(val);
        var s1 = x[ s0 ];                     // exit the context
        o( dec.didReset, crit == "foo" );     // did reset
        o( s0, s1 );                          // neutrality
    }

    [Test] public void Reset(){
        x.hold = "foo";
        x.Reset();
        o(x.hold, null);
    }

}
