using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    /// <summary>
    /// Helper class to access permissions using property names.
    /// </summary>
    internal static class PolicyReflector
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, IReadOnlyDictionary<string, Func<IPolicy, IPermission>>> statelessPermissions = new();
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, IReadOnlyDictionary<string, (Type stateType, Delegate getter)>> statefullPermissions = new();

        /// <summary>
        /// Gets a stateless permission from a policy object using the permission's property name.
        /// </summary>
        /// <typeparam name="TPolicy">The type of policy.</typeparam>
        /// <typeparam name="TUser">The type of user context.</typeparam>
        /// <typeparam name="TResource">The type of resource.</typeparam>
        /// <typeparam name="TData">The policy data type.</typeparam>
        /// <param name="policy">The given policy.</param>
        /// <param name="permissionName">The permission's property name.</param>
        /// <returns>The permission with the given name.</returns>
        /// <exception cref="ArgumentException">Thrown when there is no maytching property on the policy object.</exception>
        public static IPermission GetPermission<TPolicy, TUser, TResource, TData>(TPolicy policy, string permissionName)
            where TPolicy : Policy<TUser, TResource, TData>
        {
            var policyType = policy.GetType();

            var properties = statelessPermissions.GetOrAdd(policyType, type => type
                .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(x => x.CanRead)
                .Where(x => typeof(Policy<TUser, TResource, TData>.Permission).IsAssignableFrom(x.PropertyType))
                .ToDictionary(x => x.Name, x => buildGetter(policyType, x)));

            if (properties.TryGetValue(permissionName, out var getter))
                return getter(policy);

            throw new ArgumentException($"Policy '{policy.Name}' of type '{policyType}' does not contain a Permission property '{permissionName}'.", nameof(permissionName));

            static Func<IPolicy, IPermission> buildGetter(Type policyType, System.Reflection.PropertyInfo property)
            {
                var parameter = Expression.Parameter(typeof(IPolicy));
                var castedParameter = Expression.Convert(parameter, policyType);
                var body = Expression.Property(castedParameter, property);
                var castedResult = Expression.Convert(body, typeof(IPermission));
                return Expression.Lambda<Func<IPolicy, IPermission>>(castedResult, parameter).Compile();
            }
        }

        /// <summary>
        /// Gets a statefull permission from a policy object using the permission's property name.
        /// </summary>
        /// <typeparam name="TPolicy">The type of policy.</typeparam>
        /// <typeparam name="TUser">The type of user context.</typeparam>
        /// <typeparam name="TResource">The type of resource.</typeparam>
        /// <typeparam name="TData">The policy data type.</typeparam>
        /// /// <typeparam name="TState">The permission's state type.</typeparam>
        /// <param name="policy">The given policy.</param>
        /// <param name="permissionName">The permission's property name.</param>
        /// <returns>The permission with the given name.</returns>
        /// <exception cref="ArgumentException">Thrown when there is no maytching property on the policy object.</exception>
        public static IPermission<TState> GetPermission<TPolicy, TUser, TResource, TData, TState>(TPolicy policy, string permissionName)
            where TPolicy : Policy<TUser, TResource, TData>
        {
            var policyType = policy.GetType();
            
            var properties = statefullPermissions.GetOrAdd(policyType, type => type
                .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(x => x.CanRead)
                .Where(x => IsAssignableToGenericType(x.PropertyType, typeof(Policy<,,>.Permission<>), new[] { typeof(TUser), typeof(TResource),typeof(TData) }))
                .ToDictionary(x => x.Name, x => (x.PropertyType.GetGenericArguments().Last(), (Delegate)buildGetter(policyType, x))));

            if (properties.TryGetValue(permissionName, out var getter)
                && getter.stateType == typeof(TState))
                return (IPermission<TState>)((Func<IPolicy, object>)getter.getter)(policy);

            throw new ArgumentException($"Policy '{policy.Name}' of type '{policyType}' does not contain a Permission property '{permissionName}' with state type '{typeof(TState)}'.", nameof(permissionName));

            static Func<IPolicy, object> buildGetter(Type policyType, System.Reflection.PropertyInfo property)
            {
                var parameter = Expression.Parameter(typeof(IPolicy));
                var castedParameter = Expression.Convert(parameter, policyType);
                var body = Expression.Property(castedParameter, property);
                var castedResult = Expression.Convert(body, typeof(object));
                return Expression.Lambda<Func<IPolicy, object>>(castedResult, parameter).Compile();
            }

            static bool IsAssignableToGenericType(Type givenType, Type genericType, Type[] typeArguments)
            {
                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType
                    && typeArguments.SequenceEqual(givenType.GenericTypeArguments.Take(typeArguments.Length)))
                    return true;

                Type baseType = givenType.BaseType;
                if (baseType == null) return false;

                return IsAssignableToGenericType(baseType, genericType, typeArguments);
            }
        }
    }
}
