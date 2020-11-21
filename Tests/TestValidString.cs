using Ex = System.Exception;
using ArgEx = System.ArgumentException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestValidString : CoreTest {

    [Test] public void Implicit(){
        LogString s = ((LogString)null) && "Foo";
        ValidString v = s;
    }

    [Test] public void Implicit_invalid(){
        LogString s = new LogString("Foo", valid: false);
        Assert.Throws<ArgEx>(
            () => { var z = (ValidString)s; });
    }

}
