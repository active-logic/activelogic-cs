#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using NUnit.Framework; using Active.Core; using static Active.Raw;

public class TestActiveRaw : TestStaticAPI{

    // (logging not supported)
    //
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

    [Test] public void Eval_and_ε(
                             [Range(0, 9)] int i,
                             [Values(true, false)] bool lg,
                             [Values(true, false)] bool shorthand){
        status.log = lg;
        dynamic x = statuses[i];
        var y = shorthand ? ε(x) : Eval(x);
        o(x, y);
        o(x.GetType(), y.GetType());
    }

    [Test] public void Eval_Bool([Values(true, false)] bool x,
                                [Values(true, false)] bool lg,
                                [Values(true, false)] bool shorthand){
        status.log = lg;
        var y = shorthand ? ε(x) : Eval(x);
        o( (status)x, y );
        o( y.GetType(), typeof(status) );
    }

    [Test] public void Do_  () => o( Do(null), @void );
    [Test] public void Cont_() => o( Cont(null), forever );
    [Test] public void Fail_() => o( Fail(null), @false );

}
