using System; using InvOp = System.InvalidOperationException;
using self = Active.Core.Details.RoR;

namespace Active.Core.Details{
public static class RoR{

    public static bool enabled = true;
    internal static object owner;
    internal static int frame;
    public static int leniency = 1;

    // Keep leniency within 1~10
    public static void Enter(object @in, int frame, int leniency){
        if(!enabled) return;
        RoR.leniency = leniency;
        if(owner != null) throw
            new InvOp($"Exit {owner} before entering {@in}");
        if(@in == null) throw
            new InvOp($"Context is null");
        self.owner = @in;
        self.frame = frame;
    }

    public static void Exit(object @ex, ref int frame){
        if(!enabled) return;
        if(owner != @ex) throw
            new InvOp($"Expected {self.owner}, found {owner}");
        if(frame != self.frame) throw
            new InvOp($"!frame count({frame}!={self.frame})");
        owner = null;
        ++frame;
    }

    public static void OnResume(ref int frame, Func<action> Reset){
        if(!enabled) return;
        if(self.owner == null) throw
            new InvOp("Enter a frame context (RoR)");
		if(self.frame > frame + leniency) Reset();
		frame = self.frame;
    }

}}
