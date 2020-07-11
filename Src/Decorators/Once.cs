// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
public class Once : Conditional, Conditional.OptionalArguments{

    static int uid; internal static int id => uid = ID(uid);
	bool passing = true;
    int frame = 0;

	public Gate? pass{ get{
        RoR.OnResume(ref frame, Reset); // TODO tests
        return passing ? done() : fail();
    }}

    override public void OnStatus(status s) => passing &= s.running;

    override public action Reset(){ passing = true; return @void(); }

}

#if !AL_BEST_PERF
partial class UTask{
}partial class Task{
	public Conditional.Gate? Once([Tag] int key = -1)
	=> store.Decorator<Once>(key, Active.Core.Once.id)?.pass;
}
#endif

}
