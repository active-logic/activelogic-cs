// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Core.status;
using Active.Core.Details;
using InvOp = System.InvalidOperationException;
using Tag   = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
/*
Syntax:
with() ? [ exp  ] OP status_exp  // OP: +, % or -
with() ? [ cond ] ? [ status_exp ]
*/
public class Init : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    const string INCOMPLETE = "Incomplete 'with' pattern";
  #if !AL_OPTIMIZE
    internal static Decorator.LogData logData;
    protected object target;
  #endif
    //
    internal static Init current;
    status preconditionStatus;
    public bool passing = true;

    public Gate? this[status x]{ get{
        if(current != null) throw new InvOp(INCOMPLETE);
        current = this;
      #if !AL_OPTIMIZE
        logData = new Decorator.LogData(this, x.trace.scope, x.trace.reason);
      #endif
        if(x.complete){
            passing = false;
            return new Gate(this);
        }else{
            preconditionStatus = x;
            return null;
        }
    }}

    public Gate? this[object x]{ get{
        if(current != null) throw new InvOp(INCOMPLETE);
        current = this;
        if(x != null){
          #if !AL_OPTIMIZE
            logData = new Decorator.LogData(this, $"{x}: <{x.GetType().Name}>",
                                            null);
          #endif
            passing = false;
            return new Gate(this);
        }else{
          #if !AL_OPTIMIZE
            logData = new Decorator.LogData(this, "<null>", null);
          #endif
            preconditionStatus = fail();
            return null;
        }
    }}

    override public action Reset(){ passing = true; return @void(); }

    internal static status PreconditionStatus(){
        var s = Init.current.preconditionStatus;
        Init.current = null;
        return s;
    }

    // ------------------------------------------------------------------------

    public readonly struct Gate{

        readonly Init owner;

        public Gate(Init self) => owner = self;

        public StatusRef this[status s] { get{
          #if !AL_OPTIMIZE
            owner.target = s.targetScope;
          #endif
            owner.passing = !s.running;
            Init.current = null;
            return new StatusRef(s);
        }}

        public static status operator % (Gate? self, status s)
        { Owner(self).passing = !s.running; Init.current = null; return s; }

        public static status operator + (Gate? self, status s)
        { Owner(self).passing = s.complete; Init.current = null; return s; }

        public static status operator - (Gate? self, status s)
        { Owner(self).passing = s.failing; Init.current = null; return s; }

        static Init Owner(Gate? self) => self?.owner ?? Init.current;

    }  // Gate class

    public readonly struct StatusRef{

        readonly status x;

        public StatusRef(status value) => x = value;

      #if AL_OPTIMIZE
        public static implicit operator status(StatusRef? self)
        => ToStatus(self);
      #else
        public static implicit operator status(StatusRef? self)
        => status.log ? ToStatusWithLog(self) : ToStatus(self);
      #endif

        static status ToStatus(StatusRef? self)
        => self?.x ?? Init.PreconditionStatus();

      #if !AL_OPTIMIZE
        static status ToStatusWithLog(StatusRef? self){
            if(logData.scope == null) throw new InvOp("Log data is null");
            return (self?.x ?? Init.PreconditionStatus())
                   .ViaDecorator(logData.scope, log && logData.Reason());
       }
      #endif

   }  // StatusRef class

}

// ----------------------------------------------------------------------------

#if !AL_BEST_PERF
partial class Task{
    public Init With([Tag] int key = -1)
    => store.Decorator<Init>(key, Active.Core.Init.id);
}
#endif

}  // Active.Core
