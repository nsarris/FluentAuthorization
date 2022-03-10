namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser, TResource>
    {
        IPolicyContextProvider<TUser, T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TUser, TResource>, new();
    }
}
