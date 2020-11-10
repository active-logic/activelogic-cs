namespace Active.Core.Details{
public readonly struct Reckoning{

    internal readonly ReCon.Context context;

    public Reckoning(bool arg, ReCon stack)
    => context = arg ? stack.Enter(forward: true) : null;

    public status this[status s]{get{
        context?.Exit();
        return s;
    }}

}}
