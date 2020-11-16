using System; using InvOp = System.InvalidOperationException;
using NUnit.Framework; using static Active.Raw; using Active.Core;

namespace Unit.Certainties{
public class BooleanOps : OpsTest{

    [Test] public void Bool_x_Status([Values(false, true)] bool x,
                                     [Range(-1, 1)]        int y){
        o(x && s(y), s(x ? 1 : -1) && s(y));
        o(x || s(y), s(x ? 1 : -1) || s(y));
    }

    [Test] public void Bool_x_Action([Values(false, true)] bool x){
        o(x && @void, x);
        o(x || @void, true);
    }

    [Test] public void Bool_x_Failure([Values(false, true)] bool x){
        o(x && @false, false);
        o(x || @false, x);
    }

    [Test] public void Bool_x_Loop([Values(false, true)] bool x){
        o(x && (status)forever, (status)x && forever);
        o(x || (status)forever, (status)x || forever);
    }

    [Test] public void Bool_x_Pending([Values(false, true)] bool x,
                                      [Range(0, 1)]         int  y){
        o((status)x && p(y), x && s(y));
        o((status)x || p(y), x || s(y));
    }

    [Test] public void Bool_x_Impending([Values(false, true)] bool x,
                                        [Range(-1, 0)]        int  y){
        o((status)x && i(y), x && s(y));
        o((status)x || i(y), x || s(y));
    }

}}
