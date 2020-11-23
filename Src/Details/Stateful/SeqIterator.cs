namespace Active.Core.Details{
public class SeqIterator : Iterator{

    internal SeqIterator (Sequence c) : base(c){}

    public SeqIterator Reset(){ i = 0; return this; }

    // Faster than forwarding to κ[x]
    override public status this[in status x]{ get{
        if(x.complete){
            κ.index++;
            return -x;
        }
        return x;
    }}

    override public status end => status.@unchecked(+2).Via();

    override public status repeat{ get{
        κ.index = -1;
        return status.@unchecked(+2).Via();
    }}

    override public status loop{ get{
        κ.index = -1; return status.done();
    }}

}}
