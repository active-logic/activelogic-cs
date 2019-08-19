// Doc/Reference/Mutable Composite.md
using System;
using System.Collections.Generic;
using static Active.Core.status;
using Ex = System.Exception;

namespace Active.Core{
public partial class MComposite{

    public bool loop = true;

    public status current { get; private set; }

    public bool dither{ get => flow != Ordered; set => throw new Ex("Unimp."); }

    public bool isSequence { set => key = _done; get => key.complete; }
    public bool isSelector { set => key = _fail; get => key.failing;  }

    public bool concurrent { set => flow=Concurrent;  get => flow==Concurrent; }
    public bool ordered    { set => flow=Ordered;     get => flow==Ordered;    }
    public bool progressive{ set => flow=Progressive; get => flow==Progressive;}

    public static MComposite Selector(params Func<status>[] args)
    => new MComposite(_fail, new List<Func<status>>(args));

    public static MComposite Selector(IEnumerable<Func<status>> tasks)
    => new MComposite(_fail, tasks);

    public static MComposite Sequence(params Func<status>[] args)
    => new MComposite(_done, new List<Func<status>>(args));

    public static MComposite Sequence(IEnumerable<Func<status>> tasks)
    => new MComposite(_done, tasks);

    public static implicit operator status (MComposite self) => self.Step();
    public static implicit operator Func<status>(MComposite self) => self.Step;

    public status Step() => flow();

    public action Reset(){
        if(flow==Ordered) ι.Reset();
        return @void();
    }

}}
