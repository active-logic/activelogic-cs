using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class DecoratorTest<T> : CoreTest where T : AbstractDecorator, new(){

    protected static readonly LogString log = null;

    protected T x;

    [SetUp] virtual public void Setup()
    { x = new T(); StatusFormat.UseASCII(); status.log = true; }


}
