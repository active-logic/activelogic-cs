using NUnit.Framework;
using Active.Core;
using F = Active.Core.Details.TraceFormat;
using T = Active.Core.Details.LogTrace;

public class TestTraceFormat : CoreTest {

  #if !AL_OPTIMIZE

  	[Test] public void TraceDecoratorWithReason(){
          var t = new T(new Cooldown(), next: null, reason: "[5] Steer");
          o ( F.LogTrace(t), "<C> [5] Steer" );
      }

  #endif

	[Test] public void ReasonField(){
		o ( F.ReasonField(null)           	, null );
		o ( F.ReasonField("")  				, null );
		o ( F.ReasonField("Foo")  			, "Foo" );
	}

	[Test] public void LogTrace(){
		var t = new T("SetPos", next: null, reason: null);
		o ( F.LogTrace(t), "SetPos" );
	}

	[Test] public void Prefix(){
        var t = new T("SetPos", next: null, reason: null);
		t.Prefix('!');
        o ( F.LogTrace(t), "!SetPos" );
		t.Prefix('+');
		o ( F.LogTrace(t), "+!SetPos" );
    }

	[Test] public void TraceWithReason(){
        var t = new T("SetPos", next: null, reason: "Teleport");
        o ( F.LogTrace(t), "SetPos (Teleport)" );
    }

	[Test] public void TraceWithReasonAndFormatArgs(){
        var t = new T("SetPos", next: null, reason: "Teleport here");
        o ( F.LogTrace(t), "SetPos (Teleport here)" );
    }

	[Test] public void TraceWithReasonAndInner1(){
		var t0 = new T("SetPos", next: null, reason: "Teleport");
        var t1 = new T("Reposition", next: t0, reason: "Out of range");
		o ( F.LogTrace(t0), "SetPos (Teleport)" );
        o ( F.LogTrace(t1), "Reposition (Out of range) -> SetPos (Teleport)" );
    }

	[Test] public void TraceWithReasonAndInner2(){
		var t0 = new T("SetPos", next: null, reason: "Teleport");
		var t1 = new T("Reposition", next: t0, reason: null);
		o ( F.LogTrace(t0), "SetPos (Teleport)" );
		o ( F.LogTrace(t1), "Reposition -> SetPos (Teleport)" );
	}

}
