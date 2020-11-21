#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Collections.Generic;
using Active.Core.Details;

namespace Active.Core{
partial class Task{

    List<Resettable> _context;

    #if !AL_THREAD_SAFE
    public bool staticRecon = false;
    internal static Iterator iterator;
    #endif

    ReCon _rox;

    #if !AL_BEST_PERF
    Store _store;
    Store store => _store ?? (_store = new HashStore());
    #endif

}}
