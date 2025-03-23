namespace Jagabata.Resources
{
    public class AdHocCommandJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      ulong adHocCommand, JobEventEvent @event, int counter, string eventDisplay,
                                      Dictionary<string, object?> eventData, bool failed, bool changed, string uuid, ulong? host,
                                      string hostName, string stdout, int startLine, int endLine, JobVerbosity verbosity)
        : JobEventBase
    {
        /// <summary>
        /// List Ad Hoc Command Events for an Ad Hoc Command.<br/>
        /// API Path: <c>/api/v2/ad_hoc_commands/<paramref name="adHocCommandId"/>/events/</c>
        /// </summary>
        /// <param name="adHocCommandId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommandJobEvent> FindFromAdHocCommand(ulong adHocCommandId,
                                                                                        HttpQuery? query = null)
        {
            var path = $"{AdHocCommandBase.PATH}{adHocCommandId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommandJobEvent>(path, query))
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
        public ulong AdHocCommand { get; } = adHocCommand;
        public override JobEventEvent Event { get; } = @event;
        public override int Counter { get; } = counter;
        public override string EventDisplay { get; } = eventDisplay;
        public override Dictionary<string, object?> EventData { get; } = eventData;
        public override bool Failed { get; } = failed;
        public override bool Changed { get; } = changed;
        public override string UUID { get; } = uuid;
        public ulong? Host { get; } = host;
        public string HostName { get; } = hostName;
        public override string Stdout { get; } = stdout;
        public override int StartLine { get; } = startLine;
        public override int EndLine { get; } = endLine;
        public override JobVerbosity Verbosity { get; } = verbosity;

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, $"{Counter}:{Event}")
            {
                Metadata = {
                    ["Hostname"] = HostName,
                    ["Failed"] = $"{Failed}",
                    ["Changed"] = $"{Changed}"
                }
            };
        }
    }
}
