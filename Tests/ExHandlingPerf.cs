using NUnit.Framework;
using System;

public class ExHandlingPerf : TestBase{

    [Test] public void Benchmark(){
        for (int i=0; i < SlowTestIters; i++){
            try{ throw new SystemException(); }
            catch (SystemException){}
        }
    }

}
