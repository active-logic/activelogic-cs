using Nul = System.NullReferenceException;
using System.Collections.Generic;

namespace Active.Core.Details{
public class ReCon : Stack<ReCon.Context>{

    #if(!AL_THREAD_SAFE)
    static ReCon _instance;
    public static ReCon instance
    => _instance ?? (_instance = new ReCon());
    #endif

    public ReCon.Context Enter(){
        var cx = new ReCon.Context(this);
        Push(cx);
        return cx;
    }

    public void Add(Resettable r){
        if(Count > 0) Peek().Traverse(r);
    }

    // ==============================================================

    public class Context : List<Resettable>{

        ReCon owner;
        public bool forward;

        public Context(ReCon x)
        => owner = x != null ? x : throw new Nul("ReCon stack is null");

        public void Traverse(Resettable x){
            if(forward) x.Reset(); else Add(x);
        }

        public void Exit(bool reset = true){
            if(!forward && reset) foreach(var e in this) e.Reset();
            if(owner.Peek() != this){
                throw new System.Exception(
                         "Exiting context not topmost in ReCon stack");
            }
            owner.Pop();
        }

        public status this[status s]{ get{
            Exit(reset: !s.running);
            return s;
        }}

    }

}}
