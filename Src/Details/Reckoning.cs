// Doc/Reference/Reset-Management.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

namespace Active.Core.Details{
public readonly struct Reckoning{

    #if !AL_OPTIMIZE
    internal readonly CallInfo callInfo;
    #endif
    internal readonly ReCon.Context context;

    public Reckoning(bool arg, ReCon stack){
        context = arg ? stack.Enter(forward: true) : null;
        #if !AL_OPTIMIZE
        callInfo = CallInfo.none;
        #endif
    }

    #if !AL_OPTIMIZE
    public Reckoning(bool arg, ReCon stack, CallInfo i){
        callInfo = i;
        context = arg ? stack.Enter(forward: true) : null;
    }
    #endif

    public status this[status s]{get{
        context?.Exit();
        #if AL_OPTIMIZE
        return s;
        #else
        return s / callInfo;
        #endif
    }}

}}
