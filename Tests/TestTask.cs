using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestTaskUnit : CoreTest{

    C x;

    [SetUp] public void Setup()   => x = new C();

    #if UNITY_2018_1_OR_NEWER

    [Test] public void After()    => o( x.After(1, 0) == null);

    [Test] public void Timeout()  => o( x.Timeout(1, 0) == null);

    #else

    // TODO this is probably wrong
    [Test] public void Timeout()  => o( x.Timeout(1, 0) != null);

    #endif

    [Test] public void Cooldown() => o( x.Cooldown(1, 0) != null);

    [Test] public void Selector() => o( x.Selector(0) != null);

    [Test] public void Sequence() => o( x.Sequence(0) != null);

    [Test] public void Init()     => o( x.With(0) != null);

    [Test] public void InOut()
                       => o( x.InOut(false, false, 0) == null );

    [Test] public void Interval() => o( x.Every(1, 0) != null );

    [Test] public void Latch()    => o( x.Latch(false) == null);

    [Test] public void Once()     => o( x.Once(0) != null);

    // TODO - enable in a branch first
    //[Test] public void Undef()    => o( x.undef(0)      != null);

    // From Composite.cs

    [Test] public void And() => o( x.And() is null );

    [Test] public void Or() => o( x.Or() is null );

    // From Task.cs

    [Test] public void Register() => x.Register(new Latch());

    [Test] public void Release() => x.Release();

    [Test] public void Reset() => o( x.Reset().now.complete );

    [Test] public void ToStatus() => o( ((status)x).failing );

    [Test] public void ToStatusFunc()
    => o( ((System.Func<status>)x).Method.Name is "Step" );

    class C : Task{

        public Iterator And() => and;
        public Iterator Or()  => or;

    }

}
