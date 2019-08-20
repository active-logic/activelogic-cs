using NUnit.Framework;
using Active.Core;


public class TestSequence : CoreTest {

    Sequence x;

    [SetUp] public void Setup() => x = new Sequence();

    [Test] public void end() => o ( x.end.complete );

    // NOTE: ideally should also verify that the iterator was reset.
    [Test] public void iterator(){
        var i = x.iterator;
        o( i!= null );
        o( i, x.iterator );
    }

    [Test] public void indexer(){
        o ( x[fail].failing );
        o ( x[cont].running );
        o ( x[done].running );
    }

}
