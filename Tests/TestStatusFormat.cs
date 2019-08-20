#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using F = Active.Core.Details.StatusFormat;

#if !AL_OPTIMIZE

public class TestStatusFormat : CoreTest {

    LogString log = null;

    [SetUp] public void Setup() => F.UseASCII();

    [Test] public void Decorator(){ o( F.Decorator(new Cooldown()), "<C>" ); }

    [Test] public void Name(){
        status s;
        s = fail.Via(log && "Test"); o ( F.Name(s), "failing" );
        s = done.Via(log && "Test"); o ( F.Name(s), "done" );
        s = cont.Via(log && "Test"); o ( F.Name(s), "running" );
    }

    [Test] public void SingleLevelCallTree(){
        var s = fail.Via(log && "Out of range", "NPC", "Attack");
        o ( F.CallTree(s), "  NPC.Attack (Out of range)\n" );
    }

    [Test] public void MultiLevelCallTree(){
		var s = (
            done.Via(log && "B.", "Foo", "Bar")
            && done.Via(log && "C.", "Foo", "Bar")
        ).Via(log && "A.", "Top", "Level");
		o ( F.CallTree(s),
            "* Top.Level (A.)\n  * Foo.Bar (B.)\n  * Foo.Bar (C.)\n");
	}

    [Test] public void Status(){
        o (F.Status(done.Via(log && "Out of range", "NPC", "Attack")),
            "* NPC.Attack (Out of range)" );
    }

    [Test] public void StatusWithInversionPrefix(){
        o (F.Status(!done.Via(log && "Attack", "A", "B")), "  !A.B (Attack)");
    }

    /*
    [Test] public void StatusWithPromotionPrefix(){
        o (F.Status((+cont).due.Via(
                               log && "Attack", "A", "B")), "* +A.B (Attack)");
    }

    [Test] public void StatusWithDemotionPrefix(){
        o (F.Status((-done).undue.Via(
                               log && "Attack", "A", "B")), "+ -A.B (Attack)");
    }

    [Test] public void StatusWithCondonePrefix(){
        o (F.Status((~fail).due.Via(
                               log && "Attack", "A", "B")), "* ~A.B (Attack)");
    }

    [Test] public void StatusWithMultiplePrefix(){
        o (F.Status(+-!~fail.Via(log && "Attack", "A", "B")),
                                         "+ +-!~A.B (Attack)" );
    }
    */

    [Test] public void TraceViaCallerInfo(){
        o (F.Status(done.Via()), "* TestStatusFormat.TraceViaCallerInfo");
    }

    [Test] public void Symbol(){
        o ( F.Symbol(fail.Via(log && ")Do")) == ' ' );
        o ( F.Symbol(cont.Via(log && ")Go")) == '+' );
        o ( F.Symbol(done.Via(log && ")Up")) == '*' );
    }

}
#endif  // AL_OPTIMIZE
