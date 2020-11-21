#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core.Details;

public class TestBase {

    #if !AL_OPTIMIZE

    [SetUp] public void LogDataSetup()
    => StatusRef.checkLogData = true;

    [TearDown] public void LogDataTeardown()
    => StatusRef.ClearLogData();

    #endif

    // Set to 1e3 for running benches
    protected const int BaseIters = (int)1;

    protected const int XtrmTestIters = BaseIters * (int)1e5;
    protected const int HighTestIters = BaseIters * (int)1e4;
    protected const int PerfTestIters = BaseIters * (int)1e3;
    protected const int NormTestIters = BaseIters * (int)1e2;
    protected const int SlowTestIters = BaseIters * (int)1e1;

    public TestBase(){
        StatusFormat.UseASCII();
        // TODO - since all decorators require an RoR context,
        // unit tests break down when RoR is enabled. This should be
        // relatively safe. Alternatively consider managing the
        // context on setup/teardown
        RoR.enabled = false;
    }

    protected bool o (bool arg) { Assert.That(arg); return true; }

    protected bool o (object x, object y)
    { Assert.That(x, Is.EqualTo(y)); return true; }

    #if UNITY_EDITOR
    protected void print(string msg){
        UnityEngine.Debug.Log(msg);
    }
    #endif

}
