using NUnit.Framework;
using Active.Core;


public class TestDrive : DecoratorTest<Drive> {

    [Test] public void Drive([Range(-1, 1)] int lh,
                              [Range(-1, 1)] int rh){
        status left = S(lh), right = S(rh);
        o ( (status) x[ left, crit: false ]?[ right ], left );
    }

    [Test] public void Tie([Range(-1, 1)] int lh,
                              [Range(-1, 1)] int rh){
        status left = S(lh), right = S(rh);
        if(lh != 0)
            o ( (status) x[ left, crit: true ]?[ right ], left );
        else
            o ( (status) x[ left, crit: true ]?[ right ], right );
    }

    [Test] public void Shorting([Range(-1, 1)       ] int lh,
                                [Range(-1, 1)       ] int rh,
                                [Values(false, true)] bool crit){
        status left = S(lh);
        int z = 0;
        var state = x[ left, crit ]?[ Assign(rh, out z) ];
        if(lh == 0) o(z, 1);
        else        o(z, 0);
    }

    status Assign(int s, out int z){
        z = 1;
        return S(s);
    }

    status S(int val) => status.@unchecked(val);

}
