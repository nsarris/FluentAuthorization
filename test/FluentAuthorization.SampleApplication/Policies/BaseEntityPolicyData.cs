namespace SampleApplication.Authorization.Policies
{
    public class BaseEntityPolicyData
    {
        public bool? View { get; init; }
        public bool? Create { get; init; }
        public bool? Update { get; init; }
        public bool? Delete { get; init; }

        public BaseEntityPolicyData(bool? create, bool? update, bool? delete, bool? view)
        {
            Create = create;
            Update = update;
            Delete = delete;
            View = view;
        }
    }
}
