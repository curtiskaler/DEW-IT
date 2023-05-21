
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace B7.Types;

internal class BoxedObject
{
    public Type ReturnType { get; init; }
    public object Object { get; init; }
    public static BoxedObject Box<T>(T item)
    {
        return new BoxedObject() { ReturnType = typeof(T), Object = item };
    }
}

public class BoxedDictionary
{
    private Dictionary<string, BoxedObject> _boxedObjects;

    public dynamic this[string key]
    {
        get
        {
            bool found = TryGetValue(key, out dynamic value);
            return found ? value : null;
        }
        set { this.Add(key, value); }
    }

    public List<string> Keys => _boxedObjects.Keys.ToList();

    public List<object> Values => _boxedObjects.Values.Select<BoxedObject, object>(it => it.Object).ToList<object>();

    public int Count => _boxedObjects.Count;

    public bool IsReadOnly => false;

    public void Add<T>(string key, T value)
    {
        var obj = new BoxedObject { ReturnType = typeof(T), Object = value };
        _boxedObjects.Add(key, obj);
    }

    public void Add<T>(KeyValuePair<string, T> item)
    {
        Add<T>(item.Key, item.Value);
    }

    public void Clear()
    {
        _boxedObjects.Clear();
    }

    public bool Contains(string key, Type type)
    {
        if (!ContainsKey(key))
        {
            return false;
        }

        BoxedObject value = _boxedObjects[key];
        return value.ReturnType == type;
    }

    public bool ContainsKey(string key)
    {
        return _boxedObjects.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _boxedObjects.Remove(key);
    }

    public bool Remove(string key, Type type)
    {
        if (!ContainsKey(key))
        {
            return false;
        }
        BoxedObject value = _boxedObjects[key];
        return (value.ReturnType == type) ? Remove(key) : false;
    }

    public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        bool found = _boxedObjects.TryGetValue(key, out BoxedObject boxed);
        if (!found || boxed.ReturnType != typeof(T))
        {
            value = default(T);
            return false;
        }

        value = (T)Convert.ChangeType(boxed.Object, boxed.ReturnType);
        return true;
    }
}
