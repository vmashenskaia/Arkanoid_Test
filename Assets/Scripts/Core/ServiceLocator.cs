using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Provides global service registration and lookup.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public static T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out object storedService))
            {
                return (T)storedService;
            }

            throw new Exception("Service " + typeof(T) + " not registered.");
        }

        public static bool TryResolve<T>(out T value)
        {
            if (services.TryGetValue(typeof(T), out object storedService))
            {
                value = (T)storedService;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}