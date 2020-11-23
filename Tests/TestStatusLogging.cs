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
    bool _log;

    [SetUp] public void Setup()
    { _log = status.log; StatusFormat.UseASCII(); }

    [TearDown] public void RestoreLoggingState() => status.log = _log;

    [Test] public void Indexer1([Values(true, false)] bool lg){
        status.log = lg;
        var s = (done() && cont())[log && "Floppy"];
        o(StatusFormat.Status(s), lg ? $"+ {T}.Indexer1 (Floppy)"
                                     : "+ ?trace");
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
