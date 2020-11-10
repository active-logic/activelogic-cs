using NUnit.Framework;
using Active.Core;
using T = Active.Core.Wait;

public class TestWait : DecoratorTest<T> {

    [Test] public void ID() => o(T.id > -1);

	[Test] public void Constructor() => o(new T(5).delay, 5);

    [Test] public void ImplicitFromFloat() { x = 5f; o(x.delay, 5); }

    [Test] public void ImplicitToStatus() { status s = x; }

    [Test] public void Running([Values(true, false)] bool logging){
        status.log = logging;
        status s = x;
        o( s.running );
    }

    [Test] public void Complete([Values(true, false)] bool logging){
        status.log = logging;
        x.stamp = time - 1.05f;
        status s = x;
        o( s.complete );
    }

    [Test] public void Reset(){
        x.Reset();
        o( x.stamp, null);
        o( x[1f].running );
    }

}
