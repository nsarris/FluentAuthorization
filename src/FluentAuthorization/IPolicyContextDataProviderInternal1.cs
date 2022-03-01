using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextDataProviderInternal<TData>
    {
        Task<TData> GetDataAsync();
    }
}
