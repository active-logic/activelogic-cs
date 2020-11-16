using NUnit.Framework;
using P = Active.Core.pending;
using I = Active.Core.impending;
using S = Active.Core.status;
using static Active.Raw;

namespace Unit.Certainties{
public class TypeConversions : CoreTest{

    // From Action --------------------------------------------------

    [Test] public void ActionToBool   () => o( ((bool)@void) );
    [Test] public void ActionToPending() => o( ((P)@void).complete );
    [Test] public void ActionToStatus () => o( ((S)@void).complete );

    // From Failure --------------------------------------------------

    [Test] public void FailureToBool     () => o( !((bool)@false) );
    [Test] public void FailureToImpending() => o( ((I)@false).failing );
    [Test] public void FailureToStatus   () => o( ((S)@false).failing );

    // From Loop -----------------------------------------------------

    [Test] public void LoopToPending  () =>  o( ((P)forever).running );
    [Test] public void LoopToImpending() =>  o( ((I)forever).running );
    [Test] public void LoopToStatus   () =>  o( ((S)forever).running );

    // From Pending/Impending ----------------------------------------

    [Test] public void PendingToStatus()
    => o( ((S)pending_cont).running && ((S)pending_done).complete );

    [Test] public void ImpendingToStatus()
    => o( ((S)impending_cont).running && ((S)impending_fail).failing );

}}
