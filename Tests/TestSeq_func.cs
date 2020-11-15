using System;
using NUnit.Framework;
using Active.Core; using static Active.Status;
using Active.Core.Details;
using static Active.Core.Seq;

public class TestSeq_func : TestBase{

    [Test] public void TestSeq_1_task(){
        int a = 0;
        var seq = new Seq();
        for(int i = 0; i < 3; i++){
            status z = seq.Step(repeat: false)
                     + task?[ Do(++a) ];
        }
        o(a, 1);
    }

    [Test] public void TestSeq_2_tasks(){
        int a = 0, b = 0;
        var seq = new Seq();
        status s = fail();
        for(int i = 0; i < 3; i++){
            s = seq.Step(repeat: false)
                + task?[ Do(++a) ]
                + task?[ Do(b++) ];
        }
        o(a, 1);
        o(b, 1);
        o(s.complete);
    }

    [Test] public void TestSeq_failing(){
        int a = 0;
        var seq = new Seq();
        for(int i = 0; i < 3; i++){
            status s = seq.Step(repeat: false) + task?[ Fail( a++ ) ];
            o(s.failing);
        }
        o(a, 1);
    }

    [Test] public void TestSeq_cont(){
        int a = 0;
        var seq = new Seq();
        for(int i = 0; i < 3; i++){
            status s = seq.Step(repeat: false) + task?[ Cont( a++ ) ];
            o(s.running);
        }
        o(a, 3);
    }

    status Do(object arg)   => done();
    status Fail(object arg) => fail();
    status Cont(object arg) => cont();

}
