using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class DecoratorTest<T> : CoreTest
                                   where T : AbstractDecorator, new(){

    protected static readonly LogString log = null;
    protected T x;

    [SetUp] virtual public void Setup(){
        x = new T();
        StatusFormat.UseASCII();
        status.log = true;
    }

    protected virtual float time{ get{
      #if UNITY_2018_1_OR_NEWER
        return UnityEngine.Time.time;
      #else
        return System.DateTime.Now.Millisecond/1000f;
      #endif
    }}
    
}
