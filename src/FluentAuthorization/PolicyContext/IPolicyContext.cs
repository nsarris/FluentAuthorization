using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Assert the selected async permission.
        /// </summary>
        /// <param name="select">The async permission to assert.</param>
        /// <returns>A task representing the async assertion result.</returns>
        Task<AssertionResult> AssertAsync(Func<T, IAsyncPermission> select);

        /// <summary>
        /// Assert the selected async permission.
        /// </summary>
        /// <param name="permissionName">The permission property name to assert.</param>
        /// <returns>A task representing the async assertion result.</returns>
        Task<AssertionResult> AssertAsync(string permissionName);

        /// <summary>
        /// Assert the selected async permission.
        /// </summary>
        /// <param name="select">The async permission to assert.</param>
        /// <returns>A task representing the async assertion result.</returns>
        Task<AssertionResult> AssertAsync<TState>(Func<T, IAsyncPermission<TState>> select, TState state);

        /// <summary>
        /// Assert the selected async permission.
        /// </summary>
        /// <param name="permissionName">The permission property name to assert.</param>
        /// <returns>A task representing the async assertion result.</returns>
        Task<AssertionResult> AssertAsync<TState>(string permissionName, TState state);
    }
}
