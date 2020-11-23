// Doc/Reference/Mutable Composite.md
using System;
using System.Collections.Generic;
using Ex = System.Exception;
using static Active.Raw;
using Active.Core.Details;

namespace Active.Core{
public partial class MComposite : Resettable{

    public bool loop = true;
    public bool resetOnResume = true;
    int frame;

    public status current { get; private set; }

    public bool dither{ get => flow != Ordered; set => throw new Ex("Unimp."); }

    public bool isSequence { set => key = done; get => key.complete; }
    public bool isSelector { set => key = fail; get => key.failing;  }

    public bool concurrent { set => flow=Concurrent;  get => flow==Concurrent; }
    public bool ordered    { set => flow=Ordered;     get => flow==Ordered;    }
    public bool progressive{ set => flow=Progressive; get => flow==Progressive;}

    public static MComposite Selector(params Func<status>[] args)
    => new MComposite(fail, new List<Func<status>>(args));

    public static MComposite Selector(IEnumerable<Func<status>> tasks)
    => new MComposite(fail, tasks);

    public static MComposite Sequence(params Func<status>[] args)
    => new MComposite(done, new List<Func<status>>(args));

    public static MComposite Sequence(IEnumerable<Func<status>> tasks)
    => new MComposite(done, tasks);

    public static implicit operator status (MComposite self) => self.Step();
    public static implicit operator Func<status>(MComposite self) => self.Step;

    // TODO RoR untested for mutable composites
    public status Step(){
        Notices.OnEnter(ref frame, this);
        return flow();
    }

    public action Reset(){
        if(flow==Ordered) ι.Reset();
        return @void;
    }

}}
