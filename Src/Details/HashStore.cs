using System.Collections.Generic;
using Active.Core;

namespace Active.Core.Details{
class HashStore : Store{

    Dictionary<int, Composite> cmap;             // Stores composites
    Dictionary<int, AbstractDecorator> dmap;     // Decorators map

    Dictionary<int, AbstractDecorator> decs
    => (dmap = dmap ?? new Dictionary<int, AbstractDecorator>());

    Dictionary<int, Composite> composites
    => (cmap = cmap ?? new Dictionary<int, Composite>());

    T Store.Composite<T>(int key){
        Composite @out;
        composites.TryGetValue(key, out @out);
        if(@out!=null)
            return (T)@out;
        T newt = new T();
        composites[key] = newt;
        return newt;

    }

    T Store.Decorator<T>(int key, int decoratorId){
        AbstractDecorator @out;
        key = (key << 8) + decoratorId;
        if (!decs.TryGetValue(key, out @out)){
            @out = new T();
            decs[key] = @out;
        } return @out as T;
    }

    void Store.Reset(){ cmap?.Clear(); dmap?.Clear(); }

}}
