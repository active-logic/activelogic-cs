// Doc/Reference/Decorator.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using UnityEngine;
using Active.Core.Details;

namespace Active.Core{
public abstract partial class AbstractDecorator : IDecorator, Resettable{

    protected virtual float time       => Time.time;
    protected static  int   ID(int id) => id == 0 ? ++MaxId : id;

}

public abstract partial class Decorator : AbstractDecorator{

    public abstract void OnStatus(status s);

    protected Gate done(ValidString reason=null){
      #if !AL_OPTIMIZE
        logData = new LogData(this, ".", reason);
      #endif
        return new Gate(this);
    }

    protected Gate? fail(ValidString reason=null){
      #if !AL_OPTIMIZE
        logData = new LogData(this, target, reason);
      #endif
        return null;
    }

    public interface OptionalArguments{ Decorator.Gate? pass{ get; } }

}}  // Active.Core
