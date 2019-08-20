using NUnit.Framework;
using System;
using Active.Core;
using Active.Core.Details;

public class LogPerf : CoreTest {

    LogString LOG = null;

    void Setup(){ StatusFormat.UseASCII(); status.log = false; }

    [Test] public void Bench_1_1_RawStatus_75(){
        Setup();
        for(int i = 0; i < HighTestIters; i++){
             var s = done;
         }
    }

    [Test] public void Bench_2_1_PForm_Ternary_CallerInf_200(){
        Setup();
        int k = 5;
        for(int i = 0; i < HighTestIters; i++){
            var s = done.Via(status.log ? (LOG && $"Re:{k}") : null);
        }
    }

    [Test] public void Bench_2_2_PForm_Manual_Check_240(){
        Setup();
        int k = 5;
        for(int i = 0; i < HighTestIters; i++){
            var s = status.log ? done : done.Via(LOG && $"Re:{k}");
        }
    }

    [Test] public void Bench_2_3_PForm_Binary_CallerInf_235(){
        Setup();
        int k = 5;
        for(int i = 0; i < HighTestIters; i++){
            var s = done.Via(LOG && $"Re:{k}");
        }
    }

  #if UNITY_2018_1_OR_NEWER
    [Test] public void Bench_4_Vectors_280(){
        Setup();
        var u = new UnityEngine.Vector3(1, 2, 3);
        var v = new UnityEngine.Vector3(7.2f, 24f, 0.3f);
        for(int i = 0; i < HighTestIters; i++){
            var s = u + v;
        }
    }
  #endif

}
