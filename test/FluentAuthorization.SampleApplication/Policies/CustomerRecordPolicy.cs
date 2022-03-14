namespace SampleApplication.Authorization.Policies
{
    public class CustomerRecordResource : RecordResource
    {
        public CustomerRecordResource(int id)
            : base(EntityTypeResource.Customer, id)
        {

        }
    }

    public class CustomerRecordPolicy : RecordPolicy<CustomerRecordResource, RecordPolicyData>
    {
        public Permission View { get; }
        public Permission Create { get; }
        public Permission Update { get; }
        public Permission Delete { get; }

        public CustomerRecordPolicy()
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
