namespace FluentAuthorization
{
    public interface ISecurityPolicy 
    {
        string Name { get; }
        string Source { get; }
        ISecurityPolicy Merge(ISecurityPolicy next);
    }

    public interface ISecurityPolicyWithData : ISecurityPolicy
    {
        PolicyData Data { get; }
    }

    public interface ISecurityPolicy<T> : ISecurityPolicyWithData
        where T : PolicyData
    {
        new T Data { get; }
    }
}
