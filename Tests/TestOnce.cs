using NUnit.Framework;
using Active.Core;


public class TestOnce : DecoratorTest<Once> {

    [Test] public void Cycle(){
        o( x.pass.HasValue ); // Works the first time
        x.OnStatus(done.Via());
        o( x.pass, null);     // Not the second
        o( x.pass, null);     // or third
        x.Reset();
        o( x.pass.HasValue ); // Ready for another round
    }

}
