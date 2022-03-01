using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextDataProviderInternal
    {
        Task<object> GetDataAsync();
    }
}
