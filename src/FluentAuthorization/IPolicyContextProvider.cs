namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser>
    {
        IPolicyContextProvider<TUser> ForUser(TUser user);
        IPolicyContextBuilder<TResource> ForResource<TResource>(TResource resource);
    }
}
