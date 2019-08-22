using NUnit.Framework;
using Active.Core;
using static Active.Core.status;

public class StatusPerf : TestBase {

    const int unary_iters = PerfTestIters / 3;
    const int binary_iters = PerfTestIters / 9;

    public delegate status UnaryOp    (in status x);
    public delegate status BinaryOp   (in status x, in status y);
    public delegate bool   Comparator (in status x, in status y);

    [SetUp] public void Setup() => status.log = true;

    [Test] public void Create(){
        for(int i = 0; i < unary_iters; i++){
            var x = done();
            var y = fail();
            var z = cont();
        }
    }

    [Test] public void iCreateWithIntUnchecked(){
        for(int i = 0; i < PerfTestIters; i++){
            var x = status.@unchecked(1);
        }
    }

    [Test] public void BinaryBaseline()
    => Run((in status x, in status y) => y);

    [Test] public void UnaryBaseline()
    => Run((in status x) => x);

    [Test] public void Eq()
    => Compare((in status x, in status y) => x == y);

    [Test] public void NotEq()
    => Compare((in status x, in status y) => x != y);

    [Test] public void And()
    => Run((in status x, in status y) => x && y);

    [Test] public void Or()
    => Run((in status x, in status y) => x && y);

    [Test] public void StrictCombinator()
    => Run((in status x, in status y) => x * y);

    [Test] public void LenientCombinator()
    => Run((in status x, in status y) => x + y);

    [Test] public void NeutralCombinator()
    => Run((in status x, in status y) => x % y);

    [Test] public void Inverter()
    => Run((in status x) => !x);

    [Test] public void Condoner()
    => Run((in status x) => (~x).due);

    [Test] public void Promoter()
    => Run((in status x) => (+x).due);

    [Test] public void Demoter()
    => Run((in status x) => (-x).undue);

    public void Run(UnaryOp op){
        for(int i = 0; i < unary_iters; i++){
            for(int x = -1; x <= 1; x++){
                op(status.@unchecked(x));
            }
        }
    }

    public void Run(BinaryOp op){
        for(int i = 0; i < binary_iters; i++){
            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    op(status.@unchecked(x), status.@unchecked(y));
                }
            }
        }
    }

    public void Compare(Comparator op){
        for(int i = 0; i < binary_iters; i++){
            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    op(status.@unchecked(x), status.@unchecked(y));
                }
            }
        }
    }

}
