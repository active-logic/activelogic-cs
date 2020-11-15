#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using Active.Core;
using Active.Core.Details;
using static Active.Status;

using NUnit.Framework;
public class TestMeta : CoreTest {

    // This matches unwinding a node via Eval(). In this case the input node
    // (m) has prev node. The output (branch) must have 2 child nodes and no
    // predecessor.
    [Test] public void TestViaScope(){
        var m = new Meta(new LogTrace(this), prev: new status.Ref(cont()));
        o( m.prev != null);
        var branch = m.ViaScope(done(), this, "rea");
        o(branch.components.Length, 2);
        o(branch.prev, null);
    }

}

#endif  // !AL_OPTIMIZE
