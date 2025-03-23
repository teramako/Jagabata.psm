namespace Jagabata.Resources
{
    public class InventoryUpdateJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                         SummaryFieldsDictionary summaryFields, DateTime created,
                                         DateTime? modified, JobEventEvent @event, int counter, string eventDisplay,
                                         Dictionary<string, object?> eventData, bool failed, bool changed, string uuid,
                                         string stdout, int startLine, int endLine, JobVerbosity verbosity,
                                         ulong inventoryUpdate)
        : JobEventBase
    {
        /// <summary>
        /// List Inventory Update Events for an Inventory Update.<br/>
        /// API Path: <c>/api/v2/inventory_updates/<paramref name="inventoryUpdateJobId"/>/events/</c>
        /// </summary>
        /// <param name="inventoryUpdateJobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventoryUpdateJobEvent> FindFromInventoryUpdateJob(ulong inventoryUpdateJobId,
                                                                                                 HttpQuery? query = null)
        {
            var path = $"{InventoryUpdateJobBase.PATH}{inventoryUpdateJobId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventoryUpdateJobEvent>(path, query))
            {
                foreach (var jobEvent in result.Contents.Results)
                {
                    yield return jobEvent;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public override JobEventEvent Event { get; } = @event;
        public override int Counter { get; } = counter;
        public override string EventDisplay { get; } = eventDisplay;
        public override Dictionary<string, object?> EventData { get; } = eventData;
        public override bool Failed { get; } = failed;
        public override bool Changed { get; } = changed;
        public override string UUID { get; } = uuid;
        public override string Stdout { get; } = stdout;
        public override int StartLine { get; } = startLine;
        public override int EndLine { get; } = endLine;
        public override JobVerbosity Verbosity { get; } = verbosity;
        public ulong InventoryUpdate { get; } = inventoryUpdate;
    }
}
