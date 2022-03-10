using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IDataProvider<TData>
    {
        Task<IEnumerable<TData>> GetDataAsync();
    }
}
