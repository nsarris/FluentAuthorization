using System.Threading.Tasks;

namespace FluentAuthorization
{
    /// <summary>
    /// Abstraction of a service that provides the context of a user.
    /// </summary>
    /// <typeparam name="T">The type of the user context object.</typeparam>
    public interface IUserContextProvider<T>
    {
        /// <summary>
        /// Retrieves the instance of a user context.
        /// </summary>
        /// <returns>The user context.</returns>
        Task<T> GetAsync();
    }
}
