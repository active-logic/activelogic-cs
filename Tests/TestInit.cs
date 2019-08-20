using System;
using NUnit.Framework;
using Active.Core;
using NullRef = System.NullReferenceException;

using static Active.Core.status;

public class TestInit : DecoratorTest<Init> {

    override public void Setup(){
        base.Setup();
        // Need to reset this because testing purposely breaks the decorator
        Init.current = null;
    }

    [Test] public void TestInitFailingViaNull(){
        string str = "peek";
        status s = x?[ str = null ]?[ Substr(ref str, 1)];
        o( str, null);
    }

    [Test] public void TestIgnoreFailingInitViaNull(){
        string str = "peek";
        Assert.Throws<NullRef>(
            () => { status s = x?[ str = null ] + Substr(ref str, 1); }
        );
    }

    [Test] public void TestFirstPass(){
        string str = "";
        status s = x?[ str = "boo" ] + Substr(ref str, 1);
        o( str, "oo");
    }

    [Test] public void TestSecondPass(){
        string str = "peek";
        status s = x?[ str = "boo" ] + Substr(ref str, 1);
        o( s.complete );
        o( str, "oo");
        o( x.passing, true);
        s = x?[ str = "boo" ] + Substr(ref str, 1);
        o( s.complete );
        o( str, "oo" );
    }

    [Test] public void TestSecondPassWithPlusOp(){
        x.passing = false;
        status s;
        s = x?[ noop ] + fail(); o( !x.passing );
        s = x?[ noop ] + done(); o(  x.passing );
        s = x?[ noop ] + cont(); o( !x.passing );
    }

    [Test] public void TestSecondPassWithMinusOp(){
        x.passing = false;
        status s;
        s = x?[ noop ] - fail(); o(  x.passing );
        s = x?[ noop ] - done(); o( !x.passing );
        s = x?[ noop ] - cont(); o( !x.passing );
    }

    [Test] public void TestSecondPassWithNeutralOp(){
        x.passing = false;
        status s;
        s = x?[ noop ] % fail(); o(  x.passing );
        s = x?[ noop ] % done(); o(  x.passing );
        s = x?[ noop ] % cont(); o( !x.passing );
    }

    [Test] public void TestDetectUnclosedInitExp(){
        string str = "";
        Init.Gate? gate = x?[ str = "boo" ];
        Assert.Throws<InvalidOperationException>( () => {
            var s = x?[ str = "boo" ] + Substr(ref str, 1);
        });
    }

    [Test] public void StatusExp_InitFailing([Range(-1, +1)] int val)
    { o( (status)x?[ fail() ]?[ ForInt(val) ], status._fail ); }

    [Test] public void StatusExp_InitSucceeding([Range(-1, +1)] int val)
    { o( (status)x?[ done() ]?[ ForInt(val) ], ForInt(val) ); }

    [Test] public void StatusExp_InitRunning([Range(-1, +1)] int val)
    { o( (status)x?[ cont() ]?[ ForInt(val) ], status._cont ); }

    object noop => 0;

    status ForInt(int val) => status.@unchecked(val);

    status Substr(ref string s, int i){
        s = s.Substring(i);
        return status.done();
    }

}
