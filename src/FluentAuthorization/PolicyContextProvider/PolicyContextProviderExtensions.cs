using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public static class PolicyContextProviderExtensions
    {
        /// <summary>
        /// Sets an optional resource for context.
        /// </summary>
        /// <remarks>The resource value is used to retrieve policy data and passed on to the assertion context. Make sure that a resource is passed if reuired by the policy.</remarks>
        public static IPolicyContextProvider<TUser, TPolicy> ForResource<TUser, TPolicy, TResource>(this IPolicyContextProvider<TUser, TPolicy> context, TResource resource)
            where TPolicy : class, IPolicyWithResource<TUser, TResource>, new()
        {
            return ((IPolicyContextProvider<TUser, TPolicy, TResource>)context).ForResource(resource);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated data. This will lazily invoke side effects on <see cref="IUserContextProvider{TUser}"/> (if not overriden with <see cref="IPolicyContextProvider{TUser}.ForUser"/>).
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TData>(this IPolicyContextProvider<TUser, T> provider, TData data)
            where T : IPolicy<TUser, TData>
        {
            var typedProvider = (IPolicyContextProviderWithData<TUser, T, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated data. This will lazily invoke side effects on <see cref="IUserContextProvider{TUser}"/> (if not overriden with <see cref="IPolicyContextProvider{TUser}.ForUser"/>).
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IPolicyContext<T>> BuildContextAsync<TUser, T, TData>(this IPolicyContextProvider<TUser, T> provider, IEnumerable<TData> data)
            where T : IPolicy<TUser, TData>
        {
            var typedProvider = (IPolicyContextProviderWithData<TUser, T, TData>)provider;
            return typedProvider.BuildContextAsync(data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated user and data. This will not invoke any side effects.
        /// </summary>
        /// <returns>A new context.</returns>
        public static IPolicyContext<T> BuildContext<TUser, T, TData>(this IPolicyContextProvider<TUser, T> provider, TUser user, TData data)
            where T : IPolicy<TUser, TData>
        {
            var typedProvider = (IPolicyContextProviderWithData<TUser, T, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        /// <summary>
        /// Builds a new context overriding the encapsulated user and data. This will not invoke any side effects.
        /// </summary>
        /// <returns>A new context.</returns>
        public static IPolicyContext<T> BuildContext<TUser, T, TData>(this IPolicyContextProvider<TUser, T> provider, TUser user, IEnumerable<TData> data)
            where T : IPolicy<TUser, TData>
        {
            var typedProvider = (IPolicyContextProviderWithData<TUser, T, TData>)provider;
            return typedProvider.BuildContext(user, data);
        }

        /// <summary>
        /// Gets the encapsulated data of the context. This will invoke side effects on <see cref="IPolicyDataProvider{TUser}"/>
        /// </summary>
        /// <returns>A new context.</returns>
        public static Task<IEnumerable<TData>> GetDataAsync<TUser, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TData>> provider)
        {
            var typedProvider = (IDataProvider<TData>)provider;

            return typedProvider.GetDataAsync();
        }

        /// <summary>
        /// Gets an aggregated instance of the encapsulated data of the context. This will invoke side effects on <see cref="IPolicyDataProvider{TUser}"/>
        /// </summary>
        /// <returns>A new context.</returns>
        public static async Task<TData> GetAggregatedData<TUser, TData>(this IPolicyContextProvider<TUser, IPolicy<TUser, TData>> provider)
        {
            var data = await provider.GetDataAsync();

            return provider.Policy.Aggregate(data);
        }
    }
}
