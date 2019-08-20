#if UNITY_2018_1_OR_NEWER
#if !AL_BEST_PERF

using NUnit.Framework;
using UnityEngine;
using Active.Core;
using static Active.Core.status;

class FT_Duelist : TestBase{

    Duelist x;

    [SetUp] public void Setup(){ x = new Duelist(); }

    [Test] public void Step(){ status s = x; }

    class Duelist : Task{

        float health = 100;

        Transform threat => null;

        override public status Step()
        => Attack() || Defend().due || Retreat().undue;

        status Attack() => threat && health > 25
            ? Engage(threat) && Cooldown(1.0f)?[ Strike(threat).now ]
            : fail(log && $"No threat, or low hp ({health})");

      #if !AL_OPTIMIZE
        pending   Defend()                  => + undef();
        status    Engage(Transform threat)  =>   undef();
        impending Retreat()                 => - undef();
      #else
        pending   Defend()                  =>   pending.done();
        status    Engage(Transform threat)  =>   status.done();
        impending Retreat()                 =>   impending.doom();
      #endif

        action    Strike(Transform threat)  =>   @void();

    }

}

#endif  // !AL_BEST_PERF
#endif
