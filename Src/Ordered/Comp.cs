// Doc/Reference/OrderedComposites.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using static Active.Core.status; using Active.Core.Details;
using T = Active.Core.Comp;

namespace Active.Core{
public abstract class Comp : Active.Core.Details.Resettable{

    #if !AL_OPTIMIZE
    protected CallInfo callInfo;
    #endif

    internal static Stack<T> stack = new Stack<T>();

    internal status key, state;
    internal int index, ι;

    abstract public T this[status s]{ get; }

    public action Reset(){
        state = key;
        index = ι = 0;
        return @void();
    }

    public static implicit operator status(T x){
        var @this = (T)stack.Pop();
        #if AL_OPTIMIZE
        return @this.state;
        #else
        return @this.state / @this.callInfo;
        #endif
    }

}}
