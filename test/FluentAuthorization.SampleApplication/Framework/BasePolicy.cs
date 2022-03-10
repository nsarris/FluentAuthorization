using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleApplication.Authorization
{
    public abstract class BasePolicy<TResource, TData> : Policy<Principal, TResource, TData>
    {
        public override TData Aggregate(IEnumerable<TData> data)
            => data.First();
    }
}
