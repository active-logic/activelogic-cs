// Doc/Reference/Decorators.md
using static Active.Status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
public class Latch : Conditional{

    static int uid; internal static int id => uid = ID(uid);
	internal bool passing;
    int frame;

	public Gate? this[bool cond]{ get{
        Notices.OnEnter(ref frame, this);
        return (passing |= cond) ? done() : fail();
    }}

    override public void OnStatus(status s) => OnStatus(s.running);

	void OnStatus(bool running) => passing = running ? passing : false;

    override public action Reset(){ passing = false; return @void(); }

}

partial class UTask{
}partial class Task{
	public Conditional.Gate? Latch(bool @in, [Tag] int key = -1)
	=> store.Decorator<Latch>(key, Active.Core.Latch.id)[@in];
}

}
