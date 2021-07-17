#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using ArgEx = System.ArgumentException;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using LogData = Active.Core.AbstractDecorator.LogData;

public class TestStatusRef : TestBase{

    Dec dec;
    StatusRef x;

    [SetUp] public void Setup(){
        x = new StatusRef(status.done(),
                          new LogData(dec = new Dec(), null, null));
    }

    [Test] public void ToStatus(){
        o((status)x, status.done());
    }

    [Test] public void ToStatusNullRef([Range(-1, 1)] int w){
        #if !AL_OPTIMIZE
        StatusRef.SetLogData(dec, null, null);
        #endif
        StatusRef.hold = status.@unchecked(w);
        StatusRef? z = null;
        o((status)z, StatusRef.hold);
    }

    // --------------------------------------------------------------

    #if !AL_OPTIMIZE

    [Test] public void SetLogData_clearFirst(){
        StatusRef.SetLogData(dec, null, null);
        Assert.Throws<InvOp>(
                        () => StatusRef.SetLogData(dec, null, null) );
    }

    [Test] public void ToStatusWithLog([Values(true, false)] bool lg){
        status.log = lg;
        StatusRef.SetLogData(dec, null, null);
        StatusRef.ToStatusWithLog(x);
    }

    [Test] public void ToStatusWithLog_badScope([Values(true, false)] bool lg){
        status.log = lg;
        StatusRef.SetLogData(null, null, null);
        if(status.log)
            Assert.Throws<InvOp>(
                () => StatusRef.ToStatusWithLog(null) );
    }

    [Test] public void ToStatusWithLog_nullRef([Values(true, false)]
                                                            bool lg){
        status.log = lg;
        StatusRef.SetLogData(dec, null, null);
        StatusRef.ToStatusWithLog(null);
    }

    #endif

    // ==============================================================

    public class Dec : AbstractDecorator{
        override public action Reset() => action.done();
    }

}
