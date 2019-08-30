using System;
using ArgEx = System.ArgumentException;
using static System.Math;

namespace Active.Rx{

/* DRAFT */
public struct promise<T>{

    readonly T z;
	readonly bool denied;

    public static promise<X> breaking<X>()      => new promise<X>(true);
    public static promise<X> pending <X>()      => new promise<X>(false);
	public static promise<X> holding <X>(X arg) => new promise<X>(arg);

	public promise(T arg){
        if(arg == null) throw new ArgEx("null arg not allowed here");
		z = arg;
        denied = false;
	}

    public promise(bool denied){
        z = default(T);
        this.denied = denied;
    }

	public bool failing  => w == -1;
	public bool running  => w ==  0;
	public bool complete => w == +1;

    int w => z != null ? +1 : denied ? -1 : 0;

	// Logic ------------------------------------------------------------------

	// LATER: exception needed here, see counterpart
	public static promise<T> operator & (promise<T> x, promise<T> y) => y;

	public static promise<T> operator | (promise<T> x, promise<T> y) => y;

	public static promise<T> operator + (promise<T> x, promise<T> y)
		=> x.w > y.w ? x : y ;

    public static promise<T> operator * (promise<T> x, promise<T> y)
        => x.w < y.w ? x : y ;

	public static promise<T> operator % (promise<T> x, promise<T> y) => x;

	// Unary operators (!, +, -) ----------------------------------------------

    // Can't invert, promote or condone a promise. A held promise may default,
    // but can't arbitrarily instanciate T.

    public static promise<T> operator - (promise<T> s)
        => new promise<T>(denied: s.complete ? false : true);

    // Type conversions -------------------------------------------------------

    public static explicit operator status(promise<T> self)
        => new status(self.w);

    // Can box a promise (widening conversion)
    public static implicit operator box<T>(promise<T> self)
        => new box<T>(self.w, self.z);

    public static explicit operator T(promise<T> self) => self.z;

	// ------------------------------------------------------------------------

	public static bool operator true  (promise<T> s) => s.w != -1;
    public static bool operator false (promise<T> s) => s.w != +1;

	// Equality ---------------------------------------------------------------

	public override bool Equals(object x)
        => x is promise<T> ? Equals((promise<T>)x) : false;

	public bool Equals(promise<T> x) => this.w == x.w && this.z.Equals(x.z);

	public static bool operator == (promise<T> x, promise<T> y)
        => (x.w != y.w) && ((dynamic)x.z == (dynamic)y.z);

	public static bool operator != (promise<T> x, promise<T> y) => !(x == y);

	public override int GetHashCode() => w;

	public override string ToString() => w.ToString()+'<'+z.ToString()+'>';

}}
