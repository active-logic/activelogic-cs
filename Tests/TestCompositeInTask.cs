#if UNITY_2018_1_OR_NEWER

using Ex = System.Exception;
using NUnit.Framework;
using Active.Core;
using static Active.Core.status;

public class TestCompositeInTask : CoreTest {

  #if !AL_BEST_PERF

    const int MaxIter = 16;
    Job x;

    [SetUp] public void Setup() => x = new Job();

    [Test] public void StatefulCompositeViaTernary(){
        status s = fail;
        for(int i = 0; !s.complete; i++){
            s = x.Ternary();
            if( i>= MaxIter) throw new Ex("Too many iterations");
        }
        o(x.foo, 15);
    }

    class Job : Active.Core.Task{

        public int foo = 0;
      #if !AL_THREAD_SAFE
        public status Ternary() => Sequence()[
            and ? to(4)  :
            and ? to(10) :
            and ? to(15) :
        end ];
      #else
        public status Ternary(){
            var i = Sequence();
            return i[
                i ? to(4)  :
                i ? to(10) :
                i ? to(15) :
            i.end];
        }
      #endif

        status to(int k) => ++foo == k ? done() : cont();

    }

  #endif
}

#endif
