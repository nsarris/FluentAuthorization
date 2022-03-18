namespace SampleApplication.Authorization
{
    public abstract class RecordPolicy<TResource, TPolicyData> : BasePolicy<TResource, TPolicyData>
        where TResource : RecordResource
        where TPolicyData : RecordPolicyData
    {

    }
}
