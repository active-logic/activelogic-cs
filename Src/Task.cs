// Doc/Reference/Task.md
#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using System.Collections.Generic;  // used by Register
using Active.Core.Details;
//using static Active.Core.Decorator;
using static Active.Core.status;

namespace Active.Core{
public abstract partial class Task : Gig, Context {

    public void Register(Resettable rsc)
    => (_context ?? (_context = new List<Resettable>())).Add(rsc);

    public virtual action Reset(){
        if(_context != null) foreach(var k in _context) k.Reset();
      #if !AL_BEST_PERF
        _store?.Reset();
      #endif
        return @void();
    }

    public virtual action Release(){
        _context = null;
      #if !AL_BEST_PERF
        _store = null;
      #endif
        return Reset();
    }

    public static implicit operator status       (Task self) => self.Step();
    public static implicit operator Func<status> (Task self) => self.Step;

}}
