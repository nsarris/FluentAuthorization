using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public static class PolicyContextProviderExtensions
    {
        public static Task<IPolicyContext<T>> BuildContextAsync<T, TResource>(this IPolicyContextBuilder<T, TResource> provider)
            where T : IPolicyWithResource<TResource>
        {
            var typedProvider = (IPolicyContextProviderInternal<T, TResource>)provider;
            return typedProvider.BuildAsync();
        }

        public static async Task<IEnumerable<TData>> GetDataAsync<TResource, TData>(this IPolicyContextBuilder<IPolicy<TResource, TData>, TResource> provider)

        {
            var typedProvider = (IPolicyContextDataProviderInternal<TData>)provider;

            return new[] { await typedProvider.GetDataAsync() };
        }

        public static Task<TData> GetAggregatedData<TResource, TData>(this IPolicyContextBuilder<IPolicy<TResource, TData>, TResource> provider)
        {
            var typedProvider = (IPolicyContextDataProviderInternal<TData>)provider;

            return typedProvider.GetDataAsync();
        }

        
    }
}
