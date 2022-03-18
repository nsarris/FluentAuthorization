namespace SampleApplication.Authorization.Policies
{

    public class BaseEntityPolicy<TData> : BasePolicy<EntityTypeResource, TData>
        where TData : BaseEntityPolicyData
    {
        public Permission View { get; }
        public Permission Create { get; }
        public Permission Update { get; }
        public Permission Delete { get; }

        public BaseEntityPolicy()
        {
            View = permissionBuilder
                .AssertWith(ctx => ctx.Data.View)
                .WithName(nameof(View))
                .Build()
                ;

            Create = permissionBuilder
                .AssertWith(ctx => ctx.Data.Create)
                .WithName(nameof(Create))
                .Build()
                ;

            Update = permissionBuilder
                .AssertWith(ctx => ctx.Data.Update)
                .WithName(nameof(Update))
                .Build()
                ;

            Delete = permissionBuilder
                .AssertWith(ctx => ctx.Data.Delete)
                .WithName(nameof(Delete))
                .Build()
                ;
        }
    }
}
