#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core;

namespace Active{
public static class Raw{

    public static status  done    = status._done;
    public static status  fail    = status._fail;
    public static status  cont    = status._cont;
    public static action  @void   = action._done;
    public static failure @false  = failure._fail;
    public static loop    forever = loop._cont;

    public static pending   pending_cont   = pending._cont;
    public static pending   pending_done   = pending._done;
    public static impending impending_cont = impending._cont;
    public static impending impending_fail = impending._fail;

    public static status Eval(status s) => s;

    #if !AL_OPTIMIZE
    public static status undef()         => status._fail;
    public static status undef(status s) => s;
    #endif

    public static action  Do   (object arg) => @void;
    public static loop    Cont (object arg) => forever;
    public static failure Fail (object arg) => @false;

}}
