using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestComposite : CoreTest {

    Composite x;

    [SetUp] public void Setup() => x = new TComposite();

    [Test] public void TestIndex()
    { x.index = 5; o( x.index, 5 ); }

    [Test] public void TestIntConversion()
    { x.index = 5; o( x+1, 6 ); }

    [Test] public void TestReset()
    { x.index = 5; x.Reset(); o( x.index, 0 ); }

    [Test] public void TestEnd(){ status s = x.end; }

    [Test] public void TestLoop()
    { x.index = 5; status s = x.loop; o(x.index, 0); o( s.running ); }

    class TComposite : Composite{

        override public status end => done;

    }

}
