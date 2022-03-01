namespace FluentAuthorization
{
    public interface IPolicyContextBuilder<TResource>
    {
        IPolicyContextBuilder<T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TResource>;
    }
}
