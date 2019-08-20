#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

#if !AL_OPTIMIZE

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestUndef : DecoratorTest<Undef> {

    [Test] public void Format()
    => o( StatusFormat.Status(x[1f]), "+ <U> undef" );

}

#endif
