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

}

}  // Active.Core
