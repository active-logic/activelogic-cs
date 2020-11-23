using System.Collections.Generic;
using Active.Core;

namespace Active.Core.Details{
class HashStore : Store{

    internal Dictionary<int, Seq> seqmap;            // Stores new seq
    internal Dictionary<int, Sel> selmap;            // Stores new seq

    internal Dictionary<int, Composite> cmap;     // Stores composites
    internal Dictionary<int, AbstractDecorator> dmap; // Decorators

    Dictionary<int, AbstractDecorator> decs
    => (dmap = dmap ?? new Dictionary<int, AbstractDecorator>());

    Dictionary<int, Composite> composites
    => (cmap = cmap ?? new Dictionary<int, Composite>());

    Dictionary<int, Seq> seqs
    => (seqmap = seqmap ?? new Dictionary<int, Seq>());

    Dictionary<int, Sel> sels
    => (selmap = selmap ?? new Dictionary<int, Sel>());

    Seq Store.Seq(int key){
        Seq @out;
        seqs.TryGetValue(key, out @out);
        if(@out != null){
            //nityEngine.Debug.Log("Ret seq "+@out);
            return @out;
        }
        Seq _new = new Seq();
        seqs[key] = _new;
        //nityEngine.Debug.Log("Ret new seq "+_new);
        return _new;
    }

    Sel Store.Sel(int key){
        Sel @out;
        sels.TryGetValue(key, out @out);
        if(@out!=null)
            return @out;
        Sel _new = new Sel();
        sels[key] = _new;
        return _new;
    }

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

    void Store.Reset(){
        cmap?.Clear();
        dmap?.Clear();
        seqmap?.Clear();
        selmap?.Clear();
    }

}}
