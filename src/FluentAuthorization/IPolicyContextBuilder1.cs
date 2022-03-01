namespace FluentAuthorization
{
    public interface IPolicyContextBuilder<out T, TResource> where T : IPolicyWithResource<TResource>
    {
        //IEnumerable<T> Get<T>();
        //IPolicyContext<T> Build<TData>(TData data);
        //T Policy { get; }
    }
}
