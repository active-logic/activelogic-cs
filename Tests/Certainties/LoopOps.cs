using System;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using static Active.Raw;
using Active.Core;

namespace Unit.Certainties{
public class LoopOps : OpsTest{

    [Test] public void DisallowAND()
    => AND_CS217_InvOp(forever, @void, @false, forever, true, false,
            impending_fail, impending_cont, pending_done,
            pending_cont, fail, cont, done);

    [Test] public void DisallowOR()
    => OR__CS217_InvOp(forever, @void, @false, forever, true, false,
            impending_fail, impending_cont, pending_done,
            pending_cont, fail, cont, done);

}}
