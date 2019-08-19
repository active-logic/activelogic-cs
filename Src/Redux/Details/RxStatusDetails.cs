#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using ArgEx = System.ArgumentException;

namespace Active.Rx{
partial struct status{

    readonly int ω;

    internal status(int ω){
	    #if !AL_OPTIMIZE
		if(ω < -1 || ω > +1) throw new ArgEx(ω.ToString());
	    #endif
		this.ω = ω;
	}

	internal status(int ω, bool @unchecked){
		#if !AL_OPTIMIZE
		if(!@unchecked) throw new ArgEx("'Unchecked' must be true");
		#endif
		this.ω = ω;
	}

    public static status operator & (status x, status y){
      #if !AL_OPTIMIZE
        return (x.ω < +1) ? throw new ArgEx() : y;
      #else
        return y;
      #endif
    }

    public static status operator | (status x, status y){
      #if !AL_OPTIMIZE
        return (x.ω > -1) ? throw new ArgEx() : y;
      #else
        return y;
      #endif
    }

    public static bool operator true  (status s) => s.ω != -1;
    public static bool operator false (status s) => s.ω != +1;

    public override bool Equals(object x)
    => x is status ? Equals((status)x) : false;

    public bool Equals(status x) => this == x;

    public static bool operator == (status x, status y) => x.ω == y.ω;
    public static bool operator != (status x, status y) => !(x == y);

    public override int GetHashCode() => ω;
	public override string ToString() => ω.ToString();

}}
