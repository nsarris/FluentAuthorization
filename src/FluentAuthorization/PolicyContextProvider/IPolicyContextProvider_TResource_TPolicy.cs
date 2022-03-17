using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser, T, TResource> where T : IPolicyWithResource<TUser, TResource>
    {
        /// <summary>
        /// The encapsulated policy instance for this context.
        /// </summary>
        T Policy { get; }

        /// <summary>
        /// Builds a new context. This will lazily invoke side effects on <see cref="IUserContextProvider{TUser}"/> (if not overriden with <see cref="IPolicyContextProvider{TUser}.ForUser"/>) and <see cref="IPolicyDataProvider{TUser}"/>.
        /// </summary>
        /// <returns>A new context.</returns>
        Task<IPolicyContext<T>> BuildContextAsync();

        /// <summary>
        /// Builds a new context overriding the encapsulated user. This will lazily invoke side effects on <see cref="IPolicyDataProvider{TUser}"/>.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        Task<IPolicyContext<T>> BuildContextAsync(TUser user);
    }
}
