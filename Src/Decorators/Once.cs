// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using InvOp = System.InvalidOperationException;
using Active.Core.Details;
using Self = Active.Core.Once;

namespace Active.Core{
public class Once : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    //
    static status hold;
    //
	status state = cont();
    int frame = 0;

    #if !AL_OPTIMIZE
    internal static LogData logData;
    protected object target;
    #endif

	public Gate? pass{ get{
        Notices.OnEnter(ref frame, this);
        return state.running ? Eval() : Bypass();
    }}

    protected Gate Eval(ValidString reason=null)
    => new Gate(this, new LogData(this, ".", reason));

    protected Gate? Bypass(ValidString reason=null){
        #if !AL_OPTIMIZE
        logData = new LogData(this, target, reason);
        #endif
        hold = state;
        return null;
    }

    public void OnStatus(status s) => state = s;

    override public action Reset(){ state = cont(); return @void(); }

    // ==============================================================

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

    }

    // ===============================================================

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
         => self?.x ?? Once.hold;

         #if !AL_OPTIMIZE
         static status ToStatusWithLog(StatusRef? self){
             if(self.HasValue){
                 var ι = self.Value;
                 return ι.x.ViaDecorator(
                             ι.logData.scope,
                             log && ι.logData.Reason());
             }else{
                 if(Self.logData.scope == null) throw
                    new InvOp("Log data is null");
                 return hold.ViaDecorator(
                             Self.logData.scope,
                             log && Self.logData.Reason());
             }
        }
        #endif

    }  // StatusRef

}

#if !AL_BEST_PERF
partial class Task{
	public Self.Gate? Once([Tag] int key = -1)
	=> store.Decorator<Self>(key, Self.id)?.pass;
}
#endif

}
