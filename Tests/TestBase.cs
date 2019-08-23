using NUnit.Framework;
using Active.Core.Details;

public class TestBase {

    // Set to 1e3 for running benches
    protected const int BaseIters = (int)1;

    protected const int XtrmTestIters = BaseIters * (int)1e5;
    protected const int HighTestIters = BaseIters * (int)1e4;
    protected const int PerfTestIters = BaseIters * (int)1e3;
    protected const int NormTestIters = BaseIters * (int)1e2;
    protected const int SlowTestIters = BaseIters * (int)1e1;

    public TestBase() => StatusFormat.UseASCII();

    protected void o (bool arg) { Assert.That(arg); }
    protected void o (object x, object y) { Assert.That(x, Is.EqualTo(y)); }
    #if UNITY_EDITOR
    protected void print(string msg){
        UnityEngine.Debug.Log(msg);
    }
    #endif

}
