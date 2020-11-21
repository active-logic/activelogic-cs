using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using InvOp = System.InvalidOperationException;

public class TestLogTrace : CoreTest{

    [Test] public void Constructors_nologging(){
        var lg = status.log;
        status.log = false;
        Assert.Throws<InvOp>( () => new LogTrace(null, null));
        Assert.Throws<InvOp>( () => new LogTrace(null, null, null));
        status.log = lg;
    }

    // TODO - don't think LogTrace allows a null scope; then
    // the `?.` in `Matches` will never be used.
    [Test] public void Matches(){
        var trace = new LogTrace(new object(), null);
        trace.Matches(null, null);
    }

}
