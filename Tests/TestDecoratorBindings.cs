#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_BEST_PERF

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Raw;

#pragma warning disable 0618

public class TestDecoratorBindings : TestBase{

    Task x; [SetUp] public void Setup() => x = new Task();

    [Test] public void TestBindings(){
        object o;
        //o = x.After(1f);
        o = x.Cooldown(1f);
        o = x.InOut(true, false);
        o = x.Every(1f);
        o = x.Once();
        o = x.Timeout(1f);
        o = x.Wait(1f);
        o = x.While(cont);
        o = x.While(true);
        o = x.with(null);
        o = x.Tie(cont);
        o = x.Tie(true);
        o = x.Sequence();
        o = x.Selector();
        #if !AL_THREAD_SAFE
        o = x.Seq();
        o = x.Sel();
        o = x.reckon(true);
        o = x.with(null);
        #endif  // end !AL_THREAD_SAFE
    }

    class Task : Active.Core.Task{}

}

#endif  // !AL_BEST_PERF
