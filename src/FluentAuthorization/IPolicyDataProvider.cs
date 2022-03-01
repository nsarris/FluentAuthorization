using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IPolicyDataProvider<TUser>
    {
        Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(TUser user, TPolicy policy, TResource resource) where TPolicy : IPolicy<TResource, TData>;
    }
}
