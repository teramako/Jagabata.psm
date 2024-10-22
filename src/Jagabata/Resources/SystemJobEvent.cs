using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public class SystemJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                SystemJobEvent.Summary summaryFields, DateTime created, DateTime? modified,
                                JobEventEvent @event, int counter, string eventDisplay, Dictionary<string, object?> eventData,
                                bool failed, bool changed, string uuid, string stdout, int startLine, int endLine,
                                JobVerbosity verbosity, ulong systemJob)
        : IJobEventBase, IResource<SystemJobEvent.Summary>
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

        public record Summary();
        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;
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
        public ulong SystemJob { get; } = systemJob;
    }
}
