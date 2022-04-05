using System;

namespace FluentAuthorization
{
    /// <summary>
    /// A policy context to perform permission assertions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPolicyContext<out T> where T : IPolicy
    {
        /// <summary>
        /// The encapsulated policy.
        /// </summary>
        T Policy { get; }

        /// <summary>
        /// Assert the selected permission.
        /// </summary>
        /// <param name="select">The permission to assert.</param>
        /// <returns>An assertion resul.</returns>
        AssertionResult Assert(Func<T, IPermission> select);

        /// <summary>
        /// Assert the selected permission.
        /// </summary>
        /// <param name="permissionName">The permission property name to assert.</param>
        /// <returns>An assertion resul.</returns>
        AssertionResult Assert(string permissionName);

        /// <summary>
        /// Assert the selected permission.
        /// </summary>
        /// <param name="select">The permission to assert.</param>
        /// <returns>An assertion resul.</returns>
        AssertionResult Assert<TState>(Func<T, IPermission<TState>> select, TState state);

        /// <summary>
        /// Assert the selected permission.
        /// </summary>
        /// <param name="permissionName">The permission property name to assert.</param>
        /// <returns>An assertion resul.</returns>
        AssertionResult Assert<TState>(string permissionName, TState state);
    }
}
