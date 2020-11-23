#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestAbstractDecorator
             : DecoratorTest<TestAbstractDecorator.Concrete>{

    #if AL_OPTIMIZE
    [Test] public void Reason_isNull() => o(
        new AbstractDecorator.LogData(null, null, null)
        .Reason(), null );
    #endif

    [Test] public void Nature(){
        o( x is Resettable );
        o( x is IDecorator );
        o( x is AbstractDecorator );
    }

    [Test] public void FnReset() => x.Reset();

    [Test] public void FnToString() => o( x.ToString(), "<C>" );

    public class Concrete : AbstractDecorator
    { override public action Reset() => action.done(); }

}
