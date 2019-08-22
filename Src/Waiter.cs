// Doc/Reference/Decorator.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

// using UnityEngine;
using Active.Core.Details;

namespace Active.Core{
public abstract partial class Waiter : AbstractDecorator{

    public abstract void OnStatus(status s);

    protected Gate done(ValidString reason=null){
      #if !AL_OPTIMIZE
        logData = new LogData(this, ".", reason);
      #endif
        return new Gate(this);
    }

    protected Gate? cont(ValidString reason=null){
      #if !AL_OPTIMIZE
        logData = new LogData(this, target, reason);
      #endif
        return null;
    }

    public interface OptionalArguments{ Waiter.Gate? pass{ get; } }

}}
