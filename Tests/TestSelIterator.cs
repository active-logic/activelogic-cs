using NUnit.Framework;
using Active.Core;
using Active.Core.Details;


public class TestSelIterator : CoreTest {

    Selector    s;
    SelIterator i;

    [SetUp] public void Setup(){
        s = new Selector();
        i = new SelIterator(s);
    }

    [Test] public void end(){
        status k = i.end;
        o( k.failing );
        o( k.raw +1, -1 );
    }

    [Test] public void repeat()
    { o ( (+i.repeat).failing ); o ( s.index, -1 ); }

    [Test] public void loop()
    { o ( (+i.loop).running ); o ( s.index, -1 ); }

    [Test] public void indexer(){
                               o( s.index, 0 );
        o( i[fail].running  ); o( s.index, 1 );
        o( i[cont].running  ); o( s.index, 1 );
        o( i[done].complete ); o( s.index, 1 );
    }

    [Test] public void Reset() => o( i.Reset(), i );

}
