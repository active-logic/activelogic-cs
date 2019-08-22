// Doc/Reference/Status.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Core.status;

public class TestWaiter : CoreTest {

	protected static readonly LogString log = null;

	Deco x; [SetUp] public void Setup() => x = new Deco();

	[Test] public void Functional([Values(true, false)] bool lhs,
								  [Range(-1, 1)]        int  rhs,
								  [Values(true, false)] bool logging){
		status.log = logging;
		status s = x[lhs]?[status.@unchecked(rhs)];
		status.log = true;
	}

	[Test] public void Pass([Range(-1, 1)] int val){
		var z = status.@unchecked(val);
		status s = x[true]?[z];
		o(s, z);
	}

	[Test] public void Cont([Range(-1, 1)] int val){
		var z = status.@unchecked(val);
		status s = x[false]?[z];
		o(s, status._cont);
	}

	[Test] public void Construct(){}

	[Test] public void StatusNotification(){
		x.OnStatus(status.done());
		o (x.note.complete);
	}

	[Test] public void ConstructGate(){
		new Waiter.Gate(x);
	}

	[Test] public void NewGate([Range(-1, 1)] int val){
		var gate = new Waiter.Gate(x);
		var z    = status.@unchecked(val);
		o ( (status)gate[z], z );
		o ( x.note , z);
	}

	[Test] public void NewStatusRef([Range(-1, 1)] int val){
		var z = status.@unchecked(val);
		var w = new Waiter.StatusRef(z);
		o ( (status)w, z );
	}

  #if !AL_OPTIMIZE

	[Test] public void LoggingOnFailure(){
		status s = x[false]?[done(log && "Testing")];
		// Question mark indicates we don't know the subtask managed by this
		// decorator just yet
		o (TraceFormat.LogTrace(s.trace), "<D> Bear ?");
	}

	[Test] public void LoggingOnSuccess(){
		status s = x[true]?[done(log && "Testing")];
		// Question mark drops because we just don't format the target object
		// on success (since it gets evaluated, it has its own format phase)
		o (TraceFormat.LogTrace(s.trace),
		   "<D> Bug -> TestWaiter.LoggingOnSuccess (Testing)");
	}

	[Test] public void ViaDecorator(){
		o (status.log, true);
		var s = cont().ViaDecorator(x, log && "SetPos");
		o ( s.trace != null );
	}

	// Not much of a test, just basic screening
	[Test] public void DecoratorIds(){
		o ( Cooldown.id != Init.id     );
		o ( Init.id     != Interval.id );
	}

  #endif

	// ------------------------------------------------------------------------

	class Deco : Waiter{

		public status note;
		override public void OnStatus(status s) => note = s;
		override public action Reset() => @void();
		public Gate? this[bool pass] => pass ? done(log && "Bug")
											 : cont(log && "Bear");
	}

}
