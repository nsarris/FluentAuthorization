namespace FluentAuthorization
{
    public interface IPolicyContextBuilder<TUser, out T, TResource> where T : IPolicyWithResource<TUser, TResource>
    {

    }
}
