using NUnit.Framework;
using Active.Core;


public class TestSelector : CoreTest {

    Selector x;

    [SetUp] public void Setup() => x = new Selector();

    [Test] public void end() => o ( x.end.failing );

    // NOTE: ideally should also verify that the iterator was reset.
    [Test] public void iterator(){
        var i = x.iterator;
        o( i!= null );
        o( i, x.iterator );

    }

    [Test] public void indexer(){
        o ( x[fail].running  );
        o ( x[cont].running  );
        o ( x[done].complete );
    }

}
