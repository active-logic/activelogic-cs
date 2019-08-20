using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using Ex = System.Exception;


public class FT_OrderedSequence : CoreTest {

    const int MaxIter = 16;
    int foo;
    Sequence c;

    [SetUp] public void Setup(){
        c = new Sequence();
        foo = 0;
    }

    [Test] public void TestCaseNotation(){
        status s = cont;
        for(int i = 0; !s.complete; i++){
            switch(c){
                case 0 : s = c[ to(4)  ]; break;
                case 1 : s = c[ to(10) ]; break;
                case 2 : s = c[ to(15) ]; break;
                default: s = c.end;       break;
            } if(i >= MaxIter) throw new Ex("Too many iterations");
        }
        o((int)c, 3);
        o(foo, 15);
    }

    [Test] public void TestTernaryNotation(){
        status s = fail;
        for(int j = 0; !s.complete; j++){
            var i = new SeqIterator(c);
            s = i[ i? to(4)  :
                   i? to(10) :
                   i? to(15) : i.end];
           if( j>= MaxIter) throw new Ex("Too many iterations");
        }
        o(foo, 15);
    }

    [Test] public void TestIncorrectIteratorPos(){
        status s = fail;
        var i = new SeqIterator(c); // Reusing across iterations (wrong)
        for(int j = 0; !s.complete; j++){
            s = i[ i?to(4) : i?to(10) : i?to(15) : i.end];
            if(j >= MaxIter) throw new Ex("Too many iterations");
        }
        o(foo != 15);
    }

    status to(int k) => ++foo == k ? done : cont;

}
