namespace Active.Core.Details{
public abstract class Iterator{

    internal static readonly LogString log = null;

    protected int i;
    protected readonly Composite κ;

    protected Iterator(Composite c){ κ = c; i = 0; }

    public abstract status this[in status x] { get; }

    public abstract status end    { get; }
    public abstract status loop   { get; }
    public abstract status repeat { get; }

    public static bool operator true  (Iterator self)
    => self.κ.index == self.i++;

    public static bool operator false (Iterator self)
    => throw new System.InvalidOperationException();

}}
