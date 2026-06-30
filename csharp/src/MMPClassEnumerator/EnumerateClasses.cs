// -----------------------------------------------------------------------------------------------
//  EnumerateClasses.cs by Marcus Medina, Copyright (C) 2021-2025, http://MarcusMedina.Pro.
//  Published under Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)
//  https://creativecommons.org/licenses/by-sa/4.0/
// -----------------------------------------------------------------------------------------------

namespace MarcusMedinaPro.ClassEnumerator;

using System.Collections.Concurrent;
using System.Reflection;

/// <summary>
/// Provides methods to discover and instantiate classes by interface or inheritance using reflection.
/// Includes optional caching for improved performance.
/// </summary>
/// <typeparam name="T">The interface or base class to search for</typeparam>
public static class EnumerateClasses<T>
{
    private static readonly ConcurrentDictionary<(Assembly, bool), IEnumerable<T>> _instanceCache = new();
    private static readonly ConcurrentDictionary<(Assembly, bool), IEnumerable<Type>> _typeCache = new();

    /// <summary>
    /// Gets instances of classes that inherit from the specified base class.
    /// Results are sorted by type name.
    /// </summary>
    /// <param name="useCache">If true, caches results for improved performance. Default is false.</param>
    /// <param name="assembly">The assembly to search in. If null, uses the calling assembly.</param>
    /// <returns>Enumerable collection of instances that inherit from T</returns>
    /// <example>
    /// <code>
    /// // Find all classes inheriting from BaseService
    /// var services = EnumerateClasses&lt;BaseService&gt;.GetClassesByInheritance();
    /// foreach (var service in services)
    /// {
    ///     Console.WriteLine(service.ServiceName);
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<T> GetClassesByInheritance(bool useCache = false, Assembly? assembly = null)
    {
        var targetAssembly = assembly ?? Assembly.GetCallingAssembly();
        var cacheKey = (targetAssembly, true);

        if (useCache && _instanceCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        // Inspired from https://stackoverflow.com/a/699871
        var instances = from type in targetAssembly.GetTypes()
                        where type.BaseType == typeof(T)
                           && !type.IsAbstract
                           && type.GetConstructor(Type.EmptyTypes) != null
                        select CreateInstance(type);

        var result = instances
            .Where(i => i != null)
            .Cast<T>()
            .OrderBy(n => n?.GetType().Name)
            .ToList();

        if (useCache)
        {
            _instanceCache[cacheKey] = result;
        }

        return result;
    }

    /// <summary>
    /// Gets instances of classes that implement the specified interface.
    /// Results are sorted by type name.
    /// </summary>
    /// <param name="useCache">If true, caches results for improved performance. Default is false.</param>
    /// <param name="assembly">The assembly to search in. If null, uses the calling assembly.</param>
    /// <returns>Enumerable collection of instances that implement T</returns>
    /// <example>
    /// <code>
    /// // Find all plugins implementing IPlugin
    /// var plugins = EnumerateClasses&lt;IPlugin&gt;.GetClassesByInterface(useCache: true);
    /// foreach (var plugin in plugins)
    /// {
    ///     plugin.Execute();
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<T> GetClassesByInterface(bool useCache = false, Assembly? assembly = null)
    {
        var targetAssembly = assembly ?? Assembly.GetCallingAssembly();
        var cacheKey = (targetAssembly, false);

        if (useCache && _instanceCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        // Inspired from https://stackoverflow.com/a/699871
        var instances = from type in targetAssembly.GetTypes()
                        where type.GetInterfaces().Contains(typeof(T))
                           && !type.IsAbstract
                           && !type.IsInterface
                           && type.GetConstructor(Type.EmptyTypes) != null
                        select CreateInstance(type);

        var result = instances
            .Where(i => i != null)
            .Cast<T>()
            .OrderBy(n => n?.GetType().Name)
            .ToList();

        if (useCache)
        {
            _instanceCache[cacheKey] = result;
        }

        return result;
    }

    /// <summary>
    /// Gets type definitions of classes that inherit from the specified base class.
    /// Does not create instances. Results are sorted by type name.
    /// </summary>
    /// <param name="useCache">If true, caches results for improved performance. Default is false.</param>
    /// <param name="assembly">The assembly to search in. If null, uses the calling assembly.</param>
    /// <returns>Enumerable collection of Type objects</returns>
    /// <example>
    /// <code>
    /// // Get type information without creating instances
    /// var serviceTypes = EnumerateClasses&lt;BaseService&gt;.ListClassesByInheritance();
    /// foreach (var type in serviceTypes)
    /// {
    ///     Console.WriteLine($"Found: {type.FullName}");
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<Type> ListClassesByInheritance(bool useCache = false, Assembly? assembly = null)
    {
        var targetAssembly = assembly ?? Assembly.GetCallingAssembly();
        var cacheKey = (targetAssembly, true);

        if (useCache && _typeCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        // Inspired from https://stackoverflow.com/a/699871
        var types = from type in targetAssembly.GetTypes()
                    where type.BaseType == typeof(T)
                       && !type.IsAbstract
                       && type.GetConstructor(Type.EmptyTypes) != null
                    select type;

        var result = types
            .OrderBy(t => t.Name)
            .ToList();

        if (useCache)
        {
            _typeCache[cacheKey] = result;
        }

        return result;
    }

    /// <summary>
    /// Gets type definitions of classes that implement the specified interface.
    /// Does not create instances. Results are sorted by type name.
    /// </summary>
    /// <param name="useCache">If true, caches results for improved performance. Default is false.</param>
    /// <param name="assembly">The assembly to search in. If null, uses the calling assembly.</param>
    /// <returns>Enumerable collection of Type objects</returns>
    /// <example>
    /// <code>
    /// // Get all plugin types without creating instances
    /// var pluginTypes = EnumerateClasses&lt;IPlugin&gt;.ListClassesByInterface();
    /// foreach (var type in pluginTypes)
    /// {
    ///     Console.WriteLine($"Available: {type.Name}");
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<Type> ListClassesByInterface(bool useCache = false, Assembly? assembly = null)
    {
        var targetAssembly = assembly ?? Assembly.GetCallingAssembly();
        var cacheKey = (targetAssembly, false);

        if (useCache && _typeCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        // Inspired from https://stackoverflow.com/a/699871
        var types = from type in targetAssembly.GetTypes()
                    where type.GetInterfaces().Contains(typeof(T))
                       && !type.IsAbstract
                       && !type.IsInterface
                       && type.GetConstructor(Type.EmptyTypes) != null
                    select type;

        var result = types
            .OrderBy(t => t.Name)
            .ToList();

        if (useCache)
        {
            _typeCache[cacheKey] = result;
        }

        return result;
    }

    /// <summary>
    /// Clears all cached results. Useful if assemblies are dynamically loaded.
    /// </summary>
    public static void ClearCache()
    {
        _instanceCache.Clear();
        _typeCache.Clear();
    }

    /// <summary>
    /// Safely creates an instance of the specified type.
    /// Returns null if creation fails.
    /// </summary>
    private static T? CreateInstance(Type type)
    {
        try
        {
            return (T?)Activator.CreateInstance(type);
        }
        catch (Exception ex) when (ex is MissingMethodException or TargetInvocationException or TypeLoadException)
        {
            // Silently ignore types that can't be instantiated
            return default;
        }
    }
}
