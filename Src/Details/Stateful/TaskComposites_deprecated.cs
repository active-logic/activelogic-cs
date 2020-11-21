using System;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Active.Core.Details;

namespace Active.Core{
partial class Task{

    #if !AL_BEST_PERF
    #if AL_THREAD_SAFE

      [Obsolete("Use Seq() instead", false)]
      public Iterator Sequence([Tag] int key = -1)
      => store.Composite<Sequence>(key).iterator;

      [Obsolete("Use Sel() instead", false)]
      public Iterator Selector([Tag] int key = -1)
      => store.Composite<Selector>(key).iterator;

    #else  // !AL_THREAD_SAFE

      protected Iterator and => iterator;
      protected Iterator or  => iterator;

      protected status end
      { get{ var i = iterator; iterator = null; return i.end; }}

      protected status loop
      { get{ var i = iterator; iterator = null; return i.loop; }}

      [Obsolete("Use Seq() instead", false)]
      public Iterator Sequence([Tag] int key = -1)
      => iterator = store.Composite<Sequence>(key).iterator;

      [Obsolete("Use Sel() instead", false)]
      public Iterator Selector([Tag] int key = -1)
      => iterator = store.Composite<Selector>(key).iterator;

    #endif  // end !AL_THREAD_SAFE
    #endif  // end !AL_BEST_PERF

}}
