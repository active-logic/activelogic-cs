using NUnit.Framework;
using Active.Core;

public class TestTaskUnit : CoreTest{

    Task x;

    [SetUp] public void Setup()   => x = new C();

    #if UNITY_2018_1_OR_NEWER
    [Test] public void After()    => o( x.After(1, 0)   == null);
    [Test] public void Timeout()  => o( x.Timeout(1, 0) == null);
    #else
    // TODO this is probably wrong
    [Test] public void Timeout()  => o( x.Timeout(1, 0) != null);
    #endif
    [Test] public void Cooldown() => o( x.Cooldown(1, 0)!= null);
    [Test] public void Init()     => o( x.With(0)       != null);
    [Test] public void Latch()    => o( x.Latch(false)  == null);
    [Test] public void Once()     => o( x.Once(0)       != null);
    // TODO - enable in a branch first
    //[Test] public void Undef()    => o( x.undef(0)      != null);

    class C : Task{}

}
