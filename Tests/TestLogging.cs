#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

#if !AL_OPTIMIZE

public class TestLogging : CoreTest {

    bool _log;
    [SetUp]    public void SaveLoggingState()    => _log = status.log;
    [TearDown] public void RestoreLoggingState() => status.log = _log;

    [Test] public void Status([Values(true, false)] bool lg){
        status.log = lg;
        o( done, Logging.Status(done, null, "path", "m", -1) );
    }

    [Test] public void Pending([Values(true, false)] bool lg){
        status.log = lg;
        o( pending._cont,
           Logging.Pending(pending.cont(), null, "path", "m", -1) );
    }

    [Test] public void Impending([Values(true, false)] bool lg){
        status.log = lg;
        o( impending._cont,
           Logging.Impending(impending.cont(), null, "path", "m", -1) );
    }

}
#endif
