#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using NUnit.Framework; using Active.Core; using static Active.Raw;
//using Ex = System.Exception;

public class TestStaticAPI : TestBase{

    protected List<object> statuses;

    [SetUp] public void EvalStatuses(){
        statuses = new List<object>();
        foreach(var s in status.values) statuses.Add(s);
        foreach(var s in pending.values) statuses.Add(s);
        foreach(var s in impending.values) statuses.Add(s);
        foreach(var s in loop.values) statuses.Add(s);
        foreach(var s in failure.values) statuses.Add(s);
        foreach(var s in action.values) statuses.Add(s);
    }

}
