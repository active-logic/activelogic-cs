// Doc/Reference/Ordered Composites.md
using Active.Core.Details;

namespace Active.Core{
public class Selector : Composite{

    SelIterator it;

    public SelIterator iterator
    => (it ?? (it = new SelIterator(this))).Reset();

    public status this[in status x]
    { get{ if(x.failing) { index++; return (+x).due; } return x; }}

    override public status end => status.fail();

}}
