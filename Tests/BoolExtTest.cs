using NUnit.Framework;
using Active.Core;
using static Active.Status;

public class BoolExtTest : TestBase{

    [Test] public void TrueToStatus([Values(true, false)] bool log){
        status.log = log;
        o( true.status(), done());
    }

    [Test] public void FalseToStatus([Values(true, false)] bool log){
        status.log = log;
        o( false.status(), fail());
    }

}
