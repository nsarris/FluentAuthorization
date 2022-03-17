using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public static class PolicyContextProviderExtensions
    {
        /// <summary>
        /// Builds a new context overriding the encapsulated data. This will lazily invoke side effects on <see cref="IUserContextProvider{TUser}"/> (if not overriden with <see cref="IPolicyContextProvider{TUser}.ForUser"/>).
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated data. This will lazily invoke side effects on <see cref="IUserContextProvider{TUser}"/> (if not overriden with <see cref="IPolicyContextProvider{TUser}.ForUser"/>).
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, IEnumerable<TData> data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated user and data. This will not invoke any side effects.
        /// </summary>
        /// <returns>A new context.</returns>
        public static IPolicyContext<T> BuildContext<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TUser user, TData data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated user and data. This will not invoke any side effects.
        /// </summary>
        /// <returns>A new context.</returns>
        public static IPolicyContext<T> BuildContext<TUser, T, TResource, TData>(this IPolicyContextProvider<TUser, T, TResource> provider, TUser user, IEnumerable<TData> data)
            where T : IPolicy<TUser, TResource, TData>
        {
            var typedProvider = (IPolicyContextProvider<TUser, T, TResource, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        /// <summary>
        /// Gets the encapsulated data of the context. This will invoke side effects on <see cref="IPolicyDataProvider{TUser}"/>
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IEnumerable<TData>> GetDataAsync<TUser, TResource, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var typedProvider = (IDataProvider<TData>)provider;

            return typedProvider.GetDataAsync();
        }

        /// <summary>
        /// Gets an aggregated instance of the encapsulated data of the context. This will invoke side effects on <see cref="IPolicyDataProvider{TUser}"/>
        /// </summary>
        /// <returns>A new context.</returns>
        public static async Task<TData> GetAggregatedData<TUser, TResource, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TResource, TData>, TResource> provider)
        {
            var data = await provider.GetDataAsync();

            return provider.Policy.Aggregate(data);
        }
    }
}
