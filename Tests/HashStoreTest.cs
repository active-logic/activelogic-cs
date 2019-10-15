using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestHashStore : TestBase{

    Store x;

    [SetUp] public void Setup(){
        x = new HashStore();
    }

    [Test] public void Composite(){
        var c0 = x.Composite<Sequence>(0);
        var c1 = x.Composite<Sequence>(1);
        var c2 = x.Composite<Sequence>(0);
        o( c0 != c1 );
        o( c0, c2);

    }

    [Test] public void Decorator(){
        var c0 = x.Decorator<Latch>(0, Latch.id);
        var c1 = x.Decorator<Latch>(1, Latch.id);
        var c2 = x.Decorator<Latch>(0, Latch.id);
        o( c0 != c1 );
        o( c0, c2);
    }

    [Test] public void Reset(){
        var c0 = x.Composite<Sequence>(0);
        x.Reset();
        var c1 = x.Composite<Sequence>(0);
        o(c0 != c1);
    }

}
