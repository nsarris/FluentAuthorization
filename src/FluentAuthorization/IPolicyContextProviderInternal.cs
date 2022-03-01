using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextProviderInternal<T, TResource> where T : IPolicyWithResource<TResource>
    {
        Task<IPolicyContext<T>> BuildAsync();
    }
}
