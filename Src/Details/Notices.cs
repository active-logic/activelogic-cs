namespace Active.Core.Details{
public static class Notices{

    public static void OnEnter(ref int frame, Resettable obj){
        #if !AL_THREAD_SAFE
        RoR.OnResume(ref frame, obj.Reset);
        ReCon.instance.Add(obj);
        #endif
    }

}}
