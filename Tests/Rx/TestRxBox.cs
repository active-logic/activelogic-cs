using NUnit.Framework;
using Active.Rx;
using static Active.Rx.status;


public class TestBox : TestBase {

    [Test] public void Unbox(){
        var x = GetString();
        o( (string)x , "Hello" );
        o( x.complete, true    );
    }

    box<string> GetString() => done.With("Hello");

}
