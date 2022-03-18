using System.Collections.Generic;

namespace FluentAuthorization
{
    internal interface IDataContainer<out TData>
    {
        IEnumerable<TData> Data { get; }
    }
}
