#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using NUnit.Framework;
using Active.Core;
using static Active.Status; using Active.Core.Details;

#pragma warning disable 649

public class TestCallInfo : TestBase{

    CallInfo x, y;

    [SetUp] public void Setup() => y = ("n/a", "Moo", 17);

    [Test] public void Default(){
        o( x.path  , null );
        o( x.member, null );
        o( x.line  , 0    );
        o( x.Validate(), false );
    }

    [Test] public void FromTuple(){
        o( y.path  , "n/a" );
        o( y.member, "Moo" );
        o( y.line  , 17    );
        o( y.Validate(), true );
    }

    [Test] public void Div([Values(true, false)] bool lg){
        status.log = lg;
        status s = done() / y;
        o( s.complete );
    }

    [Test] public void Div_uninit([Values(true, false)] bool lg){
        status.log = lg;
        status s = done() / x;
        o( s.complete );
    }

}

#endif  // !AL_OPTIMIZE
