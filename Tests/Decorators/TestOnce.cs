#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using static Active.Raw;

public class TestOnce : DecoratorTest<Once> {

    [Test] public void Cycle(){
        o( x.pass.HasValue ); // Works the first time
        x.OnStatus(done.Via());
        o( x.pass, null);     // Not the second
        o( x.pass, null);     // or third
        x.Reset();
        o( x.pass.HasValue ); // Ready for another round
    }

    [Test] public void Id() => o( Once.id > 0 );

    [Test] public void Pass_done()
    { x.state = done; o(x.pass, null); }

    [Test] public void Pass_fail()
    { x.state = fail; o(x.pass, null); }

    [Test] public void Pass_cont()
    { x.state = cont; o(x.pass != null); }

    [Test] public void GateIndexer([Values(true, false)] bool lg){
        status.log = lg;
        var gate = new Once.Gate(x, new Once.LogData(x, null, null));
        var rf = gate[done];
        o((status)rf, done);
    }

    [Test] public void NullStatusRefToStatus(){
        var s = Once.StatusRef.ToStatus(null);
    }

    #if !AL_OPTIMIZE
    [Test] public void NullStatusRefToStatusWithLog(){
        var s = Once.StatusRef.ToStatusWithLog(null);
    }
    #endif

    //[Test] public void Once_StatusRef(){
    //    var rf = new StatusRef(done, new Once.LogData(x, null, null));
    //
    //}

}
