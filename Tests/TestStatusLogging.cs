// Doc/Reference/Steppers.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Status;

public class TestStatusLogging : CoreTest {

    const string T = "TestStatusLogging";
    protected static readonly LogString log = null;

    [SetUp] public void Setup() => StatusFormat.UseASCII();

    [Test] public void Indexer1(){
        var s = (done() && cont())[log && "Floppy"];
        o(StatusFormat.Status(s), $"+ {T}.Indexer1 (Floppy)");
    }

    [Test] public void Indexer2(){
        var s = Nested()[log && "Floppy"];
        o(StatusFormat.Status(s), $"+ {T}.Nested (Floppy)");
    }

    [Test] public void RedundantTrace(){
        var s = Eval(done());
        o( StatusFormat.Status(s).Substring(2), $"{T}.RedundantTrace" );
    }

    // ------------------------------------------------------------------------

    status Nested() => done() && cont();

}

#endif
