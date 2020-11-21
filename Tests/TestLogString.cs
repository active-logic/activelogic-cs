using Ex = System.Exception;
using NUnit.Framework;
//using Active.Core;
using Active.Core.Details;
using RBE = Microsoft.CSharp.RuntimeBinder.RuntimeBinderException;

public class TestLogString : TestBase{

    LogString valid;
    LogString invalid;

    [SetUp] public void Setup(){
        valid = new LogString("Foo", valid: true);
        invalid = new LogString("Foo");
    }

    [Test] public void Valid(){
        o( !invalid.valid );
        o( valid.valid );
    }

    [Test] public void ToString_(){
        o( valid.ToString(), "Foo");
        Assert.Throws<Ex>( () => invalid.ToString() );
    }

    [Test] public void Invalid(){
        o( LogString.Invalid( invalid ) );
        o( !LogString.Invalid( valid ) );
    }

    [Test] public void @true()
    => Assert.Throws<Ex> (() => { var z = (dynamic)valid || true; });

    [Test] public void @false()
    => Assert.Throws<RBE> (() => { var z = (dynamic)valid && true; });

    [Test] public void Implicit(){
        LogString x = "Foo";
        o( x.valid, false);
    }

    [Test] public void AND() => o( (valid & invalid).valid );

    [Test] public void OR()
    => Assert.Throws<Ex>( () => { var z = valid | invalid; } );

}
