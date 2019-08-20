#if UNITY

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using F = Active.Core.Details.StatusFormat;
using GO = UnityEngine.GameObject;
using static Active.Core.status;

public class FT_DecoratorsInTask : CoreTest {

    // TODO try moving this to CoreTest
    protected static readonly LogString log = null;

    UTask task;

    [SetUp] public void Setup(){
        task = new GO().AddComponent<UTask>();
        StatusFormat.UseASCII();  // TODO move to CoreTest
    }

    [Test] public void StepCooldownInTask() => task.Step();

    class UTask : Active.Core.UTask{

      #if AL_BEST_PERF

        public Cooldown overheat = 0.1f;
        override protected void Start() => Register(overheat);
        override public status  Step()  => overheat.pass?[Fire()];

      #else

        override public status Step() => Cooldown(0.1f)?[Fire()];

      #endif

        status Fire() => done();

    }

}

#endif
