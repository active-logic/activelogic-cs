#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using ArgEx = System.ArgumentException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using S = Active.Core.status;
using static Active.Core.status;

public class TestStatus : CoreTest {

	[Test] public void done_Const()    => o ( done.complete );
	[Test] public void fail_Const()    => o ( fail.failing  );
	[Test] public void cont_Const() => o ( cont.running  );

	[Test] public void done_Func()    => o ( done().complete );
	[Test] public void Fail_Func()    => o ( fail().failing  );
	[Test] public void Running_Func() => o ( cont().running  );

  #if !AL_OPTIMIZE

    LogString log = null;

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

	// ToString is not overriden in optimized mode
	[Test] public void ToString_(){
		o ( fail.ToString() , "failing ()" );
		o ( cont.ToString() , "running ()" );
		o ( done.ToString(),  "done ()"    );
	}

  #endif

	[Test] public void Map(){
		o (done.Map(done, fail, cont).running);
		o (fail.Map(done, fail, cont).complete);
		o (cont.Map(done, fail, cont).failing);
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

	[Test] public void Promoter(){
		o(+fail, pending._cont);
		o(+cont, pending._done);
		o(+done, pending._done);
	}

	[Test] public void Demoter(){
		o(-fail , impending._fail);
		o(-cont , impending._fail);
		o(-done , impending._cont);
	}

	[Test] public void Condoner(){
		o(~fail, pending._done);
		o(~cont, pending._cont);
		o(~done, pending._done);
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
    [Test] public void Falsehood(){}

    [Test] public void RunningIsTrueAndFalse(){
		if(cont) Assert.Pass(); else Assert.Fail();
		// Don't know how to verify falsehood
	}

    [Test] public void EqualsOp(){ o ( null == fail, false ); }

    //[Test] public void BoolToStatus(){ o((S)true, done); o((S)false, fail); }

    [Test] public void StatusToBool(){ o (TryNarrowingConversion()); }

	// Test internal APIs -----------------------------------------------------

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
