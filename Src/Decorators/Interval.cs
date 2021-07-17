// Doc/Reference/Decorators.md
using System;
using static System.Math;
using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
[System.Serializable]
public class Interval : Waiter, Waiter.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);

    public bool catchup = false;
    public float offset = 0, period = 1;
	internal float stamp; // Next time i'val will fire, minus offset

	public Interval() => stamp = time;

	public Interval(float period, float offset=0f){
        this.period = period;
        this.offset = offset;
        stamp = time;
    }

    public float due => stamp + offset;

	public static implicit operator Interval(float val)
    => new Interval(val);

	public Gate? pass => this[period];

	public Gate? this[float s]{ get{
        if(s == 0f){
            stamp = time;
            return done(log && "[○{bypass}]");
        }
		if(time >= stamp + offset){
            stamp += catchup ? s : EvalInc(s, offset, stamp, time);
			return done(log && $"[○{stamp:0.00}]");
		}else
          return cont(log && $"[{time:0.00}▻{stamp+s+offset:0.00}]");
	}}

	override public action Reset(){ stamp = time; return @void(); }

	override public void OnStatus(status s){}

    // period, offset, stamp, time
    static internal float EvalInc(float p, float o, float s, float t)
    => (float)(Floor((t - s - o) / p) + 1) * p;

}

partial class Task{
	public Waiter.Gate? Every(float delay, float offset = 0f,
                                           [Tag] int key = -1)
    => store.Decorator<Interval>(key, Active.Core.Interval.id)[delay];
}

}
