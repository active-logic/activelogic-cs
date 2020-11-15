using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestAbstractDecorator
             : DecoratorTest<TestAbstractDecorator.Concrete>{

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
