namespace SampleApplication.Authorization
{
    public class RecordPolicyData
    {
        public RecordPolicyData(bool view, bool create, bool update, bool delete)
        {
            View = view;
            Create = create;
            Update = update;
            Delete = delete;
        }

        public bool View { get;  }
        public bool Create { get; }
        public bool Update { get;  }
        public bool Delete { get; }
    }
}
