#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;
using InvOp = System.InvalidOperationException;

namespace Active.Core{
partial class Waiter{

  #if !AL_OPTIMIZE
    internal static LogData logData;
    protected object target;
  #endif

     public readonly struct Gate{

         readonly Waiter o; public Gate(Waiter x) => o = x;

         public StatusRef this[status s]{ get{
           #if !AL_OPTIMIZE
             o.target = s.targetScope;
           #endif
             o.OnStatus(s); return new StatusRef(s);
         }}

     }

     // `StatusRef` is used instead of `status` because, when the gate is not
     // presented, this would results in a null `status?`. In context we know
     // that the null status denotes failure, but we would rather not define an
     // implicit conversion from null `status?` to status.fail() as this might
     // be error prone.
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

         static status ToStatus(StatusRef? self) => self?.x ?? status.cont();
      #if !AL_OPTIMIZE
         static status ToStatusWithLog(StatusRef? self){
             if(logData.scope == null) throw new InvOp("Log data is null");
             return (self?.x ?? status._cont)
                    .ViaDecorator(logData.scope, log && logData.Reason());
        }
      #endif

     }

}

}  // Active.Core
