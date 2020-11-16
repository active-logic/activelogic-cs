using System;
using Ex = System.Exception;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using static Active.Raw;
using Active.Core;
using RBE = Microsoft.CSharp.RuntimeBinder.RuntimeBinderException;

namespace Unit.Certainties{
public abstract class OpsTest : CoreTest{

    protected dynamic z;

    protected Func<int, Active.Core.status> s
              = Active.Core.status.@unchecked;

    protected pending p(int x) => x == 1 ? pending_done : x == 0
              ? pending_cont : throw new Ex();

    protected impending i(int x) => x == -1 ? impending_fail : x == 0
              ? impending_cont : throw new Ex();

    public void AND_CS217_InvOp(dynamic x, params dynamic[] y){
        foreach(var e in y)
            Assert.Throws<InvOp>( () => { var z = x && e; } );
    }

    public void OR__CS217_InvOp(dynamic x, params dynamic[] y){
        foreach(var e in y)
            Assert.Throws<InvOp>( () => { var z = x || e; } );
    }

    public void OR__CS19_Disjoint(dynamic x, params dynamic[] y_arr){
        foreach(dynamic y in y_arr){
            status s = (status)x; if(s.running) return;
            Assert.That( () => { var z = x || y; },
                Throws.TypeOf<RBE>().With.Message.Contains(
                    "Operator '||' cannot be applied to operands"));
        }
    }

    public void AND_CS19_Disjoint(dynamic x, params dynamic[] y_arr){
        foreach(dynamic y in y_arr){
            status s = (status)x; if(s.running) return;
            Assert.That( () => { var z = x && y; },
                Throws.TypeOf<RBE>().With.Message.Contains(
                    "Operator '&&' cannot be applied to operands"));
        }
    }

}}
