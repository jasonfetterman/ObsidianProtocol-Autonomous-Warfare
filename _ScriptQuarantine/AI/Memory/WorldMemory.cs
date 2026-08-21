using System;
using System.Collections.Generic;

public class WorldMemory
{
    private readonly Dictionary<string, object> _memories = new Dictionary<string, object>();

    public void Remember<T>(string key, T value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("WorldMemory key cannot be null or empty.", nameof(key));
        }

        _memories[key] = value;
    }

    public bool TryRecall<T>(string key, out T value)
    {
        if (_memories.TryGetValue(key, out object rawValue) && rawValue is T typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default;
        return false;
    }

    public bool HasMemory(string key)
    {
        return !string.IsNullOrEmpty(key) && _memories.ContainsKey(key);
    }

    public void Forget(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            _memories.Remove(key);
        }
    }

    public void Clear()
    {
        _memories.Clear();
    }
}
