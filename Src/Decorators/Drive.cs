// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using InvOp = System.InvalidOperationException;
using Active.Core.Details;
using Self = Active.Core.Drive;

namespace Active.Core{
public class Drive : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    //
    static status hold;

    #if !AL_OPTIMIZE
    internal static LogData logData;
    protected object target;
    #endif

    public Gate? this[status @in, bool crit]{ get{
        hold = @in;
        return @in.running ? Eval(crit) : Bypass();
    }}

    public Gate? this[bool @in, bool crit]{ get{
        hold = @in ? status.cont() : status.fail();
        return @in ? Eval(crit) : Bypass();
    }}

    protected Gate Eval(bool crit, ValidString reason=null)
    => new Gate(this, crit, new LogData(this, ".", reason));

    protected Gate? Bypass(ValidString reason=null){
        #if !AL_OPTIMIZE
        logData = new LogData(this, target, reason);
        #endif
        return null;
    }

    // We don't have state, so ideally do not need a reset
    override public action Reset() => @void();

    // ==============================================================

    public readonly struct Gate{

        readonly Self owner;
        readonly LogData logData;
        readonly bool crit;

        internal Gate(Self owner, bool crit, LogData logData){
            this.owner = owner;
            this.crit = crit;
            this.logData = logData;
        }

        public StatusRef this[status s]{ get{
            #if !AL_OPTIMIZE
            owner.target = s.targetScope;
            #endif
            return new StatusRef(crit ? s : hold, logData);
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
         => self?.x ?? Self.hold;

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
	public Self.Gate? While(status @in, [Tag] int key = -1)
	=> store.Decorator<Self>(key, Self.id)[@in, crit: false];
    public Self.Gate? Tie(status @in, [Tag] int key = -1)
	=> store.Decorator<Self>(key, Self.id)[@in, crit: true];
    public Self.Gate? While(bool @in, [Tag] int key = -1)
    => store.Decorator<Self>(key, Self.id)[@in, crit: false];
    public Self.Gate? Tie(bool @in, [Tag] int key = -1)
    => store.Decorator<Self>(key, Self.id)[@in, crit: true];
}
#endif

}
