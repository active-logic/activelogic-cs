// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Status;
using InvOp = System.InvalidOperationException;
using Tag   = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
public class Init : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    internal static Init[] stack = new Init[128];
    internal static int stackIndex = -1;
    public bool passing = true;
    int frame;

    // NOTE: unlike other decorators, it appears that the entry
    // point for Init is the `pass` getter. Notice how this[]
    // returns Gate vs Gate? (check cPatrol.cs for an example)
    public Init pass{ get{
        Notices.OnEnter(ref frame, this);
        Init.Push(this);
        return passing ? this : null;
    }}

    public Gate this[object x]{ get{
        passing = false;
        return new Gate();
    }}

    override public action Reset(){ passing = true; return @void(); }

    static void Push (Init x) => stack[++stackIndex] = x;

    static Init Pop(){
        var @out = stack[stackIndex];
        stack[stackIndex--] = null;
        return @out;
    }

    public readonly struct Gate{

        public static status operator % (Gate? self, status s)
        { Init.Pop().passing = !s.running; return s; }

        public static status operator + (Gate? self, status s)
        { Init.Pop().passing = s.complete; return s; }

        public static status operator - (Gate? self, status s)
        { Init.Pop().passing = s.failing; return s; }

    }  // Gate class

}

#if !AL_BEST_PERF
partial class Task{
    public Init With([Tag] int key = -1)
    => store.Decorator<Init>(key, Active.Core.Init.id).pass;
}
#endif

}  // end-Active.Core
