// Doc/Reference/OrderedComposites.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using static Active.Status; using Active.Core.Details;
using T = Active.Core.Comp;

namespace Active.Core{
public abstract class Comp : Active.Core.Details.Resettable{

    #if !AL_OPTIMIZE
    protected CallInfo callInfo;
    #endif

    internal static Stack<T> stack = new Stack<T>();

    internal status key, state;
    internal int index, ι;

    public static T current => stack.Peek();

    #if !AL_OPTIMIZE
    internal T Step(bool repeat, CallInfo i)
    {callInfo = i; return Step(repeat); }
    #endif

    internal T Step(bool repeat){
        if(repeat && !state.running){ index = 0; state = key; }
        ι = 0;
        stack.Push(this); return this;
    }

    public T @do => (state != !key && ι++ == index) ? this : null;

    abstract public T this[status s]{ get; }

    public static implicit operator status(T x){
        var @this = (T)stack.Pop();
        #if AL_OPTIMIZE
        return @this.state;
        #else
        return @this.state / @this.callInfo;
        #endif
    }

    public action Reset(){
        state = key;
        index = ι = 0;
        return @void();
    }

}}
