using NUnit.Framework;
using Active.Core;

public class TestTimeout2 : DecoratorTest<Timeout> {

    bool _log;

    [SetUp]    public void SaveLoggingState()    => _log = status.log;
    [TearDown] public void RestoreLoggingState() => status.log = _log;

    [Test] public void OnStatus([Values(true, false)] bool running){
        x.OnStatus(running);
        if(running){
            o(x.stamp != -1);
        }else{
            o(x.stamp, -1);
        }

    }

    [Test] public void OnStatus_timeoutDisabled(
                                 [Values(true, false)] bool running){
        x.stamp = -1;
        x.OnStatus(running);
        if(running){
            o( System.Math.Abs(x.stamp-x.time) < 1e-4 );
        }else{
            o(x.stamp, -1);
        }
    }

    [Test] public void Indexer_timeoutEnabled_pass(
                                  [Values(true, false)] bool logging){
        status.log = logging;
        o( x.enabled );
        o( x[1f] != null );
    }

    [Test] public void Indexer_timeoutEnabled_didTimeout(
                                  [Values(true, false)] bool logging){
        status.log = logging;
        x.stamp = x.time - 2f;
        o( x.enabled );
        o( x[1f] == null );
    }

    [Test] public void Indexer_timeoutDisabled(
                                  [Values(true, false)] bool logging){
        status.log = logging;
        x.stamp = -1;
        o( x[1f] != null );
        o( x[0f] != null );
    }

}
