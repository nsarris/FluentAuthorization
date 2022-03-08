namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser>
    {
        IPolicyContextProvider<TUser> ForUser(TUser user);
        IPolicyContextBuilder<TUser, TResource> ForResource<TResource>(TResource resource);
    }
}
