#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using ArgEx = System.ArgumentException;
using Active.Core.Details;

namespace Active.Core{

partial class AbstractDecorator{

    internal static readonly LogString log = null;
    static int MaxId = 0;

    public abstract action Reset();

    override public string ToString()
    => StatusFormat.Decorator(this);

    #if !AL_OPTIMIZE
    protected void SetLogData(object target, string reason)
    => StatusRef.SetLogData(this, target, reason);
    #endif

    internal readonly struct LogData{

        public readonly AbstractDecorator scope;
        readonly object target;
        readonly string reason;

        // NOTE: s == null allowed only for testing purposes
        public LogData(AbstractDecorator s, object tg, string r){
            //if(s == null) throw new ArgEx("Scope cannot be null");
            scope = s; target = tg; reason = r;
        }

        public string Reason()
        #if AL_OPTIMIZE
        => null;
        #else
        => TraceFormat.DecoratorReason(target, reason);
        #endif

    }

}}
