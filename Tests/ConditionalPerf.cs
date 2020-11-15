using System;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Status;

public class ConditionalPerf : TestBase {

	[Test] public void Benchmark([Values(false, true)] bool lhs,
                                 [Values(false, true)] bool logging){
        status.log = logging;
        var dec = new Stub();
		for(int rhs = -1; rhs <= +1; rhs++)
        	for(int i = 0; i < SlowTestIters; i++){
            	status s = dec[lhs]?[status.@unchecked(rhs)];
			}
        status.log = true;
	}

    class Stub : Conditional{
        override public void OnStatus(status s){}
		override public action Reset() => @void();
        public Gate? this[bool pass] => pass ? done() : fail();
    }

}
