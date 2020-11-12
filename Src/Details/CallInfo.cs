// Internal
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

namespace Active.Core.Details{
public readonly struct CallInfo{

    internal readonly string path;
    internal readonly string member;
    internal readonly int line;

    CallInfo(string p, string m, int l){
        path   = p;
        member = m;
        line   = l;
    }

    public static CallInfo none => new CallInfo(null, null, 0);

    public bool Validate()
    => !(path == null && member == null && line==0);

    public static implicit operator CallInfo(
                           (string path, string member, int line) i)
    => new CallInfo(i.path, i.member, i.line);

    public static status operator / (status s, CallInfo i){
        return status.log && i.Validate()
            ? Logging.Status(s, null, i.path, i.member, i.line)
            : s;
    }

}}

#endif
