using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextProviderInternal<TUser, T, TResource> where T : IPolicyWithResource<TUser, TResource>
    {
        Task<IPolicyContext<T>> BuildAsync();
        Task<IPolicyContext<T>> BuildAsync(TUser user);
    }

    internal interface IPolicyContextProviderInternal<TUser, T, TResource, TData> where T : IPolicy<TUser, TResource, TData>
    {
        Task<IPolicyContext<T>> BuildAsync(TData data);
        IPolicyContext<T> Build(TUser user, TData data);
    }
}
