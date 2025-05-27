//// Author: Sadikur Rahman ////
// A lightweight Dependency Injection container using Service Locator pattern.
// Allows registering and resolving services by type across your game systems.

using System;
using System.Collections.Generic;
using UnityEngine;

public static class DIContainer {
    private static readonly Dictionary<Type, object> _services = new();

    /// <summary>
    /// Registers a service instance by its type. Overwrites if already registered.
    /// </summary>
    public static void Register<T>(T instance) {
        Type type = typeof(T);
        if (_services.ContainsKey(type)) {
            Debug.LogWarning($"[DI] Overwriting service: {type.Name}");
        }
        _services[type] = instance;
    }

    /// <summary>
    /// Resolves and returns the registered instance of type T. Logs error if not found.
    /// </summary>
    public static T Resolve<T>() {
        if (_services.TryGetValue(typeof(T), out var service)) {
            return (T)service;
        }
        Debug.LogError($"[DI] Service of type {typeof(T).Name} not found.");
        return default;
    }

    /// <summary>
    /// Clears all registered services (for reset or scene transitions).
    /// </summary>
    public static void Reset() => _services.Clear();
}