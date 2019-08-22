// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class Latch : Conditional{

    static int uid; internal static int id => uid = ID(uid);

	public bool passing;

	public Gate? this[bool cond] => (passing |= cond) ? done() : fail();

    override public void OnStatus(status s) => OnStatus(s.running);

	void OnStatus(bool running) => passing = running ? passing : false;

    override public action Reset(){ passing = false; return @void(); }

}

#if !AL_BEST_PERF
partial class UTask{
}partial class Task{
	public Conditional.Gate? Latch(bool @in, [Tag] int key = -1)
	=> store.Decorator<Latch>(key, Active.Core.Latch.id)[@in];
}
#endif

}
