// Doc/Reference/OrderedComposites.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using static Active.Status; using Active.Core.Details;
using T = Active.Core.Seq;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using S  = System.String;

namespace Active.Core{
public class Seq : Comp{

    public Seq() => key = state = done();

    public T Repeat([P] S p="", [M] S m="", [L] int l=-1)
    => (T)Step(repeat: true
        #if !AL_OPTIMIZE
        , (p, m, l)
        #endif
    );

    public T Once([P] S p="", [M] S m="", [L] int l=-1)
    => (T)Step(repeat: false
        #if !AL_OPTIMIZE
        , (p, m, l)
        #endif
    );

    override public Comp this[status s]{ get{
        state = s; if(s == key){ index++; } return this;
    }}

    public static T operator + (T x, Comp y) => x;
    public static T operator % (T x, Comp y) => x;

}}
