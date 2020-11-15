// Doc/Reference/Decorators.md
#if UNITY_2018_1_OR_NEWER

using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
[System.Serializable]
public class Delay: Waiter, Waiter.OptionalArguments{

	static int uid; internal static int id => uid = ID(uid);
	public float duration = 1f;
	internal float elapsed = 0;
	int frame = 0;

	public Delay(){}
	public Delay(float duration){ this.duration = duration; }

	public static implicit operator Delay(float val)
	=> new Delay(val);

	public Gate? pass => this[duration];

	override public void OnStatus(status s){
		if(!s.running) elapsed = 0;
	}

	public Gate? this[float duration]{ get{
		Notices.OnEnter(ref frame, this);
	    return ((elapsed += UnityEngine.Time.deltaTime) >= duration)
       	 	   ? done(log && $"[0.0]")
	   		   : cont(log && $"[{duration-elapsed:0.0}]");
	}}

	override public action Reset(){ elapsed = 0; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
	public Waiter.Gate? After(float delay, [Tag] int key = -1)
	=> store.Decorator<Delay>(key, Active.Core.Delay.id)[delay];
}
#endif

}  // Active.Core

#endif  // UNITY REQUIRED
