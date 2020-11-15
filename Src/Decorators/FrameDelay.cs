// Doc/Reference/Decorators.md
#if UNITY_2018_1_OR_NEWER

using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
[System.Serializable]
public class FrameDelay: Waiter, Waiter.OptionalArguments{

	static int uid; internal static int id => uid = ID(uid);

	public int duration = 1;
	int elapsed = 0;
	int frame = 0;

	public FrameDelay(){}
	public FrameDelay(int duration){ this.duration = duration; }

	public static implicit operator FrameDelay(int val)
    => new FrameDelay(val);

	public Gate? pass => this[duration];

	override public void OnStatus(status s){
        if(!s.running) elapsed = 0;
    }

	public Gate? this[int duration]{ get{
		Notices.OnEnter(ref frame, this);
		return ( ++elapsed > duration) ? done(log && $"[0]")
       		   : cont(log && $"[{duration-elapsed}]");
    }}

	override public action Reset(){ elapsed = 0; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
	public Waiter.Gate? AfterFrames(int frames, [Tag] int key = -1)
	=> store.Decorator<FrameDelay>(key,
                                   Active.Core.FrameDelay.id)[frames];
}
#endif

}  // Active.Core

#endif  // UNITY REQUIRED
