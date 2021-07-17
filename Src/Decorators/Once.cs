// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using InvOp = System.InvalidOperationException;
using Active.Core.Details;
using Self = Active.Core.Once;

namespace Active.Core{
public class Once : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    //
	internal status state = cont();
    int frame = 0;

    #if !AL_OPTIMIZE
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
        SetLogData(target, reason);
        #endif
        StatusRef.hold = state;
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

}  // Once

partial class Task{
	public Self.Gate? Once([Tag] int key = -1)
	=> store.Decorator<Self>(key, Self.id).pass;
}

}
