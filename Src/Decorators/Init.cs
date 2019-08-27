// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Core.status;
using InvOp = System.InvalidOperationException;
using Tag   = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class Init : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);

    static Init current;
    public bool passing = true;

    public Init pass{ get{
        if(current != null)
        { current = null; throw new InvOp("Unclosed init detected"); }
        current = this; return passing ? this : null;
    }}

    public Gate this[object x]
    { get{ passing = false; return new Gate(this); }}

    override public action Reset(){ passing = true; return @void(); }

    new public readonly struct Gate{

        readonly Init owner;

        public Gate(Init self) => owner = self;

        static Init Owner(Gate? self) => self?.owner ?? Init.current;

        public static status operator % (Gate? self, status s)
        { Owner(self).passing = !s.running; Init.current = null; return s; }

        public static status operator + (Gate? self, status s)
        { Owner(self).passing = s.complete; Init.current = null; return s; }

        public static status operator - (Gate? self, status s)
        { Owner(self).passing = s.failing; Init.current = null; return s; }

    }  // Gate class

}

#if !AL_BEST_PERF
partial class Task{
    public Init With([Tag] int key = -1)
    => store.Decorator<Init>(key, Active.Core.Init.id).pass;
}
#endif

}  // end-Active.Core
