using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser, T, TResource> where T : IPolicyWithResource<TUser, TResource>
    {
        T Policy { get; }

        Task<IPolicyContext<T>> BuildContextAsync();
        Task<IPolicyContext<T>> BuildContextAsync(TUser user);
    }
}
