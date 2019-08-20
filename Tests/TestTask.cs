#if UNITY_2018_1_OR_NEWER

using NUnit.Framework;
using Active.Core;
using Active.Core.Details;
using F = Active.Core.Details.StatusFormat;
using static Active.Core.status;

public class TestTask : CoreTest {

    // todo move to CoreTest
    protected static readonly LogString log = null;

    Job x;

    [SetUp] public void Setup(){ x = new Job(); F.UseASCII(); }

  #if !AL_OPTIMIZE

    [Test] public void IndexingForm1() => o( F.Status(x.indexing1),
                                             "+ TestTask.indexing1 (Re:port)");

    [Test] public void IndexingForm2() => o( F.Status(x.indexing2),
                                            "+ TestTask.nested (Re:port)");

    [Test] public void RedundantTrace(){
        o( F.Status(x.redundant).Substring(2), "TestTask.redundant" );
    }

  #endif  // AL_OPTIMIZE

    class Job : Task{

        string port = "port";

        override public status Step() => done();

        //public status ExposeUndef() => undef();

        public status redundant => Eval(done());

        public status indexing1 => (done() && cont())[log && $"Re:{port}"];

        public status indexing2 => nested[log && $"Re:{port}"];

        public status nested => done() && cont();

    }

}

#endif
