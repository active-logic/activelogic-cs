using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using Ex = System.Exception;

public class TestIterator : CoreTest{

    Iterator i;

    [SetUp] public void Setup() => i = new TIterator(new Sequence());

    [Test] public void @true(){ if(i){ } else { Assert.Fail(); } }

    class TIterator : Iterator{

        public TIterator(Sequence s) : base(s) {}

        override public status this[in status x] => throw new Ex();
        override public status end               => throw new Ex();
        override public status loop              => throw new Ex();

    }

}
