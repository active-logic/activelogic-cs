// Doc/Reference/Decorator.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public abstract partial class Conditional : AbstractDecorator{

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

    public interface OptionalArguments{ Conditional.Gate? pass{ get; } }

}}  // Active.Core
