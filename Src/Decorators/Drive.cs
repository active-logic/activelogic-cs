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

    #if !AL_OPTIMIZE

    protected object target;
    #endif

    public Gate? this[status @in, bool crit]{ get{
        StatusRef.hold = @in;
        return @in.running ? Eval(crit) : Bypass();
    }}

    public Gate? this[bool @in, bool crit]
    => this[@in ? status.cont() : status.fail(), crit];

    protected Gate Eval(bool crit, ValidString reason=null)
    => new Gate(this, crit, new LogData(this, ".", reason));

    protected Gate? Bypass(ValidString reason=null){
        #if !AL_OPTIMIZE
        SetLogData(target, reason);
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
            return new StatusRef(crit ? s : StatusRef.hold, logData);
        }}

    }

    // ===============================================================

}

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

}
