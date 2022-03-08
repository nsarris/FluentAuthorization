using FluentAuthorization;
using System;

namespace SampleApplication.Authorization
{
    public abstract class BasePolicy<TResource, TData> : Policy<Principal, TResource, TData>
    {
    
    }
}
