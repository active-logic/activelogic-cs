using NUnit.Framework;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

public class TestCallerInfo : TestBase{

    [Test] public void Basic() => o( Get() , 6 );

    [Test] public void Shift(){
        o(1 << 16 , 65536);
    }

    // Caller info returns actual line number
    [Test] public void MultiLine(){
        int a, b;
        int c = (a = Get())
              + (b = Get());
        o(a != b);
        o(a, 15);
        o(b, 16);
    }

    public int Get([Tag] int n = -1) => n;

}
