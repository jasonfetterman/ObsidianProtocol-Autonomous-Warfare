using System;
using System.Collections.Generic;

public class WorldState
{
    private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

    public void Set<T>(string key, T value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("WorldState key cannot be null or empty.", nameof(key));
        }

        _values[key] = value;
    }

    public bool TryGet<T>(string key, out T value)
    {
        if (_values.TryGetValue(key, out object rawValue) && rawValue is T typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default;
        return false;
    }

    public bool Contains(string key)
    {
        return !string.IsNullOrEmpty(key) && _values.ContainsKey(key);
    }

    public void Remove(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            _values.Remove(key);
        }
    }

    public void Clear()
    {
        _values.Clear();
    }
}
