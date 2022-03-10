using System.Collections.Generic;

namespace FluentAuthorization
{
    internal interface IDataContainer<TData>
    {
        IEnumerable<TData> Data { get; }
    }
}
