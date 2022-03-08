using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public static class PolicyContextProviderExtensions
    {
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource>(this IPolicyContextBuilder<TUser, T, TResource> provider)
            where T : IPolicyWithResource<TUser, TResource>
        {
            var typedProvider = (IPolicyContextProviderInternal<TUser, T, TResource>)provider;
            return typedProvider.BuildAsync();
        }

        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource, TData>(this IPolicyContextBuilder<TUser, T, TResource> provider, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProviderInternal<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildAsync(data);
        }

        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource>(this IPolicyContextBuilder<TUser, T, TResource> provider, TUser user)
    where T : IPolicyWithResource<TUser, TResource>
        {
            var typedProvider = (IPolicyContextProviderInternal<TUser, T, TResource>)provider;
            return typedProvider.BuildAsync(user);
        }

        public static IPolicyContext<T> BuildContext<TUser, T, TResource, TData>(this IPolicyContextBuilder<TUser, T, TResource> provider, TUser user, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProviderInternal<TUser, T, TResource, TData>)provider;
            return typedProvider.Build(user, data);
        }

        public static async Task<IEnumerable<TData>> GetDataAsync<TUser, TResource, TData>(this IPolicyContextBuilder<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var typedProvider = (IPolicyContextDataProviderInternal<TData>)provider;

            return new[] { await typedProvider.GetDataAsync() };
        }

        public static Task<TData> GetAggregatedData<TUser, TResource, TData>(this IPolicyContextBuilder<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var typedProvider = (IPolicyContextDataProviderInternal<TData>)provider;

            return typedProvider.GetDataAsync();
        }
    }
}
