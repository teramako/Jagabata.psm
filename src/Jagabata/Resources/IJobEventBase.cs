namespace Jagabata.Resources
{
    public interface IJobEventBase
    {
        DateTime Created { get; }
        DateTime? Modified { get; }
        JobEventEvent Event { get; }
        int Counter { get; }
        string EventDisplay { get; }
        Dictionary<string, object?> EventData { get; }
        bool Failed { get; }
        bool Changed { get; }
        string UUID { get; }
        string Stdout { get; }
        int StartLine { get; }
        int EndLine { get; }
        JobVerbosity Verbosity { get; }
    }
}
