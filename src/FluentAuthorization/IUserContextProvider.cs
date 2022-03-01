using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IUserContextProvider<T>
    {
        Task<T> GetAsync();
    }
}
