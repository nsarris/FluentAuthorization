namespace FluentAuthorization
{
    public interface IPermission
    {
        string Name { get; }
        ISecurityPolicy Policy { get; }
    }

    public abstract partial class SecurityPolicy<TUserSecurityContext>
    {
        public interface IPermission : FluentAuthorization.IPermission
        {
            new SecurityPolicy<TUserSecurityContext> Policy { get; }
            AssertionResult Assert(TUserSecurityContext userSecurityContext);
        }

        public interface IPermission<TInput> : FluentAuthorization.IPermission
        {
            new SecurityPolicy<TUserSecurityContext> Policy { get; }
            AssertionResult Assert(TUserSecurityContext userSecurityContext, TInput input);
        }
    }

    public abstract partial class SecurityPolicy<T, TUserSecurityContext> : SecurityPolicy<TUserSecurityContext>
    where T : PolicyData
    {
        public new interface IPermission : SecurityPolicy<TUserSecurityContext>.IPermission
        {
            new SecurityPolicy<T, TUserSecurityContext> Policy { get; }
        }

        public new interface IPermission<TInput> : SecurityPolicy<TUserSecurityContext>.IPermission<TInput>
        {
            new SecurityPolicy<T, TUserSecurityContext> Policy { get; }
        }
    }
}
