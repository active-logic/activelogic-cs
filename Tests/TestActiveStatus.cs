#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using NUnit.Framework; using Active.Core; using static Active.Status;

public class TestActiveStatus : TestStaticAPI{

    bool _log;

    [SetUp]    public void SaveLoggingState()    => _log = status.log;
    [TearDown] public void RestoreLoggingState() => status.log = _log;

    [Test] public void Standard(){
        o( done().complete  );
        o( fail().failing   );
        o( cont().running   );
        o( (status)@void(),  status._done );
        o( (status)@false(), status._fail );
        o( @forever().ever,  status._cont );
        // No short forms for pending/impending
        // ...
        // ...
        // ...
        // ...
    }

    #if !AL_OPTIMIZE
    [Test] public void Undef([Values(true, false)] bool lg){
        status.log = lg;
        o( undef().failing );
        o( undef(done()).complete );
        o( undef(cont()).running );
        o( undef(fail()).failing );
    }
    #endif

    // ==============================================================

    [Test] public void LongForm(){
        o( status.done().complete  );
        o( status.fail().failing   );
        o( status.cont().running   );
        o( (status)action.done(),  status._done );
        o( (status)failure.fail(), status._fail );
        o( loop.cont().ever,  status._cont );
        //
        o( pending.cont().running);
        o( pending.done().complete);
        o( impending.cont().running);
        o( impending.fail().failing);
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

    [Test] public void Do_  ([Values(true, false)] bool lg)
    { status.log = lg; o( Do(null), @void() );     }

    [Test] public void Cont_([Values(true, false)] bool lg)
    { status.log = lg; o( Cont(null), forever() ); }

    [Test] public void Fail_([Values(true, false)] bool lg)
    { status.log = lg; o( Fail(null), @false() );  }

}
