// Doc/Reference/OrderedComposites.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using static Active.Core.status; using Active.Core.Details;
using T = Active.Core.Sel;

namespace Active.Core{
public class Sel : Comp{

    internal static T self => (T)stack.Peek();

    public Sel() => key = state = fail();

    #if !AL_OPTIMIZE
    public T Step(bool repeat, CallInfo i){
        callInfo = i;
        return Step(repeat);
    }
    #endif

    public T Step(bool repeat=true){
        if(repeat && !state.running){ index = 0; state = key; }
        ι = 0;
        stack.Push(this); return this;
    }

    public static T task
    => (self.state != !self.key && self.ι++ == self.index)
       ? self : null;

    override public Comp this[status s]{ get{
       state = s; if(s == key){ index++; } return this;
    }}

    public static T operator - (T x, Comp y) => x;
    public static T operator % (T x, Comp y) => x;

}}
