#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using Active.Core;

namespace Active.Core.Details{
public readonly struct Meta{

    internal readonly status[] components;
    internal readonly LogTrace trace;
    internal readonly status.Ref prev;

    internal Meta(LogTrace t, status[] c=null, status.Ref prev=null){
        if(t==null) throw new System.Exception();
        trace = t;
        components = c;
        this.prev = prev;
    }

    internal Meta(in Meta src, LogTrace t=null, status[] c=null,
                                                status.Ref prev=null){
        this = src;
        if(t    != null) this.trace      = t;
        if(c    != null) this.components = c;
        if(prev != null) this.prev       = prev;
    }

    // Note: 'owner' is the owner of this Meta field; it is the status that we
    // are wrapping, and possibly unwinding.
    internal Meta ViaScope(in status owner, object scope, string reason=null){
        if(prev == null){
            if(trace?.Matches(scope, reason) ?? false){
                return new Meta (this);
            }else{
                return new Meta(
                    this, new LogTrace(scope, this.trace, reason)
                );
            }
        }else{
            return new Meta(new LogTrace(scope, reason), Unwind(owner));
        }
    }

    status[] Unwind(in status owner, int i=0){
        var n = i + 1;
        var @out = prev?.value.meta.Unwind(prev.value, n) ?? new status[n];
        @out[@out.Length-n] = owner;
        return @out;
    }

}}

#endif  // !AL_OPTIMIZE
