// TODO: enable
/*
using System;
using NUnit.Framework;
using Active.Rx; using S = Active.Rx.status;
using static Active.Rx.status;

public class TestRxStatus : TestBase {

	readonly S F = new S(-1), R = new S(0), D = new S(1);

	[Test] public void ConstructFailingStatus() => o ( new S(-1).failing  );
	[Test] public void ConstructdoneStatus   () => o ( new S(+1).complete );
	[Test] public void ConstructRunningStatus() => o ( new S( 0).running  );

	[Test] public void Map(){
		o ( D.Map(D, F, R).running  );
		o ( F.Map(D, F, R).complete );
		o ( R.Map(D, F, R).failing  );
	}

	[Test] public void PassByValue(){
		S a = new S(1);
		S b = a;
		o ( a, b );
		o ( b, a );
		o ( a.Equals(b) );
		o ( b.Equals(a) );
		o ( !Object.ReferenceEquals(a, b) );
	}

	[Test] public void OpSeq(){
		o ( R && R, R );
		o ( R && F, R );
		o ( R && D, R );
		//
		o ( F && R, F );
		o ( F && F, F );
		o ( F && D, F );
		//
		o ( D && F, F );
		o ( D && D, D );
		o ( D && R, R );
	}

	[Test] public void OpSel(){
		o ( R || R, R );
		o ( R || F, R );
		o ( R || D, R );
		//
		o ( F || R, R );
		o ( F || F, F );
		o ( F || D, D );
		//
		o ( D || F, D );
		o ( D || D, D );
		o ( D || R, D );
	}

	[Test] public void OpAdd(){
		o ( R + R, R );
		o ( R + F, R );
		o ( R + D, D );
		//
		o ( F + R, R );
		o ( F + F, F );
		o ( F + D, D );
		//
		o ( D + F, D );
		o ( D + D, D );
		o ( D + R, D );
	}

	[Test] public void OpMul(){
		o ( R * R, R );
		o ( R * F, F );
		o ( R * D, R );
		//
		o ( F * R, F );
		o ( F * F, F );
		o ( F * D, F );
		//
		o ( D * F, F );
		o ( D * D, D );
		o ( D * R, R );
	}

	[Test] public void OpNeutralOver(){
		o ( R % R, R );
		o ( R % F, R );
		o ( R % D, R );
		//
		o ( F % R, F );
		o ( F % F, F );
		o ( F % D, F );
		//
		o ( D % R, D );
		o ( D % F, D );
		o ( D % D, D );
	}

	[Test] public void OpNot(){
		o ( !D != D );
		o ( !R == R );
		o ( !F != F );
		o ( (!D).failing  );
		o ( (!F).complete );
		o ( (!R).running  );
	}

	[Test] public void OpPromote(){
		o ( +F, R );
		o ( +R, D );
		o ( +D, D );
	}

	[Test] public void OpDemote(){
		o ( -F, F );
		o ( -R, F );
		o ( -D, R );
	}

	[Test] public void OpCondone(){
		o ( ~F, D );
		o ( ~R, R );
		o ( ~D, D );
	}

	[Test] public void Truehood(){
		o ( F == !true);
		o ( D ==  true);
	}

	[Test] public void Falsehood(){
		o ( F ==  false);
		o ( D == !false);
	}

	[Test] public void RunningIsNotTrueOrFalse(){
		o ( R == true, false);
		o ( R == false, false);
	}

	[Test] public void EqualsOp(){
		o ( null == F, false );
	}

	[Test] public void BoolToStatus(){
		o ((S)true , D);
		o ((S)false, F);
	}

	[Test] public void StatusToBool(){
		Assert.That(TryNarrowingConversion());
	}

	bool TryNarrowingConversion(){
		try{
			var boolean = (bool) ((dynamic)(R));
			return false;
		}catch(Exception){ return true; }
	}

	[Test] public void Invert(){
		var s = done;
		s = !s;
		o ( s != done );
	}

	[Test] public void ToString_(){
		o ( new S(-1).ToString().StartsWith("-1"));
		o ( new S( 0).ToString().StartsWith("0"));
		o ( new S(+1).ToString().StartsWith("1"));

	}

}
*/
