using NUnit.Framework;
using Active.Core;

public class TestStrings : TestBase{

    [Test] public void StatusValues(){
        o( Strings.status.names,
           new string[]{"failing", "running", "done"});
    }

}
