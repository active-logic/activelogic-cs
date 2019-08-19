using Active.Core;

namespace Active.Core.Details{

public interface Context{
    void Register(Resettable x);
}

public interface Resettable{
    action Reset();
}

}
