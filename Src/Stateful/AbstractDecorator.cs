// Doc/Reference/Decorator.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core.Details;

namespace Active.Core{
public abstract partial class AbstractDecorator : IDecorator,
                                                  Resettable{

    internal float time => SimTime.time;

    protected static int ID(int id) => id == 0 ? ++MaxId : id;

}}
