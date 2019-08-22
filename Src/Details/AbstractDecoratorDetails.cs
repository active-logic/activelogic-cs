#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;
//using InvOp = System.InvalidOperationException;

namespace Active.Core{

public interface IDecorator{}

partial class AbstractDecorator{

    protected static readonly LogString log = null;
    static int MaxId = 0;
    public abstract action Reset();
    override public string ToString() => StatusFormat.Decorator(this);

    internal readonly struct LogData{

        public readonly AbstractDecorator scope;
        readonly object target;
        readonly string reason;

        public LogData(AbstractDecorator s, object tg, string r){
            scope = s; target = tg; reason = r;
        }

        public string Reason() => TraceFormat.DecoratorReason(target, reason);

    }

}}
