using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T instance)
    {
        var type = typeof(T);

        if (instance == null)
            throw new ArgumentNullException(nameof(instance), $"Cannot register null for {type.Name}");

        if (services.ContainsKey(type))
            services[type] = instance;
        else
            services.Add(type, instance);
    }

    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.TryGetValue(type, out var instance))
            return (T)instance;

        throw new Exception($"ServiceLocator: No service registered for type {type.Name}");
    }

    public static bool Has<T>()
    {
        return services.ContainsKey(typeof(T));
    }

    public static void Clear()
    {
        services.Clear();
    }
}
