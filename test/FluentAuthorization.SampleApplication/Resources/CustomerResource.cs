namespace SampleApplication.Authorization
{
    public class CustomerResource : EntityTypeResource
    {
        public static CustomerResource Instance {get;} = new CustomerResource();

        public CustomerResource()
        {
            this.EntityType = EntityType.Customer;
        }
    }
}
