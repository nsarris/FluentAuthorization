namespace FluentAuthorization
{
    public interface IPolicyContextProvider<TUser, TResource>
    {
        /// <summary>
        /// Set the policy type for this context.
        /// </summary>
        /// <typeparam name="T">the resource Type.</typeparam>
        /// <returns></returns>
        IPolicyContextProvider<TUser, T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TUser, TResource>, new();
    }
}
