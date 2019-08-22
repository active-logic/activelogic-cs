using System;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class CompositePerf : CoreTest {

    int foo;

    [Test] public void B__15_Baseline(){
        foo = 0;
        status.log = false;
        status s = fail;
        for(int j = 0; j < PerfTestIters; j++){
            s = to(15);
            if(foo==15) reset();
        }
    }

    [Test] public void B__30_CaseNotation(){
        foo = 0;
        status.log = false;
        Sequence c = new Sequence();
        status s = fail;
        for(int j = 0; j < PerfTestIters; j++){
            switch(c){
                case 0 : s = c[ to(4)  ]; break;
                case 1 : s = c[ to(10) ]; break;
                case 2 : s = c[ to(15) ]; break;
                case 3 : s = c[ reset() ]; break;
                default: s = c.loop;      break;
            }
        }
    }

    [Test] public void B__50_OrderedΛSequence(){
        foo = 0;
        status.log = false;
        var c = MComposite.Sequence(
            new Func<status>[]{
                () => to(4),
                () => to(10),
                () => to(15),
                () => reset() });
        c.ordered = true;
        c.loop = true;
        status s;
        for(int j = 0; j < PerfTestIters; j++) s = c;
    }

    [Test] public void B__75_ThreadSafeTernaryNotation(){
        foo = 0;
        status.log = false;
        var c = new Sequence();
        status s = fail;
        var i = new SeqIterator(c);
        for(int j = 0; j < PerfTestIters; j++){
            i.Reset();
            s = i[ i ? to( 4) :
                   i ? to(10) :
                   i ? to(15) :
                   i ? reset() :
                   i.end];
        }
    }

    [Test] public void B__85_StatusExp(){
        foo = 0;
        status.log = false;
        Sequence c = new Sequence();
        status s = fail;
        for(int j = 0; j < PerfTestIters; j++){
            s = to(4) && to(10) && to(15) && reset();
        }
    }

    [Test] public void B_325_ProgressiveΛSequence(){
        foo = 0;
        status.log = false;
        var c = MComposite.Sequence(
            new Func<status>[]{
                () => to(4),
                () => to(10),
                () => to(15),
                () => reset() });
        c.progressive = true;
        status s;
        for(int j = 0; j < PerfTestIters; j++) s = c;
    }

    status to(int k){
        if(foo >= k || ++foo == k) return done;
        return cont;
    }

    status reset(){ foo = 0; return done; }

}
