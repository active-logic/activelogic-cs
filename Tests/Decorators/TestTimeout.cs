using NUnit.Framework;
using Active.Core;


/* Timeout limits the duration of the target task */
public class TestTimeout : DecoratorTest<TestTimeout.Timeout> {

	static float t{ set => SimTime.time = value; }

	[SetUp] override public void Setup(){ t = 0; base.Setup(); }

	[Test] public void NoArgConstructor([Values(0f, 3f)] float st){
		t = st;
		var dec = new Timeout();
		o(dec.due, 1 + st); o(dec.duration, 1);
	}

	[Test] public void Constructor([Values(0f, 3f)] float st){
		t = st;
		var dec = new Timeout(5);
		o(dec.due, 5 + st); o(dec.duration, 5);
	}

	[Test] public void Implicit(){
		Active.Core.Timeout z = 1f;
	}

	[Test] public void Initializer([Values(0f, 3f)] float st){
		t = st;
		var dec = new Timeout(){ duration = 5 };
		o(dec.due, 5 + st); o(dec.duration, 5);
	}

	[Test] public void NoArgsForm([Range(-1, +1)] int val)
	{ status s = x.pass?[status.@unchecked(val)]; }

	[Test] public void NoArgsForm(){ var s = x?.pass; }

	[Test] public void InitialState()
	{ t = 0; o (x[5] != null); }

    [Test] public void Triggered(){
        t = 0;
        x.OnStatus(cont.Via());  // Start the Timeout
        t = 6;          	     // Ran out of time
        o (x[5] == null);        // Not passing
        x.Reset();               // Reset the Timeout
        o (x[5] != null);        // Passing again
	}

    [Test] public void DisarmedOndone(){
        t = 4;
        x.OnStatus(cont.Via());  // Start the Timeout
        o (x[5] != null);        // There is still time
        x.OnStatus(done.Via());  // UTask done resets tmr
        t = 6;          		 // Still passing
        o (x[5] != null);
	}

    [Test] public void DisarmedOnFail(){
        t = 4;
        x.OnStatus(cont.Via());  // Start the Timeout
        x.OnStatus(fail.Via());  // UTask failed resets tmr
        t = 6;                   // Still passing
        o (x[5] != null);
	}

	public class Timeout : Active.Core.Timeout{
		public Timeout() : base(){}
		public Timeout(float duration) : base(duration){}
	}

}
