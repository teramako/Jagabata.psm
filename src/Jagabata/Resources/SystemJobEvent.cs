using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public class SystemJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                JobEventEvent @event, int counter, string eventDisplay, Dictionary<string, object?> eventData,
                                bool failed, bool changed, string uuid, string stdout, int startLine, int endLine,
                                JobVerbosity verbosity, ulong systemJob)
        : JobEventBase
    {
        /// <summary>
        /// List Sytem Job Events for a System Job.<br/>
        /// API Path: <c>/api/v2/system_jobs/<paramref name="systemJobId"/>/events/</c>
        /// </summary>
        /// <param name="systemJobId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<SystemJobEvent> FindFromSystemJob(ulong systemJobId,
                                                                         NameValueCollection? query = null,
                                                                         bool getAll = false)
        {
            var path = $"{Resources.SystemJob.PATH}{systemJobId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJobEvent>(path, query, getAll))
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
        public ulong SystemJob { get; } = systemJob;
    }
}
