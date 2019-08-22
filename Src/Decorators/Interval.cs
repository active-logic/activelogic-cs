// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
[System.Serializable]
public class Interval : Waiter, Waiter.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);

    public float offset = 0, period = 1;
	float stamp = System.Single.MinValue;

	public Interval(){}

    public Interval(bool fireOnStart){ if(!fireOnStart) stamp = 0; }

	public Interval(float period, float offset=0f, bool fireOnStart=true){
        this.period = period;
        this.offset = offset;
        if(!fireOnStart) stamp = 0;
    }

    public float due => stamp + period + offset;

	public static implicit operator Interval(float val) => new Interval(val);

	public Gate? pass => this[period];

	public Gate? this[float s]{get{
		if(time >= stamp + s + offset){
			stamp = (stamp == System.Single.MinValue) ? time : stamp + s;
			return done(log && $"[@{stamp:0.0}]");
		}else return cont(log && $"[{stamp + s - time:0.0}/{s:0.0}]");
	}}

	override public action Reset(){ stamp = 0; return @void(); }

	override public void OnStatus(status s){}

}

#if !AL_BEST_PERF
partial class Task{
	public Waiter.Gate? Every(float delay, float offset = 0f,
                                                            [Tag] int key = -1)
    => store.Decorator<Interval>(key, Active.Core.Interval.id)[delay];
}
#endif

}
