using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core.Details{
public abstract class Composite : Resettable{

    int i, frame;

    internal int index{
        get{
            Notices.OnEnter(ref frame, this);
            return i;
        }
        set => i = value;
    }

    public static implicit operator int(Composite self) => self.index;

    public action Reset(){ index = 0; return status.@void(); }

    // A complete sequence succeeds, whereas a complete selector fails;
    public abstract status end{ get; }

    // All looping composites, however, return a value of 'running'
    public status loop{ get{ index = 0; return status.cont(); }}

}}
