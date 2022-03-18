namespace FluentAuthorization
{
    /// <summary>
    /// A fluent provider of policy contexts.
    /// </summary>
    /// <typeparam name="TUser">The user context type.</typeparam>
    public interface IPolicyContextProvider<TUser, TPolicy, TResource> : IPolicyContextProvider<TUser, TPolicy>
        where TPolicy : IPolicy<TUser>, IPolicyWithResource<TUser, TResource>
    {
        /// <summary>
        /// Set the resource for this context.
        /// </summary>
        /// <typeparam name="TResource">the type of Resource.</typeparam>
        /// <param name="resource">The given resource.</param>
        IPolicyContextProvider<TUser, TPolicy, TResource> ForResource(TResource resource);
    }
}
