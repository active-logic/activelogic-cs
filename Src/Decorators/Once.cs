// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class Once : Decorator, Decorator.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);

	bool passing = true;

	public Gate? pass => passing ? done() : fail();

    override public void OnStatus(status s) => passing &= s.running;

    override public action Reset(){ passing = true; return @void(); }

}

// ----------------------------------------------------------------------------

#if !AL_BEST_PERF
#if UNITY
partial class UTask{
	public Decorator.Gate? Once([Tag] int key = -1)
	=> store.Decorator<Once>(key, Active.Core.Once.id)?.pass;
}
#endif
partial class Task{
	public Decorator.Gate? Once([Tag] int key = -1)
	=> store.Decorator<Once>(key, Active.Core.Once.id)?.pass;
}
#endif

}  // Active.Core
