// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
[System.Serializable]
public class Wait : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);

	public float duration = 1f;
	float stamp;

	public Wait(){}
	public Wait(float s) => duration = s;

	public static implicit operator Wait(float val) => new Wait(val);

	public status pass => this[duration];

	public status this[float s]{ get{
		var t = time; if(stamp == 0) stamp = t; var remaining = s + stamp - t;
		return (remaining > 0)
			? status._cont.ViaDecorator(this, log && $"[{remaining:0.0}]")
			: status._done.ViaDecorator(this, log && $"[timed out ({s:0.0}s)");
	}}

    override public action Reset(){ stamp = 0; return @void(); }

}

// ----------------------------------------------------------------------------

#if !AL_BEST_PERF
partial class Task{
	public status Wait(float duration, [Tag] int key = -1)
    => store.Decorator<Wait>(key, Active.Core.Wait.id)[duration];
}
#endif

}  // Active.Core
