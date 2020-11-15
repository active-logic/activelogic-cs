// Doc/Reference/Decorators.md
using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
[System.Serializable]
public class Timeout : Conditional, Conditional.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);
	public float duration = 1;
	float stamp;
    int frame;

	bool enabled => stamp != -1;

	public Timeout() => stamp = time;

	public Timeout(float duration)
    { stamp = time; this.duration = duration; }

    public float due => stamp + duration;

	public static implicit operator Timeout(float val)
    => new Timeout(val);

	public Gate? pass => this[duration];

	public Gate? this[float s]{ get{
        Notices.OnEnter(ref frame, this);
	    return enabled ? time < stamp + s
                           ? done(log && $"[{s + stamp - time:0.#}]")
			               : fail(log && $"timed out ({s}s)")
			           : done(log && "idle");
    }}

	override public void OnStatus(status s) => OnStatus(s.running);

    void OnStatus(bool running)
	=> stamp = enabled ^ running ? (enabled ? -1 : time) : stamp;

    override public action Reset(){ stamp = -1; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
	public Conditional.Gate? Timeout(float duration,
                                     [Tag] int key = -1)
    => store.Decorator<Timeout>(
                      key, Active.Core.Timeout.id)[duration];
}
#endif

}
