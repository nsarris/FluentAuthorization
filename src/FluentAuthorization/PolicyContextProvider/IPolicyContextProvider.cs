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
        /// Enable caching of the user for subsequent context builds.
        /// </summary>
        /// <param name="user">The given user.</param>
        /// <returns></returns>
        IPolicyContextProvider<TUser> EnableUserCaching();

        /// <summary>
        /// Set the policy for this context.
        /// </summary>
        /// <typeparam name="TPolicy">The type of Policy.</typeparam>
        IPolicyContextProvider<TUser, TPolicy> ForPolicy<TPolicy>()
            where TPolicy : class, IPolicy<TUser>, new();
    }
}
