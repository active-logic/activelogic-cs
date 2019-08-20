using NUnit.Framework;
using Active.Core;

public class TestInterval : DecoratorTest<TestInterval.Interval> {

	[Test] public void NoArgConstructor(){
		x = new Interval();
		o ( x.period, 1 );
	}

	[Test] public void DurationConstructor(){
		x = new Interval(5);
		o ( x.period, 5 );
		o ( x.period, 5 );
	}

	[Test] public void FireOnStartConstructor([Values(false, true)] bool fos){
		x = new Interval(fireOnStart: fos);
		o ( x.period, 1 );
		t = 0;
		if(fos) o(x?.pass !=null ); else o(x?.pass, null );
	}

	[Test] public void ConstructorTwoArgs([Values(false, true)] bool fos){
		x = new Interval(period: 3, fireOnStart: fos);
		o ( x.period, 3 );
		t = 0;
		if(fos) o(x?.pass !=null ); else o(x?.pass, null );
	}

	[Test] public void Initializer()
		=> o ( new Interval(){ period = 5 }.period, 5 );

	[Test] public void NoArgsForm(){ var s = x.pass; }

	[Test] public void DontFireOnStart(){
		x = new Interval(fireOnStart: false);
		t = 0; o(x?.pass, null);
	}

    [Test] public void Bypass(){
        t = 0;
        o(x[0] != null); // Repeated invocations
        o(x[0] != null); // Set the stamp but still
        o(x[0] != null); // fires every time
    }

    [Test] public void Cycle(){
        t = 5;  // Will fire at or after t == 5
		o(x[5] != null);
        t = 8;  // Won't fire at t == 8
		o(x[5], null);
        t = 10; // Will fire again now
        o(x[5] != null);
        o(x[5] == null);     // Already fired (stamp set)
	}

	public class Interval : Active.Core.Interval{
		public int time_;
		public Interval() : base(){}
		public Interval(bool fireOnStart) : base(fireOnStart) {}
		public Interval(float period, bool fireOnStart = true)
			   : base(period, 0f, fireOnStart){}
		override protected float time => time_;
	} int t{ set => x.time_ = value; }

}
