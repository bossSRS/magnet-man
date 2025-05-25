//// Author: Sadikur Rahman ////
// Lightweight DI container using service locator pattern

using System;
using System.Collections.Generic;
using UnityEngine;

public static class DIContainer {
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T instance) {
        Type type = typeof(T);
        if (_services.ContainsKey(type)) {
            Debug.LogWarning($"DIContainer: Replacing already registered type: {type.Name}");
        }
        _services[type] = instance;
    }

    public static T Resolve<T>() {
        Type type = typeof(T);
        if (_services.TryGetValue(type, out var service)) {
            return (T)service;
        }
        Debug.LogError($"DIContainer: Service of type {type.Name} not found.");
        return default;
    }

    public static void Reset() => _services.Clear();
}