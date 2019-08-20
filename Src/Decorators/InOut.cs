// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class InOut : Decorator{

    static int uid; internal static int id => uid = ID(uid);

    bool passing;

    public Gate? this[bool @in, bool @out]
    => (passing = passing ? !@out : @in) ? done() : fail();

    override public void OnStatus(status s) => OnStatus(s.running);

    void OnStatus(bool running) => passing = running ? passing : false;

    override public action Reset(){ passing = false; return @void(); }

}

// ----------------------------------------------------------------------------

#if !AL_BEST_PERF
partial class Task{
    public Decorator.Gate? InOut(bool @in, bool @out, [Tag] int key = -1)
    => store.Decorator<InOut>(key, Active.Core.InOut.id)[@in, @out];
}
#endif

}  // Active.Core
