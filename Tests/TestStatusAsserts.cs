using NUnit.Framework;
using Active.Core;
using static Active.Status;
using A = Active.Core.StatusAssert;

public class TestStatusAsserts{

    [Test] public void AssertComplete(){
        done().AssertComplete();
        Assert.Throws<A>( () => fail().AssertComplete() );
        Assert.Throws<A>( () => cont().AssertComplete() );
    }

    [Test] public void AssertFailing(){
        fail().AssertFailing();
        Assert.Throws<A>( () => done().AssertFailing() );
        Assert.Throws<A>( () => cont().AssertFailing() );
    }

    [Test] public void AssertRunning(){
        cont().AssertRunning();
        Assert.Throws<A>( () => done().AssertRunning() );
        Assert.Throws<A>( () => fail().AssertRunning() );
    }

    [Test] public void AssertPending(){
        cont().AssertPending();
        done().AssertPending();
        Assert.Throws<A>( () => fail().AssertPending() );
    }

    [Test] public void AssertImpending(){
        cont().AssertImpending();
        fail().AssertImpending();
        Assert.Throws<A>( () => done().AssertImpending() );
    }

    [Test] public void AssertImmediate(){
        done().AssertImmediate();
        fail().AssertImmediate();
        Assert.Throws<A>( () => cont().AssertImmediate() );
    }

}
