using NUnit.Framework;
using Active.Core; using static Active.Status;
using T = Active.Core.Comp;

public class TestComp : TestBase{

    Comp x; [SetUp] public void Setup() => x = new Comp();

    [Test] public void Reset(){
        x.state = fail();
        x.index = 3;
        x.ι = 2;
        x.Reset();
        o( x.key.running );
        o( x.index, 0);
        o( x.ι, 0);
    }

    [Test] public void @do([Values(-1, 1)] int key,
                           [Range (-1, 1)] int state,
                           [Values(true, false)] bool matchIndex){
        x.key   = status.@unchecked(key);
        x.state = status.@unchecked(state);
        x.ι     = matchIndex ? 0 : -1;
        x.index = 0;
        if(x.key.complete){      // for a sequence
            o(x.@do, x.state != fail() && matchIndex ? x : null);
        }else if(x.key.failing){ // for a selector
            o(x.@do, x.state != done() && matchIndex ? x : null);
        }
    }

    [Test] public void StatusConversion([Range(-1, 1)] int val){
        var s = status.@unchecked(val);
        x.Step();
        x.state = s;
        status s1;
        o( s1 = x, s);
    }

    class Comp : T{
        override public T this[status s] => null;
        public T Step(){ ι = 0; stack.Push(this); return this; }
    }

}
