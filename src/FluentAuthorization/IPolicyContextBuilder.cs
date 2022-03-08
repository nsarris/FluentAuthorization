namespace FluentAuthorization
{
    public interface IPolicyContextBuilder<TUser, TResource>
    {
        IPolicyContextBuilder<TUser, T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TUser, TResource>, new();
    }
}
