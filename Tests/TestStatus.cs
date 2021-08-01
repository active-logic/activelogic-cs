#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using ArgEx = System.ArgumentException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using S = Active.Core.status;
using static Active.Status;

public class TestStatus : CoreTest {

	[Test] public void done_Const()    => o ( done.complete );
	[Test] public void fail_Const()    => o ( fail.failing  );
	[Test] public void cont_Const() => o ( cont.running  );

	[Test] public void done_Func()    => o ( done().complete );
	[Test] public void Fail_Func()    => o ( fail().failing  );
	[Test] public void Running_Func() => o ( cont().running  );

	// --------------------------------------------------------------

	#if AL_OPTIMIZE
	// optimized mode
	[Test] public void Construct_withLogTrace(){
		status a = done();
		o( new status(a, trace: null).complete );
	}

	// optimized mode
	[Test] public void ToString_(){
		o ( fail.ToString() , "fail" );
		o ( cont.ToString() , "cont" );
		o ( done.ToString(),  "done"    );
		o ( status.@unchecked(5).ToString(), "invalid_status(5)");
	}

	// optimized mode
	[Test] public void Indexer(){
		o( done[null].complete );
	}

	// optimized mode
	[Test] public void WithValidString(){
		o( (done % (ValidString)null).complete );
	}

	#endif

	[Test] public void Construct_withValueAndTrace(
									 [Values(true, false)] bool lg){
		var _log = status.log;
		//
		var z = new status(0, new LogTrace( this ), null);
		//
		status.log = _log;
	}

	[Test] public void Construct_withStatusAndPrev_1(){
		status a = done(), b = fail();
		o( new status(a, prev: b).complete );
	}

	[Test] public void Construct_withStatusAndPrev_2()
	=> o( new status(done(), prev: fail(), value: 5).complete);

	[Test] public void Equals_type_mismatch(){
		o(done.Equals( "1" ), false);
	}

	#if AL_OPTIMIZE
	[Test] public void ViaDecorator()
	=> o( done.ViaDecorator(null, null).complete );
	#endif

	#if !AL_OPTIMIZE

	LogString log = null;

	[Test] public void Construct_logtraceMissing(){
		Assert.Throws<ArgEx>( () =>
			{ var z = new status(0, null, null); });
	}

	[Test] public void Construct_loggingDisabled(){
		var _log = status.log;
		//
		status.log = false;
		var z = new status(0, null, null);
		//
		status.log = _log;
	}

	[Test] public void ConstructWithCI(){
		status[] S = { done(), fail(), cont() };
		foreach(var s in S) o(s.trace.scope,
			       $"{nameof(TestStatus)}.{nameof(ConstructWithCI)}");
	}

	[Test] public void ConstructWithCIAndReason(){
		var r = "Reason";
		status[] S = { done(log && r), fail(log && r), cont(log && r) };
		foreach(var s in S){
			o(s.trace.scope, $"{nameof(TestStatus)}."+
						     $"{nameof(ConstructWithCIAndReason)}");
			o(s.trace.reason, r);
		}
	}

	[Test] public void Validate(){
		Assert.Throws<ArgEx>( () => status.Validate(12) );
	}

	[Test] public void Via(){
		var s = done.Via(log && "SetPos").Via(log && "Move");
		o ( s.trace.scope.ToString(), "TestStatus.Via" );
		o ( s.trace.next.scope.ToString(), "TestStatus.Via");
	}

	[Test] public void ViaImplicit(){
		var s = done.Via();
		o ( s.trace.scope.ToString(), "TestStatus.ViaImplicit" );
	}

	// Check removed in optimized mode
	[Test] public void InvalidLogicalAND(){
		try { var s = cont & cont; Assert.Fail();}catch(ArgEx){}
		try { var s = cont & fail; Assert.Fail(); }catch(ArgEx){}
		try { var s = cont & done; Assert.Fail(); }catch(ArgEx){}
		try { var s = fail & cont; Assert.Fail(); }catch(ArgEx){}
		try { var s = fail & fail; Assert.Fail(); }catch(ArgEx){}
		try { var s = fail & done; Assert.Fail(); }catch(ArgEx){}
	}

	// Check removed in optimized mode
	[Test] public void InvalidLogicalOR(){
		try { var s = cont | cont; Assert.Fail(); }catch(ArgEx){}
		try { var s = cont | fail; Assert.Fail(); }catch(ArgEx){}
		try { var s = cont | done; Assert.Fail(); }catch(ArgEx){}
		try { var s = done | cont; Assert.Fail(); }catch(ArgEx){}
		try { var s = done | fail; Assert.Fail(); }catch(ArgEx){}
		try { var s = done | done; Assert.Fail(); }catch(ArgEx){}
	}

	// TODO: optimized mode version
	[Test] public void ToString_(){
		o ( fail.ToString() , "failing ()" );
		o ( cont.ToString() , "running ()" );
		o ( done.ToString(),  "done ()"    );
		//o ( new status(5).ToString(), "invalid_status(5)");
	}

	[Test] public void Condoner_withTrace(){
		o(~status.fail(log && "boo"), status._done);
	}

    #endif  // !AL_OPTIMIZE

	// SCHEDULED FOR DEPRECATION ====================================

	#pragma warning disable 618

	#if !AL_OPTIMIZE

	[Test] public void Eval_ε([Values(true, false)] bool lg){
		var _log = status.log;
		//
		status.log = lg;
		o( status.ε(done, "path").complete );
		o( status.ε(cont, "path").running  );
		o( status.ε(fail, "path").failing  );
		//
		status.log = _log;
	}

	#endif

	[Test] public void Eval(){

		o( status.Eval(done).complete );
		o( status.Eval(cont).running  );
		o( status.Eval(fail).failing  );
	}

	[Test] public void StatusConsts(){
		o( (status)status.@void(),   done);
		//o( (status)status.@false(),  fail);
		o( (status)status.flop(),    fail);
		o( (status)status.forever(), cont);
	}

	#pragma warning restore 618

	// ==============================================================

    [Test] public void GetHashCode_(){
		o( done.GetHashCode(), +1);
		o( cont.GetHashCode(),  0);
		o( fail.GetHashCode(), -1);
	}

	[Test] public void Map(){
		o (done.Map(done, fail, cont).running);
		o (fail.Map(done, fail, cont).complete);
		o (cont.Map(done, fail, cont).failing);
		Assert.Throws<System.ArgumentException>(
		    () => status.@unchecked(5).Map(done, fail, cont));
	}

	[Test] public void Sequencer(){
		o( cont && cont  , cont);
		o( cont && fail  , cont);
		o( cont && done  , cont);
		//
		o( fail && cont , fail );
		o( fail && fail , fail );
		o( fail && done , fail );
		//
		o( done && fail, fail  );
		o( done && done , done );
		o( done && cont , cont );
	}

	[Test] public void ValidLogicalAND(){
		o( done & fail, fail  );
		o   ( done & done, done  );
		o( done & cont , cont );
	}

	[Test] public void ValidLogicalOR(){
		o( fail | cont , cont );
		o( fail | fail , fail );
		o   ( fail | done , done );
	}

	[Test] public void LenientCombinator(){
		o( cont + cont , cont );
		o( cont + fail , cont );
		o   ( cont + done, done );
		//
		o( fail + cont , cont );
		o( fail + fail , fail );
		o   ( fail + done , done);
		//
		o   ( done + fail , done );
		o   ( done + done , done);
		o   ( done + cont , done );
	}

	[Test] public void StrictCombinator(){
		o( cont * cont, cont  );
		o( cont * fail , fail);
		o( cont * done , cont);
		//
		o( fail * cont , fail );
		o( fail * fail , fail );
		o( fail * done , fail);
		//
		o( done * fail , fail );
		o   ( done * done, done );
		o( done * cont , cont );
	}

	[Test] public void NeutralCombinator(){
		o( cont % cont, cont  );
		o( cont % fail , cont );
		o( cont % done , cont);
		//
		o( fail % cont , fail );
		o( fail % fail , fail );
		o( fail % done, fail );
		//
		o( done % fail , done );
		o( done % done, done );
		o( done % cont , done );
	}

	[Test] public void Inverter(){
		o ( !done != done    );
		o ( !cont  == cont   );
		o ( !fail  != fail   );
		o ( (!done).failing  );
		o ( (!fail).complete );
		o ( (!cont).running  );
	}

	[Test] public void Promoter([Values(true, false)]bool lg){
		var z = status.log;
		status.log = lg;
		o(+fail, status._cont);
		o(+cont, status._done);
		o(+done, status._done);
		status.log = z;
	}

	[Test] public void Demoter([Values(true, false)]bool lg){
		var z = status.log;
		status.log = lg;
		o(-fail , status._fail);
		o(-cont , status._fail);
		o(-done , status._cont);
		status.log = z;
	}

	[Test] public void Condoner(){
		o(~fail, status._done);
		o(~cont, status._cont);
		o(~done, status._done);
	}

	[Test] public void PassByValue(){
  	  S a = done.Via();
  	  S b = a;
  	  o ( a, b );
  	  o ( b, a );
  	  o ( a.Equals(b) );
  	  o ( b.Equals(a) );
  	  o ( !Object.ReferenceEquals(a, b) );
    }

    [Test] public void Truehood(){
		if(fail) Assert.Fail(); else Assert.Pass();
		if(done) Assert.Pass(); else Assert.Fail();
	}

	// Don't know how to test this
    [Test] public void Falsehood(){
		o( done && @fail, @fail );
	}

    [Test] public void RunningIsTrueAndFalse(){
		if(cont) Assert.Pass(); else Assert.Fail();
		// Don't know how to verify falsehood
	}

    [Test] public void EqualsOp(){ o ( null == fail, false ); }

	[Test] public void IncrementOp(){
		status d = done, c = cont, f = fail;
		o(++f, cont); o(++c, done); o(++d, done);
	}

	[Test] public void DecrementOp(){
		status d = done, c = cont, f = fail;
		o(--f, fail); o(--c, fail); o(--d, cont);
	}

    [Test] public void StatusToBool(){ o (TryNarrowingConversion()); }

	// TODO: inconsistent across Unity, .NET
	[Test] public void StatusToString(){
		o( done.ToString().StartsWith("done") );
		o( fail.ToString().StartsWith("fail") );
		o(    cont.ToString().StartsWith("running")  // in .NET
		   || cont.ToString().StartsWith("cont"));   // in Unity
		var z = status.@unchecked(21);
		o(    z.ToString().StartsWith("invalid")     // in Unity
		   || z.ToString().StartsWith("done"));      // in .NET
	}

	// Test internal APIs -------------------------------------------

	[Test] public void iConstructdoneStatusWithStatusValue()
		=> o ( new S(StatusValue.done, newTrace).complete );

	[Test] public void iConstructRunningStatusWithStatusValue()
		=> o ( new S(StatusValue.Running, newTrace).running );

	[Test] public void iConstructFailingStatusWithStatusValue()
		=> o ( new S(StatusValue.Failing, newTrace).failing );

  #if !AL_OPTIMIZE

	[Test] public void iConstructWithScope(){
		o ( new S(StatusValue.done,
				  status.log ? new LogTrace("WithScope") : null).trace.scope,
			"WithScope" );
	}

	[Test] public void iConstructWithReason(){
		o ( new S(StatusValue.done,
				  status.log ? new LogTrace("S+", "R0") : null).trace.reason,
			"R0" );
	}

  #endif

	static LogTrace newTrace => status.log ? new LogTrace("n|a") : null;

	bool TryNarrowingConversion(){
		try{
			var boolean = (bool) ((dynamic)(cont));
			return false;
		}catch(Exception){ return true; }
	}

	bool Equals(S exp, S value){
		if(exp.Equals(value) == false) return false;
		else if((exp==value) == false) return false;
		else if((exp!=value) == true)  return false;
		return true;
	}

}
