using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal interface IPolicyContextProviderWithData<TUser, TPolicy, TData> : IPolicyContextProvider<TUser, TPolicy>
        where TPolicy : IPolicy<TUser>, IPolicyWithData<TData>
    {
        Task<IPolicyContext<TPolicy>> BuildContextAsync(TData data);
        Task<IPolicyContext<TPolicy>> BuildContextAsync(IEnumerable<TData> data);
        IPolicyContext<TPolicy> BuildContext(TUser user, TData data);
        IPolicyContext<TPolicy> BuildContext(TUser user, IEnumerable<TData> data);
    }
}
