// Doc/Reference/Decorator.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public abstract partial class AbstractDecorator : IDecorator,
                                                  Resettable{

    protected virtual float time{ get{
      #if UNITY_2018_1_OR_NEWER
        return UnityEngine.Time.time;
      #else
        return System.DateTime.Now.Millisecond/1000f;
      #endif
    }}

    protected static int ID(int id) => id == 0 ? ++MaxId : id;

}}
