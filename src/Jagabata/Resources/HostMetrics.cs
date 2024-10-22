namespace Jagabata.Resources
{
    public class HostMetric(ulong id, string hostname, string url, DateTime? firstAutomation, DateTime? lastAutomation,
                             DateTime? lastDeleted, int automatedCounter, int deletedCounter, bool deleted,
                             int? usedInInventories)
    {
        public const string PATH = "/api/v2/host_metrics/";

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = ResourceType.HostMetrics;
        public string Hostname { get; } = hostname;
        public string Url { get; } = url;
        public DateTime? FirstAutomation { get; } = firstAutomation;
        public DateTime? LastAutomation { get; } = lastAutomation;
        public DateTime? LastDeleted { get; } = lastDeleted;
        public int AutomatedCounter { get; } = automatedCounter;
        public int DeletedCounter { get; } = deletedCounter;
        public bool Deleted { get; } = deleted;
        public int? UsedInInventories { get; } = usedInInventories;
    }
}
