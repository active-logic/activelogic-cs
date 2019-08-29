// Doc/Reference/Decorators.md
using System;
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
[System.Serializable]
public class Interval : Waiter, Waiter.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);

    public bool catchup = false;
    public float offset = 0, period = 1;
	internal float stamp = System.Single.MinValue;

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
            if(stamp == System.Single.MinValue || s == 0) stamp = time;
            else if(catchup){
                stamp += s;
            }else{
                int n = (int)Math.Floor((time - stamp) / s) + 1;
                stamp += s * n;
            }
			return done(log && $"[○{stamp:0.00}]");
		}else return cont(log && $"[{time:0.00}▻{stamp+s+offset:0.00}]");
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
