using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Core.status;

public class AbstractDecoratorTest
             : DecoratorTest<AbstractDecoratorTest.C>{

    [Test] public void Time() => o( x.GetTime() != 0 );

    public class C : AbstractDecorator{
        public C(){}
        override public action Reset() => @void();
        public float GetTime() => time;
    }

}
