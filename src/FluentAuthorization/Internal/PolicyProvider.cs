using System;
using System.Collections.Concurrent;

namespace FluentAuthorization
{
    internal static class PolicyProvider
    {
        private static readonly ConcurrentDictionary<Type, IPolicy> cache = new();

        public static T Get<T>() where T : class, IPolicy, new()
        {
            return (T)cache.GetOrAdd(typeof(T), _ => Activator.CreateInstance<T>());
        }
    }
}

