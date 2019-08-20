#if UNITY_2018_1_OR_NEWER

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using F = Active.Core.Details.StatusFormat;
using GO = UnityEngine.GameObject;
using static Active.Core.status;

public class FT_DecoratorsInTask : CoreTest {

    // TODO try moving this to CoreTest
    protected static readonly LogString log = null;

    Task task;

    [SetUp] public void Setup(){
        task = new Task();
        StatusFormat.UseASCII();  // TODO move to CoreTest
    }

    [Test] public void StepCooldownInTask() => task.Step();

    class Task : Active.Core.Task{

      #if AL_BEST_PERF
        public Cooldown overheat = 0.1f;
        override protected void Start() => Register(overheat);
        override public status  Step()  => overheat.pass?[ Fire() ];
      #else
        override public status Step() => Cooldown(0.1f)?[ Fire() ];
      #endif

        status Fire() => done();

    }

}

#endif
