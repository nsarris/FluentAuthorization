namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser>
    {
        IPolicyContextProvider<TUser> ForUser(TUser user);
        IPolicyContextProvider<TUser, TResource> ForResource<TResource>(TResource resource);
    }
}
