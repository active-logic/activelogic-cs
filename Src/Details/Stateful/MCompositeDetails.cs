using System;
using System.Collections.Generic;
using static Active.Raw;

namespace Active.Core{
partial class MComposite{

    status key = cont;
    IEnumerable<Func<status>> tasks;
    internal IEnumerator<Func<status>> ι;
    Func<status> task, flow;

    internal MComposite() => flow = Progressive;

    MComposite(in status key, IEnumerable<Func<status>> tasks){
        this.current = this.key = key;
        this.tasks = tasks;
        flow = Progressive;
    }

    status Concurrent(){
        foreach(var task in tasks){
            current = task();
            if(current == !key) break;
        } return current;
    }

    status Ordered(){
        ι = ι ?? tasks.GetEnumerator();
        if(task == null){
            if(ι.MoveNext()) task = ι.Current;
            else return current;
        }
        current = task();
        if(current == key){
            if(ι.MoveNext()) { task = ι.Current; return cont; }
            else             { task = null;                   }
        }
        return current;
    }

    status Progressive(){
        foreach(var task in tasks){
            current = task();
            if(current != key) break;
        }
        return current;
    }

}}
