using NUnit.Framework;
using Active.Core;

 #if !AL_BEST_PERF

public class TestTask2 : CoreTest{

    Task x;

    [SetUp] public void Setup()   => x = new C();

    [Test] public void Rox([Values(true, false)] bool staticRecon){
        #if !AL_THREAD_SAFE
        var _static = x.staticRecon;
        x.staticRecon = staticRecon;
        o( x.rox != null );
        x.staticRecon = _static;
        #else
        o( x.rox != null );
        #endif
    }

    [Test] public void RoE(){
        o( x.roe != null );
    }

    #if !AL_THREAD_SAFE

    [Test] public void OrderedComposite_do(){
        x.Seq();
        o( x.@do != null );
    }

    #pragma warning disable 618

    [Test] public void OrderedComposite_and_or_deprecated(){
        o( x.and, Task.iterator);
        o( x.or, Task.iterator);
    }

    [Test] public void OrderedComposite_end_deprecated(){
        var z = x.Sequence();
        o( x.end, status.@unchecked(2) );
    }

    [Test] public void OrderedComposite_loop_deprecated(){
        var z = x.Sequence();
        o( x.loop.complete );
    }

    #endif  // !AL_THREAD_SAFE

    [Test] public void Reset_didHaveStore(){
        x._store = new Active.Core.Details.HashStore();
        x.Reset();
        o(x._store != null);
    }

    [Test] public void Reset_didNotHaveStore(){
        x.Reset();
        o(x._store == null);
    }

    #pragma warning restore 618

    [Test] public void Register(){
        var z = new Value();
        x.Register(z);
        z.value = true;
        x.Reset();
        o(z.value, false);
    }

    [Test] public void Release(){
        var z = new Value();
        x.Register(z);
        x.Release();
        z.value = true;
        x.Reset();
        o(z.value, true);
    }

    [Test] public void ImplicitStatus() => o( ((status)x).failing );

    [Test] public void ImplicitStatusFunction()
    => o( ((System.Func<status>)x)().failing );

    // TODO likely duplicates =======================================

    #if UNITY_2018_1_OR_NEWER
    [Test] public void After()    => o( x.After(1, 0)   == null);
    [Test] public void Timeout()  => o( x.Timeout(1, 0) != null);
    #else
    // TODO this is probably wrong
    [Test] public void Timeout()  => o( x.Timeout(1, 0) != null);
    #endif
    [Test] public void Cooldown() => o( x.Cooldown(1, 0)!= null);
    [Test] public void Init()     => o( x.With(0)       != null);
    [Test] public void Latch()    => o( x.Latch(false)  == null);
    [Test] public void Once()     => o( x.Once(0)       != null);
    // TODO - enable in a branch first
    //[Test] public void Undef()    => o( x.undef(0)      != null);

    class C : Task{}

    class Value: Active.Core.Details.Resettable{
        public bool value = false;
        public action Reset(){ value = false; return action.done(); }
    }

}

#endif // !AL_BEST_PERF
