#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestWait : DecoratorTest<TestWait.Wait> {

    #if !AL_OPTIMIZE
  	[Test] public void Format(){
  		t = 0;
  		status s = x[5];
        o (s.trace.isDecorator);
  		o (StatusFormat.Status(s), "+ <W> [5.0]");
  	}
    #endif

    public class Wait : Active.Core.Wait{
		public int time_;
		public Wait() : base(){}
		public Wait(float duration) : base(duration){}
		override protected float time => time_;
	}

    int t{ set => x.time_ = value; }

}
