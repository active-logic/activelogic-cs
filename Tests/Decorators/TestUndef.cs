#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestUndef : DecoratorTest<Undef> {

    bool _log;
    [SetUp]    public void SaveLoggingState()    => _log = status.log;
    [TearDown] public void RestoreLoggingState() => status.log = _log;

    [Test] public void Indexer([Values(true, false)] bool lg){
        status.log = lg;
        var s0 = x[1f];
        o( s0.running );
        var s1 = x[0f];
    }

    [Test] public void random(){
        var r0 = x.random;
        var r1 = x.random;
        o( r0 != null );
        o( r0, r1 );
    }

    [Test] public void Format()
    => o( StatusFormat.Status(x[1f]), "+ <U> undef" );

    // TODO - remove
    [Test] public void Random_old(){
        var z = new Undef();
        status x = z[1f];
    }

    [Test] public void Reset(){
        var z = new Undef();
        z.stamp = 1000;
        z.Reset();
        o(z.stamp != 1000);
    }

}

#endif
