using NUnit.Framework;
using Active.Core;
using Active.Core.Details;


public class TestSeqIterator : CoreTest {

    Sequence    s;
    SeqIterator i;

    [SetUp] public void Setup(){
        s = new Sequence();
        i = new SeqIterator(s);
    }

    [Test] public void end(){
        status k = i.end;
        o( k.raw - 1, 1 );
    }

    [Test] public void repeat()
    { o ( (-i.repeat).complete ); o ( s.index, -1 ); }

    [Test] public void loop()
    { o ( (-i.loop).running ); o ( s.index, -1 ); }

    [Test] public void indexer(){
                               o( s.index, 0 );
        o( i[fail].failing  ); o( s.index, 0 );
        o( i[cont].running  ); o( s.index, 0 );
        o( i[done].running  ); o( s.index, 1 );
    }

    [Test] public void Reset() => o( i.Reset(), i );

}
