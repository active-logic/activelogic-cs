using System;
using System.Collections.Generic;
using Ex = System.Exception;
using NullRef = System.NullReferenceException;
using NUnit.Framework;
using Active.Core;
using static Active.Core.MComposite;
using static Active.Status;

public class TestMutableComposite : CoreTest{

    MComposite x;

    [SetUp] public void Setup(){
        status.log = false;
        x = new MComposite();
    }

    [TearDown] public void TeardownTMC()
    => status.log = true;

    [Test] public void FieldLoop()
    { o( x.loop, true ); x.loop = false; o( x.loop, false ); }

    [Test] public void PropStatusInitial()
    => o ( x.current.running );

    [Test] public void PropDitherGet() => o ( x.dither, true );

    [Test] public void PropDitherSet()
    => Assert.Throws<Ex>( () => { x.dither = false; } );

    [Test] public void PropConcurrent(){
        x.concurrent = true; o(x.concurrent);
    }

    [Test] public void PropOrdered(){
        x.ordered = true; o(x.ordered);
    }

    [Test] public void PropIsSequenceGet() => o( !x.isSequence );

    [Test] public void PropIsSequenceSet()
    { x.isSequence = true; o ( x.isSequence ); }

    [Test] public void PropIsSelectorGet() => o( x.isSelector, false );

    [Test] public void PropIsSelectorSet()
    { x.isSelector = true; o ( x.isSelector ); }

    [Test] public void MakeSelector(){
        x = Selector();
        o( x.loop,       true );
        o( x.current.failing  );
        o( x.dither,     true );
        o( x.progressive, true);
        o( x.isSelector, true );
        o( x.isSequence, false);
    }

    [Test] public void MakeSequence(){
        x = Sequence();
        o( x.loop,       true );
        o( x.current.complete  );
        o( x.dither,     true );
        o( x.progressive);
        o( x.isSelector, false);
        o( x.isSequence, true );
    }

    [Test] public void MakeSequenceVsMakeSelector(){
        x = MComposite.Sequence();
        var y = MComposite.Selector();
        o( x.loop,       y.loop         );
        o( x.current,    !y.current      );
        o( x.dither,     y.dither       );
        o( x.isSelector, !y.isSelector  );
        o( x.isSequence, !y.isSequence  );
    }

    [Test] public void MkSelWithList() => x=Selector(new List<Func<status>>());

    [Test] public void MkSeqWithList() => x=Sequence(new List<Func<status>>());

    [Test] public void ConvertInvalidCompositeToStatus()
    => Assert.Throws<NullRef>( () => { var s = (status)x; } );

    // Empty sequence succeeds in every control mode, whether looping or not,
    // "everything has been done"
    [Test] public void CvEmptySeqToStatus([Values(false, true)] bool loop,
                                          [Range(0, 2)]         int  flow){
        x = Sequence();
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x).complete );
    }

    // Empty selector fails in every control mode, whether looping or not,
    // "nothing has been tried"
    [Test] public void CvEmptySelToStatus([Values(false, true)] bool loop,
                                          [Range(0, 2)]         int  flow){
        x = Selector();
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x).failing );
    }

    [Test] public void ConvertToStatusFunc(){ var f = (Func<status>)x; }

    [Test] public void FuncStepEmptySel([Values(false, true)] bool loop,
                                        [Range(0, 2)]         int  flow){
        x = Selector();
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x).failing );
    }

    [Test] public void FuncStepEmptySeq([Values(false, true)] bool loop,
                                        [Range(0, 2)]         int  flow){
        x = Sequence();
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x).complete );
    }

    // Singleton composite returns the same value as just running the
    // underlying subtask.
    [Test] public void FuncStepSingletonSel([Range(-1, +1)] int val,
                                            [Values(false, true)] bool loop,
                                            [Range(0, 2)] int flow){
        x = Selector( () => new status(val) );
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x), new status(val));
    }

    [Test] public void FuncStepSingletonSeq([Range(-1, +1)]       int  val,
                                            [Values(false, true)] bool loop,
                                            [Range(0, 2)]         int  flow){
        x = Sequence( () => new status(val) );
        x.loop = loop; FlowForInt(x, flow);
        o( ((status)x), new status(val));
    }

    [Test] public void FuncReset() => o ( x.Reset(), @void() );

    [Test] public void FuncReset_ordered(){
        var arr = new List<Func<status>>();
        x.ι = arr.GetEnumerator();
        x.ordered = true;
        o ( x.Reset(), @void() );
    }

    [Test] public void FuncResetSel() => o( Selector().Reset(), @void() );

    [Test] public void FuncResetSeq() => o( Sequence().Reset(), @void() );

    // ------------------------------------------------------------------------

    void FlowForInt(MComposite x, int val){
        switch(val){
            case 0: x.progressive = true; return;
            case 1: x.ordered     = true; return;
            case 2: x.concurrent  = true; return;
            default: throw new Exception();
        }
    }

}
