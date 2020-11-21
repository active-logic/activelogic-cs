using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestInterval : DecoratorTest<TestInterval.Interval> {

	static float t; // Time.time value for testing

	[SetUp] override public void Setup(){ t = 0; base.Setup(); }

	[Test] public void NoCatchupDefault() => o( x.catchup, false);

	[Test] public void PeriodOneDefault() => o( x.period, 1);

	// Constructors =================================================

	[Test] public void ConstructNoArg([Values(0, 3)]float st){
		t = st;
		x = new Interval();
		o ( x.period, 1 );
		o ( x.due, st);
	}

	[Test] public void ConstructWithDuration([Values(0, 3)]float st){
		t = st;
		x = new Interval(5);
		o ( x.period, 5 );
		o ( x.due, st);
	}

	[Test] public void ConstructWithDurationAndOffset(
											[Values(0, 3)]float st,
					                        [Values(0, 1)]float δt){
		t = st;
		x = new Interval(5, δt);
		o ( x.period, 5 );
		o ( x.due, st + δt);
	}

	[Test] public void Initializer()
	=> o ( new Interval(){ period = 5 }.period, 5 );

	// ==============================================================

	[Test] public void DefaultTestInstance(){
		o ( x.period, 1 );
		o ( x.due, 0);
		o ( x[5] != null );
	}

	[Test] public void NoArgsForm(){ var s = x.pass; }

	[Test] public void DontCatchup(){
		x = new Interval(3);
		x.stamp = 2;
		t = 11;
		o( x?.pass != null );
		o( x.stamp, 14 );
	}

	[Test] public void DoCatchup(){
		x = new Interval(3);
		x.catchup = true;
		x.stamp = 2;
		t = 11;
		o( x?.pass != null );
		o( x.stamp, 5 );
	}

    [Test] public void Bypass(){
        o(x[0] != null); // Repeated invocations
        o(x[0] != null); // Set the stamp but still
		t = 0;
        o(x[0] != null); // fires every time
    }

    [Test] public void Cycle(){
		StatusRef.checkLogData = false;
        t = 0;  // Fires on start
		o(x[5] != null);
        t = 8;  // Fires at t==8 (8 > 5)
		o(x[5] != null);
		t = 9;  // Don't fire at 9 (next is 10)
		o(x[5] == null);
		t = 10; // Fire at 10
        o(x[10] != null);
        o(x[10] == null); // Already fired
	}

	[Test] public void Reset([Values(0, 3)] float start){
		t = start;
		x.Reset();
		o(x.due, start);
	}

	[Test] public void EvalInc()
	=> o( 0.6f, Interval.EvalInc(p: 0.1f, o: 0, s: 1, t: 1.55f));

    // ==============================================================

	public class Interval : Active.Core.Interval{
		public Interval() : base(){}
		public Interval(float period, float offset=0f)
	    : base(period, offset){}
		override protected float time => TestInterval.t;
	}

}
