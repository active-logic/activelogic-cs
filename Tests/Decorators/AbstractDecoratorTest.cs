using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Status;

public class AbstractDecoratorTest
             : DecoratorTest<AbstractDecoratorTest.C>{

    [Test] public void Time(){
        SimTime.time = 10f;
        o( x.GetTime(), 10f );
    }

    public class C : AbstractDecorator{
        public C(){}
        override public action Reset() => @void();
        public float GetTime() => time;
    }

    [Test] public void ConstructLogData(){
        var x = new AbstractDecorator.LogData(new C(), null, null);
    }

}
