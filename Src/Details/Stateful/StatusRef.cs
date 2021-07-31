// Internal
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using InvOp = System.InvalidOperationException;
using Active.Core;
using Active.Core.Details;
using static Active.Core.AbstractDecorator;

namespace Active.Core.Details{
public readonly struct StatusRef{

     internal static status hold;
     internal static bool checkLogData = true;
     readonly status x;
     readonly LogData logData;

     #if !AL_OPTIMIZE
     static LogData? staticLogData;
     #endif

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
     => self?.x ?? StatusRef.hold;

     #if !AL_OPTIMIZE

     internal static void SetLogData(AbstractDecorator dec,
                              object target, string reason){
        if(!status.log) return;
        if(checkLogData && staticLogData.HasValue)
            throw new InvOp("Clear log data first");
        staticLogData = new LogData(dec, target, reason);
    }

    // Only for unit testing; do not use.
    internal static void ClearLogData() => staticLogData = null;

     internal static status ToStatusWithLog(StatusRef? self){
         if(self.HasValue){
             var ι = self.Value;
             return ι.x.ViaDecorator(
                         ι.logData.scope,
                         log && ι.logData.Reason());
         }else{
             if(!status.log) return hold;
             var scope  = staticLogData.Value.scope;
             var reason = staticLogData.Value.Reason();
             if(scope == null) throw new InvOp("scope is null");
             staticLogData = null;
             return hold.ViaDecorator(scope, log && reason);
         }
    }
    #endif

}}  // StatusRef
