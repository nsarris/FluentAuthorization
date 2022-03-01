namespace FluentAuthorization
{
    internal interface IPolicyContextDataInternal<TData>
    {
        TData Data { get; }
    }
}
