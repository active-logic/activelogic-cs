#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;
using InvOp = System.InvalidOperationException;
using Self = Active.Core.Waiter;

namespace Active.Core{
partial class Waiter{

    #if !AL_OPTIMIZE
    internal static LogData logData;
    protected object target;
    #endif

    public readonly struct Gate{

        readonly Self owner; readonly LogData logData;

        internal Gate(Self owner, LogData logData)
        { this.owner = owner; this.logData = logData; }

        public StatusRef this[status s]{ get{
          #if !AL_OPTIMIZE
            owner.target = s.targetScope;
          #endif
            owner.OnStatus(s);
            return new StatusRef(s, logData);
        }}

    }  // Gate

    public readonly struct StatusRef{

         readonly status x;
         readonly LogData logData;

         internal StatusRef(status value, LogData logData)
         { x = value; this.logData = logData; }

         #if AL_OPTIMIZE
         public static implicit operator status(StatusRef? self)
         => ToStatus(self);
         #else
         public static implicit operator status(StatusRef? self)
         => status.log ? ToStatusWithLog(self) : ToStatus(self);
         #endif

         static status ToStatus(StatusRef? self)
         => self?.x ?? status.cont();

         #if !AL_OPTIMIZE
         static status ToStatusWithLog(StatusRef? self){
             if(self.HasValue){
                 var ι = self.Value;
                 return ι.x.ViaDecorator(
                             ι.logData.scope, ι.logData.Reason());
             }else{
                 if(Self.logData.scope == null) throw
                    new InvOp("Log data is null");
                 return status._cont.ViaDecorator(
                             Self.logData.scope,
                             Self.logData.Reason());
             }
        }
        #endif

    }  // StatusRef

}

}  // Active.Core
