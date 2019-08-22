using System;
using System.Collections.Generic;
using Ex = System.Exception;
using NUnit.Framework;
using Active.Core;

public class FT_MutableComposite : CoreTest {

    const int MaxIter = 16;
    int foo;

    [SetUp] public void Setup() => foo = 0;

    [Test] public void TestProgressive(){
        var seq = MComposite.Sequence( new List<Func<status>>{
                  () => to(4), () => to(10), () => to(15) });
        seq.loop = false;
        o (seq.progressive);
        RunSeq(seq);
    }

    [Test] public void TestOrdered(){
        var tasks = new List<Func<status>>{
                  () => to(4), () => to(10), () => to(15) };
        var seq = MComposite.Sequence( tasks );
        seq.loop = false;
        seq.ordered = true;
        RunSeq(seq);
    }

    void RunSeq(MComposite c){
        status s = fail; for(int j = 0; !s.complete; j++){
            s = c;
            if(j >= MaxIter) throw new Ex("Too many iterations");
        } o (foo, 15);
    }

    status to(int k){
        if(foo >= k || ++foo == k) return done;
        return cont;
    }

}
