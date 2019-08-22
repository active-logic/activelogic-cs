#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using F = Active.Core.Details.StatusFormat;

public class TestCooldown : DecoratorTest<TestCooldown.Cooldown> {

	[Test] public void Constructor()
		=> o ( new Cooldown(5).duration, 5 );

	[Test] public void Initializer()
		=> o ( new Cooldown(){ duration = 5 }.duration, 5 );

	[Test] public void NoArgsForm([Range(-1, +1)] int val){
        status s = x.pass?[status.@unchecked(val)];
    }

	[Test] public void InitiallyPassing(){
		t = 0; o (x?.pass != null );
	}

	[Test] public void NotEnabledOnTaskRunning(){
		t = 5;
		o (x[5] != null);
		x.OnStatus(cont.Via(log && "Test"));
		o (x[5] != null);
	}

	[Test] public void EnableOnTaskFailed(){
		t = 5;
		o (x[5] != null);
		x.OnStatus(fail.Via(log && "Test"));
		o (x[5] == null);
	}

	[Test] public void EnableOnTaskdone(){
		t = 5;
		o (x[5] != null );
		x.OnStatus(done.Via(log && "Test"));
		o (x[5] == null);
	}

	[Test] public void DisableOnTimeout(){
		t = 5;
		o (x[5] != null);
		x.OnStatus(done.Via(log && "Test"));
		t = 10;
		o (x[5] != null);
	}

  #if !AL_OPTIMIZE
	[Test] public void Format(){
		t = 0;
		status s = x[5]?[done.Via(log && "Reason")];
		// '?' indicates the decorator doesn't know what it's gating just yet.
		// But we're about to find out hence '-> Test'
		o (F.Status(s), "* <C> [5.0] -> TestCooldown.Format (Reason)");
		s = x[5]?[done.Via(log && "Test")];
		// Since the target returned 'done' we're now on cooldown. The target
		// scope was stored in the previous run, so the '? ->' notation drops.
		o (F.Status(s), "  <C> [5.0] TestCooldown.Format");
	}
  #endif

	public class Cooldown : Active.Core.Cooldown{
		public int time_;
		public Cooldown() : base(){}
		public Cooldown(float duration) : base(duration){}
		override protected float time => time_;
	} int t{ set => x.time_ = value; }

}
