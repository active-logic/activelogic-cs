namespace Active.Core{

public class StatusAssert : System.Exception{

    public StatusAssert(string message) : base(message) {}

}

partial struct status{

    public status AssertComplete() => ω == 1 ? this
    : throw new StatusAssert($"Expecting `done`, found {this}");

    public status AssertFailing() => ω == -1 ? this
    : throw new StatusAssert($"Expecting `fail`, found {this}");

    public status AssertRunning() => ω == 0 ? this
    : throw new StatusAssert($"Expecting `cont`, found {this}");

    public status AssertPending() => ω != -1 ? this
    : throw new StatusAssert($"Pending status, failed");

    public status AssertImpending() => ω != 1 ? this
    : throw new StatusAssert($"Impending status, succeeded");

    public status AssertImmediate() => ω != 0 ? this
    : throw new StatusAssert($"Expecting `done` or `fail`, found `cont`");

}}
