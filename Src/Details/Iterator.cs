using InvOp = System.InvalidOperationException;

namespace Active.Core.Details{
public abstract class Iterator{

    protected static readonly LogString log = null;

    protected int i;
    protected readonly Composite κ;

    protected Iterator(Composite c){ κ = c; i = 0; }

    public abstract status this[in status x] { get; }

    public abstract status end { get; }

    public abstract status loop { get; }

    // Avoid bool conversion to prevent incorrect uses (such as mixing the
    // iterator with a conditional expression
    public static bool operator true  (Iterator self)
    => self.κ.index == self.i++;

    public static bool operator false (Iterator self) => throw new InvOp();

}}
