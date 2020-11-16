using System; using NUnit.Framework; using static Active.Raw;

namespace Unit.Certainties{ public class StatusOps : OpsTest{

    [Test] public void Status_x_Action([Range(-1, 1)] int x){
        o(s(x) && @void, s(x) && done);
        o(s(x) || @void, s(x) || done);
    }
    [Test] public void Status_x_Failure([Range(-1, 1)] int x){
        o(s(x) && @false, s(x) && fail);
        o(s(x) || @false, s(x) || fail);
    }
    [Test] public void Status_x_Loop([Range(-1, 1)] int x){
        o(s(x) && forever, s(x) && cont);
        o(s(x) || forever, s(x) || cont);
    }
    [Test] public void Status_x_Pending([Range(-1, 1)] int x,
                                        [Range( 0, 1)] int y){
        o(s(x) && p(y), s(x) && s(y));
        o(s(x) || p(y), s(x) || s(y));
    }
    [Test] public void Status_x_Impending([Range(-1, 1)] int x,
                                          [Range(-1, 0)] int y){
        o(s(x) && i(y), s(x) && s(y));
        o(s(x) || i(y), s(x) || s(y));
    }
    [Test] public void Status_x_Boolean([Range(-1, 1)]        int x,
                                        [Values(true, false)] bool y){
        o(s(x) && y, s(x) && s(y ? 1 : -1));
        o(s(x) || y, s(x) || s(y ? 1 : -1));
    }

}}
