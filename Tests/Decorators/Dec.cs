using Active.Core;
using Active.Core.Details;

// Used to test ReCon features
public class Dec: Resettable{
    public bool didReset = false;
    public action Reset(){
        didReset = true;
        return status.@void();
    }
}
