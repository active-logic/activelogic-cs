using Active.Core.Details;

namespace Active.Core{
public interface Store{

    T Composite<T>(int key) where T : Composite, new();

    Seq Seq(int key);
    Sel Sel(int key);

    T Decorator<T>(int key, int decoratorId) where T : AbstractDecorator, new();

    void Reset();

}}
