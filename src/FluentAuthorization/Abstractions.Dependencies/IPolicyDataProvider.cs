using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    /// <summary>
    /// Abstraction of a service that provides policy data.
    /// </summary>
    /// <typeparam name="TUser">The type of the user context object.</typeparam>
    public interface IPolicyDataProvider<TUser>
    {
        /// <summary>
        /// Retrieves the data for the requested policy type.
        /// </summary>
        /// <typeparam name="TPolicy">The policy type to get data for.</typeparam>
        /// <typeparam name="TResource">The resource type of the policy.</typeparam>
        /// <typeparam name="TData">The data type of the policy.</typeparam>
        /// <param name="user">The user to request data for.</param>
        /// <param name="policy">The policy instance to request data for.</param>
        /// <param name="resource">The resource instance to request data for.</param>
        /// <returns></returns>
        Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(TUser user, TPolicy policy, TResource resource) where TPolicy : IPolicy<TUser, TResource, TData>;
    }
}
