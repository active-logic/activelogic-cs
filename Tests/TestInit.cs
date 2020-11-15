using System;
using NUnit.Framework;
using Active.Core;

using static Active.Status;

public class TestInit : CoreTest {

    Init init;

    [SetUp] public void Setup(){
        init = new Init();
    }

    [Test] public void TestFirstPass(){
        string str = "";
        status x = init.pass?[ str = "boo" ] + Substr(ref str, 1);
        o( str, "oo");
    }

    [Test] public void TestSecondPass(){
        string str = "peek";
        init.passing = false;
        status x = init.pass?[ str = "boo" ] + Substr(ref str, 1);
        o( x.complete );
        o( str, "eek");
        o( init.passing, true);
        x = init.pass?[ str = "boo" ] + Substr(ref str, 1);
        o( x.complete );
        o( str, "oo" );
    }

    [Test] public void TestSecondPassWithPlusOp(){
        init.passing = false;
        status x;
        x = init.pass?[ noop ] + fail(); o( !init.passing );
        x = init.pass?[ noop ] + done(); o(  init.passing );
        x = init.pass?[ noop ] + cont(); o( !init.passing );
    }

    [Test] public void TestSecondPassWithMinusOp(){
        init.passing = false;
        status x;
        x = init.pass?[ noop ] - fail(); o(  init.passing );
        x = init.pass?[ noop ] - done(); o( !init.passing );
        x = init.pass?[ noop ] - cont(); o( !init.passing );
    }

    [Test] public void TestSecondPassWithNeutralOp(){
        init.passing = false;
        status x;
        x = init.pass?[ noop ] % fail(); o(  init.passing );
        x = init.pass?[ noop ] % done(); o(  init.passing );
        x = init.pass?[ noop ] % cont(); o( !init.passing );
    }

    [Test] public void TestNestedInit(){
        Init x = new Init(), y = new Init();
        int a = 0, b = 0;
        status s = x.pass?[ a = (a == 0 ? 1 : 2) ] % (
            y.pass?[ b = (b == 0 ? 1 : 2) ] % cont()
        );
    }

    object noop => 0;

    status Substr(ref string s, int i){
        s = s.Substring(i);
        //print();
        return status.done();
    }

}
