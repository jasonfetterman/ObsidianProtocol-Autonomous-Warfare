using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void Register<T>(T service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        Services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        if (Services.TryGetValue(typeof(T), out object service))
            return (T)service;

        throw new InvalidOperationException(
            $"ServiceLocator: Service of type {typeof(T).Name} has not been registered."
        );
    }

    public static bool TryGet<T>(out T service)
    {
        if (Services.TryGetValue(typeof(T), out object value))
        {
            service = (T)value;
            return true;
        }

        service = default;
        return false;
    }

    public static bool Has<T>()
    {
        return Services.ContainsKey(typeof(T));
    }

    public static void Unregister<T>()
    {
        Services.Remove(typeof(T));
    }

    public static void Clear()
    {
        Services.Clear();
    }
}