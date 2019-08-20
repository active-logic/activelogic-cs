using NUnit.Framework;
using Active.Core;

public class StatusPerfLogless : StatusPerf {

    [SetUp] new public void Setup() => status.log = false;

}
