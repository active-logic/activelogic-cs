// Doc/Reference/Decorators.md

using Active.Core.Details;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using static Active.Status;

namespace Active.Core{
[System.Serializable]
public class Wait : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
	int frame;
    public   float delay = 1f;
    internal float? stamp;

    public Wait(){}
    public Wait(float delay){ this.delay = delay; }

    public static implicit operator Wait(float delay)
    => new Wait(delay);

    public static implicit operator status(Wait self)
    => self[self.delay];

    public status this[float duration]{ get{
        Notices.OnEnter(ref frame, this);
        stamp = stamp ?? time;
        var elapsed = time - stamp.Value;
        return elapsed > delay
               ? done(log && $"[0.0]")
               : cont(log && $"[{delay - elapsed:0.0}]");
    }}

    override public action Reset(){ stamp = null; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
	public status Wait(float delay, [Tag] int key = -1)
	=> store.Decorator<Wait>(key, Active.Core.Wait.id)[delay];
}
#endif

}  // Active.Core
