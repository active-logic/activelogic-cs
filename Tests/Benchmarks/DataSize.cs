using NUnit.Framework;
using System.Runtime.InteropServices;
using Active.Core;

public class DataSize : TestBase {  // Note: sizes in bytes

    [Test] public void RxStatusSize()
    => o (Marshal.SizeOf(new Active.Rx.status(0)), 4);

  #if AL_OPTIMIZE
    [Test] public void StatusSize() => o (Marshal.SizeOf(status.cont()), 4);
  #else

  #if UNITY
    [Test] public void StatusSize() => o (Marshal.SizeOf(status.cont()), 24);
  #endif

  #endif  // AL_OPTIMIZE

}
