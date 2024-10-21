namespace Jagabata.Resources
{
    public interface IJobDetail
    {
        string JobArgs { get; }
        string JobCwd { get; }
        Dictionary<string, string> JobEnv { get; }
        string ResultTraceback { get; }
    }
}

