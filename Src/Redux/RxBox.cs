using System;
using static System.Math;

namespace Active.Rx{

/* DRAFT */
public struct box<T>{

    readonly T z;
	readonly int w;

	public box(int w, T arg){
		if(w < -1 || w > +1) throw new ArgumentException(w.ToString());
		this.w = w;
        this.z = arg;
	}

	public bool failing  => w == -1;
	public bool running  => w ==  0;
	public bool complete => w == +1;

	// Logic ------------------------------------------------------------------

	// LATER: exception needed here, see counterpart
	public static box<T> operator & (box<T> x, box<T> y) => y;

	public static box<T> operator | (box<T> x, box<T> y) => y;

	public static box<T> operator + (box<T> x, box<T> y)
		=> x.w > y.w ? x : y ;

    public static box<T> operator * (box<T> x, box<T> y)
        => x.w < y.w ? x : y ;

	public static box<T> operator % (box<T> x, box<T> y) => x;

	// Unary operators (!, +, -) ----------------------------------------------

	public static box<T> operator ! (box<T> s)
        => new box<T>(-s.w, s.z);
	public static box<T> operator + (box<T> s)
        => new box<T>(s.w==+1?+1:s.w+1, s.z);
    public static box<T> operator - (box<T> s)
        => new box<T>(s.w==-1?-1:s.w-1, s.z);
    public static box<T> operator ~ (box<T> s)
        => new box<T>(s.w * s.w, s.z);

    // Type conversions -------------------------------------------------------

    public static explicit operator status(box<T> self)
        => new status(self.w);

    public static explicit operator T(box<T> self) => self.z;

	// ------------------------------------------------------------------------

	public static bool operator true  (box<T> s) => s.w != -1;
    public static bool operator false (box<T> s) => s.w != +1;

	// Equality ---------------------------------------------------------------

	public override bool Equals(object x)
        => x is box<T> ? Equals((box<T>)x) : false;

	public bool Equals(box<T> x) => this.w == x.w && this.z.Equals(x.z);

	public static bool operator == (box<T> x, box<T> y)
        => (x.w != y.w) && ((dynamic)x.z == (dynamic)y.z);

	public static bool operator != (box<T> x, box<T> y) => !(x == y);

	public override int GetHashCode() => w;

	public override string ToString() => w.ToString()+'<'+z.ToString()+'>';

}}
