namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        internal interface IPermission
        {
            AssertionResult Assert(AssertionContext context);
            string BuildMessage(AssertionContext context);
            string Name { get; }
        }

        /// <summary>
        /// Abstract policy permission. Inherit from this class to define your own permission classes if you opt ouf using the inline delegate based permission builder.
        /// </summary>
        public abstract class Permission : FluentAuthorization.IPermission, IPermission
        {
            protected abstract AssertionResult Assert(AssertionContext context);
            protected abstract string BuildMessage(AssertionContext context);
            public abstract string Name { get; }

            public override string ToString() => Name;

            AssertionResult IPermission.Assert(AssertionContext context) => Assert(context);
            string IPermission.BuildMessage(AssertionContext context) => BuildMessage(context);
        }
    }
}
