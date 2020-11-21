using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using System.Collections.Generic;

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

    [Test] public void Seq(){
        var s0 = x.Seq(0); o( s0 != null );
        var s1 = x.Seq(0); o( s0, s1);
    }

    [Test] public void Sel(){
        var s0 = x.Sel(0); o( s0 != null );
        var s1 = x.Sel(0); o( s0, s1);
    }

    [Test] public void Decorator(){
        var c0 = x.Decorator<Latch>(0, Latch.id);
        var c1 = x.Decorator<Latch>(1, Latch.id);
        var c2 = x.Decorator<Latch>(0, Latch.id);
        o( c0 != c1 );
        o( c0, c2);
    }

    [Test] public void Reset(){
        var c0 = x.Decorator<Latch>(0, Latch.id);
        var z = x as HashStore;
        o( z.dmap.Count, 1);
        x.Reset();
        o( z.dmap.Count, 0);
        var c1 = x.Decorator<Latch>(0, Latch.id);
        o( c0 != c1);
    }

    [Test] public void Reset_noop(){
        x.Reset();
        var z = x as HashStore;
        o(z.cmap, null);
        o(z.dmap, null);
        o(z.seqmap, null);
        o(z.selmap, null);
    }

    [Test] public void Reset_clear(){
        var z = (HashStore)x;
        z.seqmap = new Dictionary<int, Seq>();
        z.selmap = new Dictionary<int, Sel>();
        z.cmap   = new Dictionary<int, Composite>();
        z.dmap   = new Dictionary<int, AbstractDecorator>();
        x.Reset();
        o(z.cmap   != null);
        o(z.dmap   != null);
        o(z.seqmap != null);
        o(z.selmap != null);
    }

}
