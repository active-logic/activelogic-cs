namespace Active.Core.Details{
public class SelIterator : Iterator{

    internal SelIterator (Selector c) : base(c){}

    public SelIterator Reset(){ i = 0; return this; }

    override public status this[in status x]  // Faster than forwarding to κ[x]
    { get{ if(x.failing) { κ.index++; return (+x).due; } return x; }}

    override public status end => status.@unchecked(-2).Via();

    override public status loop
    { get{ κ.index = -1; return status.fail(); }}

}}
