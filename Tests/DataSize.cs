using NUnit.Framework;
using System.Runtime.InteropServices;
using Active.Core;

public class DataSize : TestBase {                      // Note: sizes in bytes

    // TODO: enable
    // [Test] public void RxStatusSize()
    // => o (Marshal.SizeOf(new Active.Rx.status(0)), 4);

  #if UNITY_2021
    const int ED_STATUS_SIZE = 40;
  #else
    const int ED_STATUS_SIZE = 24;
  #endif

  #if AL_OPTIMIZE

    [Test] public void StatusSize()
    => o (Marshal.SizeOf(status.cont()), 4);

  #else

    //[Test] public void OrderedCompositeSize()
    //=> o (Marshal.SizeOf(new Active.Core.Sequence()), 4);

    //[Test] public void OrderedCompositeIteratorSize(){
    //    var s = new Sequence();
    //    o (Marshal.SizeOf(new Active.Core.SeqIterator(s)), 4);
    //}

    #if UNITY_EDITOR
    [Test] public void StatusSize(){
        //print(""+Marshal.SizeOf(status.cont()));
        o (Marshal.SizeOf(status.cont()), ED_STATUS_SIZE);
    }
    #endif

  #endif

}
