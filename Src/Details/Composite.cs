//using UnityEngine;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core.Details{
public abstract class Composite{

    internal int index = 0;

    public static implicit operator int(Composite self) => self.index;

    public void Reset() => index = 0;

    // A complete sequence succeeds, whereas a complete selector fails;
    public abstract status end{ get; }

    // All looping composites, however, return a value of 'running'
    public status loop{ get{ index = 0; return status.cont(); }}

}}

namespace Active.Core{

partial class Task{

    #if !AL_BEST_PERF
    #if AL_THREAD_SAFE

      protected Iterator Sequence([Tag] int key = -1)
      => store.Composite<Sequence>(key).iterator;

      protected Iterator Selector([Tag] int key = -1)
      => store.Composite<Selector>(key).iterator;

    #else  // !AL_THREAD_SAFE

      protected Iterator and => iterator;
      protected Iterator or  => iterator;

      protected status end
      {get{ var i = iterator; iterator = null; return i.end; }}

      protected status loop
      { get{ var i = iterator; iterator = null; return i.loop; }}

      protected Iterator Sequence([Tag] int key = -1)
      => iterator = store.Composite<Sequence>(key).iterator;

      protected Iterator Selector([Tag] int key = -1)
      => iterator = store.Composite<Selector>(key).iterator;

    #endif  // end !AL_THREAD_SAFE
    #endif  // end !AL_BEST_PERF

}

}  // Active.Core
