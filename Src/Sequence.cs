// Doc/Reference/Ordered Composites.md
using Active.Core.Details;

namespace Active.Core{
public class Sequence : Composite{

    SeqIterator it;

    public SeqIterator iterator
    => (it ?? (it = new SeqIterator(this))).Reset();

    public status this[in status x]
    { get{ if(x.complete){ index++; return (-x).undue; } return x; }}

    override public status end => status.done();

}}
