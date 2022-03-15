using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public static class PolicyContextProviderExtensions
    {
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, IEnumerable<TData> data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        public static IPolicyContext<T> BuildContext<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TUser user, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        public static IPolicyContext<T> BuildContext<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TUser user, IEnumerable<TData> data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        public static Task<IEnumerable<TData>> GetDataAsync<TUser, TResource, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var typedProvider = (IDataProvider<TData>)provider;

            return typedProvider.GetDataAsync();
        }

        public static async Task<TData> GetAggregatedData<TUser, TResource, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var data = await provider.GetDataAsync();

            return provider.Policy.Aggregate(data);
        }
    }
}
