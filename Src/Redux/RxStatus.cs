#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

//using System;
using static System.Math;

namespace Active.Rx{
public readonly partial struct status{

	public static readonly status done = new status(+1),
								  fail = new status(-1),
								  cont = new status( 0);

	public bool failing  => ω == -1;
	public bool running  => ω ==  0;
	public bool complete => ω == +1;

	public status Map(in status failTo, in status contTo, in status doneTo){
		switch(ω){
			case -1: return new status(failTo.ω);
			case  0: return new status(contTo.ω);
			case +1: return new status(doneTo.ω);
			default: throw new System.ArgumentException();
		}
	}

	// @provisional
	public box<T> With<T>(T arg) => new box<T>(this.ω, arg);

	// public status Via(LogString info) => this;
	// public status Via() => this;

	// Logic ------------------------------------------------------------------

	// NOTE: while supported, the conditional logical operators `&&` and `||`
	// are not explicitly implemented.

	public static status operator + (status x, status y)
	=> new status(Max(x.ω, y.ω));

	public static status operator * (status x, status y)
	=> new status(Min(x.ω, y.ω));

	public static status operator % (status x, status y) => new status(x.ω);

	public static status operator ! (status s) => new status(-s.ω);

	public static status operator ~ (status s) => new status(s.ω * s.ω);

	public static status operator + (status s) => new status(s.ω==+1?+1:s.ω+1);
    public static status operator - (status s) => new status(s.ω==-1?-1:s.ω-1);

	public static status operator ++ (status s) => +s;
	public static status operator -- (status s) => -s;

	public static implicit operator status(bool s) => new status(s ? +1 : -1);

}}
