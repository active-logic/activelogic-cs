// Doc/Reference/Status.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using static Active.Status;

public class TestConditional : CoreTest {

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

	[Test] public void Fail([Range(-1, 1)] int val){
		var z = status.@unchecked(val);
		var f = status.@unchecked(-1);
		status s = x[false]?[z];
		o(s, f);
	}

	[Test] public void Construct(){}

	[Test] public void StatusNotification(){
		x.OnStatus(status.done());
		o (x.note.complete);
	}

	[Test] public void ConstructGate(){
		new Conditional.Gate(x, newLogData);
	}

	[Test] public void NewGate([Range(-1, 1)] int val){
		var gate = new Conditional.Gate(x, newLogData);
		var z    = status.@unchecked(val);
		o ( (status)gate[z], z );
		o ( x.note , z);
	}

/*
	[Test] public void NewStatusRef([Range(-1, 1)] int val){
		var z = status.@unchecked(val);
		var w = new Conditional.StatusRef(z, newLogData);
		o ( (status)w, z );
	}
*/

  #if !AL_OPTIMIZE

	[Test] public void LoggingOnFailure(){
		status s = x[false]?[done(log && "Testing")];
		// Question mark indicates we don't know the subtask managed by this
		// decorator just yet
		o (TraceFormat.LogTrace(s.trace).StartsWith("<D> Bear ?"));
	}

	[Test] public void LoggingOnSuccess(){
		status s = x[true]?[done(log && "Testing")];
		// Question mark drops because we just don't format the target object
		// on success (since it gets evaluated, it has its own format phase)
		o (TraceFormat.LogTrace(s.trace),
		   "<D> Bug -> TestConditional.LoggingOnSuccess (Testing)");
	}

	[Test] public void ViaDecorator(){
		o (status.log, true);
		var s = cont().ViaDecorator(x, log && "SetPos");
		o ( s.trace != null );
	}

/*
	[Test] public void ToStatusWithLog(){
		var @ref = new Conditional.StatusRef(done(), newLogData);
		var s = (status)@ref;
	}
*/

	[Test] public void NestedLogs(){
		var x = new Deco(_done: "+Out", _fail: "-Out");
		var y = new Deco(_done: "+In" , _fail: "-In" );
		bool z = false;
		var s = x[true]?[ y[true]?[z=true] ];
		o(z, true);
		o(StatusFormat.Status(s),
		  "* <D> +Out -> <D> +In -> StatusDetails.op_Implicit");
	}

/*
	// NOTE: static scope only used when Gate/StatusRef is null
	[Test] public void ToStatusWithLog_badStaticScope(){
		Conditional.logData
			= new AbstractDecorator.LogData(null, null, null);
		Assert.Throws<System.InvalidOperationException>(()=>{
			Conditional.StatusRef? @ref = null;
			var s = (status)@ref;
		});
	}
*/

	// Not much of a test, just basic screening
	[Test] public void DecoratorIds(){
		o ( Cooldown.id != Init.id     );
		o ( Init.id     != Interval.id );
	}

  #endif  // !AL_OPTIMIZE

    // ..............................................................

  	// Pushing 'x' here is only because an instance is needed
  	Conditional.LogData newLogData
  	=> new Conditional.LogData(x, null, null);

	// --------------------------------------------------------------

	class Deco : Conditional{

		public string doneMsg, failMsg;
		public status note;

		public Deco(string _done="Bug", string _fail="Bear"){
			doneMsg = _done; failMsg = _fail;
		}

		override public void OnStatus(status s) => note = s;

		override public action Reset() => @void();

		public Gate? this[bool pass] => pass ? done(log && doneMsg)
											 : fail(log && failMsg);
	}

}
