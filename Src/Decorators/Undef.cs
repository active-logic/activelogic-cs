// Doc/Reference/Decorators.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using System;
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class Undef : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    protected Random _random;
    float stamp;
    status value;

	public Undef(){}

    protected Random random => _random ?? (_random = new Random());

    public status this[float s]{ get{
		var t = time; if(stamp == 0) stamp = t; var remaining = s + stamp - t;
		return (
            (remaining > 0) ? value
            : (value = status.@unchecked(random.Next(-1, 2)))
        ).ViaDecorator(this, log && "undef");
	}}

    override public action Reset(){ stamp = 0; return @void(); }

}

// ----------------------------------------------------------------------------

#if !AL_BEST_PERF
partial class Task{
	public status undef(float duration = 1, [Tag] int key = -1)
    => store.Decorator<Undef>(key, Active.Core.Undef.id)[duration];
}
#endif  // !AL_BEST_PERF
}  // Active.Core

#endif  // !AL_OPTIMIZE
