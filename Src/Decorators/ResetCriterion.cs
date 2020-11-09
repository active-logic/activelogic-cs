// Doc/Reference/Decorators.md
using static Active.Core.status;
using Active.Core.Details;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
public class ResetCriterion : AbstractDecorator{

    static int uid; internal static int id => uid = ID(uid);
    object hold;
    ReCon.Context context;

    public ResetCriterion Check(object arg, ReCon stack){
        if(!arg.Equals(hold)){
            context = stack.Enter();
            context.forward = true;
            hold = arg;
        }
        return this;
    }

    public status this[status s]{ get{
        context?.Exit();
        context = null;
        return s;
    }}

    override public action Reset(){ hold = null; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
    public ResetCriterion with(object arg, [Tag] int key = -1)
    => store.Decorator<ResetCriterion>(
                  key, Active.Core.ResetCriterion.id).Check(arg, rox);
}
#endif

}  // Active.Core
