// TODO: enable
/*
using NUnit.Framework;

using Active.Rx;
using S = Active.Rx.status;
using static Active.Rx.status;

public class StatusPerfRx : TestBase {

    const int unary_iters = PerfTestIters / 3;
    const int binary_iters = PerfTestIters / 9;

    public delegate status UnaryOp    (status x);
    public delegate status BinaryOp   (status x, status y);
    public delegate bool   Comparator (status x, status y);

    // Adding this to match overheads with sibling tests
    [SetUp] public void Setup() => Active.Core.status.log = false;

    [Test] public void Create(){
        for(int i = 0; i < unary_iters; i++){
            var x = done;
            var y = fail;
            var z = cont;
        }
    }

    [Test] public void iCreateWithInt(){
        for(int i = 0; i < PerfTestIters; i++){
            var x = new status(1);
        }
    }

    [Test] public void BinaryBaseline()
    => Run((status x, status y) => y);

    [Test] public void UnaryBaseline()
    => Run((status x) => x);

    [Test] public void Eq()
    => Compare((status x, status y) => x == y);

    [Test] public void NotEq()
    => Compare((status x, status y) => x != y);

    [Test] public void And()
    => Run((status x, status y) => x && y);

    [Test] public void Or()
    => Run((status x, status y) => x && y);

    [Test] public void StrictCombinator()
    => Run((status x, status y) => x * y);

    [Test] public void LenientCombinator()
    => Run((status x, status y) => x + y);

    [Test] public void NeutralCombinator()
    => Run((status x, status y) => x % y);

    [Test] public void Inverter()
    => Run((status x) => !x);

    [Test] public void Condoner()
    => Run((status x) => ~x);

    [Test] public void Promoter()
    => Run((status x) => +x);

    [Test] public void Demoter()
    => Run((status x) => -x);

    public void Run(UnaryOp op){
        for(int i = 0; i < unary_iters; i++){
            for(int x = -1; x <= 1; x++){
                op(new status(x));
            }
        }
    }

    public void Run(BinaryOp op){
        for(int i = 0; i < binary_iters; i++){
            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    op(new status(x), new status(y));
                }
            }
        }
    }

    public void Compare(Comparator op){
        for(int i = 0; i < binary_iters; i++){
            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    op(new status(x), new status(y));
                }
            }
        }
    }

}
*/
