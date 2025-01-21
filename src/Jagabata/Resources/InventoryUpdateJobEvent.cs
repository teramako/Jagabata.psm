using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public class InventoryUpdateJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                         SummaryFieldsDictionary summaryFields, DateTime created,
                                         DateTime? modified, JobEventEvent @event, int counter, string eventDisplay,
                                         Dictionary<string, object?> eventData, bool failed, bool changed, string uuid,
                                         string stdout, int startLine, int endLine, JobVerbosity verbosity,
                                         ulong inventoryUpdate)
        : IJobEventBase, IResource, ICacheableResource
    {
        /// <summary>
        /// List Inventory Update Events for an Inventory Update.<br/>
        /// API Path: <c>/api/v2/inventory_updates/<paramref name="inventoryUpdateJobId"/>/events/</c>
        /// </summary>
        /// <param name="inventoryUpdateJobId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventoryUpdateJobEvent> FindFromInventoryUpdateJob(ulong inventoryUpdateJobId,
                                                                                  NameValueCollection? query = null,
                                                                                  bool getAll = false)
        {
            var path = $"{InventoryUpdateJob.PATH}{inventoryUpdateJobId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventoryUpdateJobEvent>(path, query, getAll))
            {
                foreach (var jobEvent in result.Contents.Results)
                {
                    yield return jobEvent;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public JobEventEvent Event { get; } = @event;
        public int Counter { get; } = counter;
        public string EventDisplay { get; } = eventDisplay;
        public Dictionary<string, object?> EventData { get; } = eventData;
        public bool Failed { get; } = failed;
        public bool Changed { get; } = changed;
        public string UUID { get; } = uuid;
        public string Stdout { get; } = stdout;
        public int StartLine { get; } = startLine;
        public int EndLine { get; } = endLine;
        public JobVerbosity Verbosity { get; } = verbosity;
        public ulong InventoryUpdate { get; } = inventoryUpdate;

        public string GetDescription()
        {
            return $"[{ResourceType.InventoryUpdate}:{InventoryUpdate}] {Counter}:{StartLine}:{EndLine} {Event}";
        }
    }
}
