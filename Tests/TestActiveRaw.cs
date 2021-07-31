#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework; using Active.Core; using static Active.Raw;
using Ex = System.Exception;

public class TestActiveRaw : TestBase{

    // ...
    // ...
    // ...

    [Test] public void Standard(){
        o( done.complete  );
        o( fail.failing   );
        o( cont.running   );
        o( (status)@void,  status._done );
        o( (status)@false, status._fail );
        o( @forever.ever,  status._cont );
        // Raw values are provided for pending/impending
        o( pending_cont.running );
        o( pending_done.complete );
        o( impending_cont.running );
        o( impending_fail.failing );
    }

    #if !AL_OPTIMIZE
    [Test] public void Undef(){
        o( undef().failing );
        o( undef(done).complete );
        o( undef(cont).running );
        o( undef(fail).failing );
    }
    #endif

    // ==============================================================

    [Test] public void LongForm(){
        o( status._done.complete  );
        o( status._fail.failing   );
        o( status._cont.running   );
        o( (status)action._done,  status._done );
        o( (status)failure._fail, status._fail );
        o( loop._cont.ever,  status._cont );
        // Status constants not provided for pending/impending
        o( pending._cont.running);
        o( pending._done.complete);
        o( impending._cont.running);
        o( impending._fail.failing);
    }

    [Test] public void Eval_([Range(-1, 1)] int val){
        var s0 = status.@unchecked(val);
        o( s0, Eval(s0) );
    }

    [Test] public void Do_  () => o( Do(null), @void );
    [Test] public void Cont_() => o( Cont(null), forever );
    [Test] public void Fail_() => o( Fail(null), @false );

}
