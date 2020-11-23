using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using InvOp = System.InvalidOperationException;
using Ex = System.Exception;

public class TestIterator : CoreTest{

    Iterator i;

    [SetUp] public void Setup() => i = new TIterator(new Sequence());

    [Test] public void Log() => o( Iterator.log == null );

    [Test] public void @true(){ if(i){ } else { Assert.Fail(); } }

    [Test] public void @false()
    => Assert.Throws<InvOp>( () => { var z = (dynamic)i && true; } );

    class TIterator : Iterator{

        public TIterator(Sequence s) : base(s) {}

        override public status this[in status x] => throw new Ex();
        override public status end               => throw new Ex();
        override public status loop              => throw new Ex();
        override public status repeat            => throw new Ex();

    }

}
