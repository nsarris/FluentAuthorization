namespace FluentAuthorization
{
    /// <summary>
    /// A fluent provider of policy contexts.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IPolicyContextProvider<TUser>
    {
        /// <summary>
        /// Override the encapsulated user for this context.
        /// </summary>
        /// <param name="user">The given user.</param>
        /// <returns></returns>
        IPolicyContextProvider<TUser> ForUser(TUser user);

        /// <summary>
        /// Set the resource for this context.
        /// </summary>
        /// <typeparam name="TResource">the type of Resource.</typeparam>
        /// <param name="resource">The given resource.</param>
        /// <returns></returns>
        IPolicyContextProvider<TUser, TResource> ForResource<TResource>(TResource resource);
    }
}
