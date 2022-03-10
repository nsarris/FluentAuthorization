using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextProvider<TUser, T, TResource, TData> : IPolicyContextProvider<TUser, T, TResource>
        where T : IPolicy<TUser, TResource, TData>
    {
        Task<IPolicyContext<T>> BuildContextAsync(TData data);
        IPolicyContext<T> BuildContext(TUser user, TData data);
    }
}
