using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IAdHocCommandJobEvent : IJobEventBase
    {
        ulong AdHocCommand { get; }
        ulong? Host { get; }
        string HostName { get; }
    }

    public class AdHocCommandJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      ulong adHocCommand, JobEventEvent @event, int counter, string eventDisplay,
                                      Dictionary<string, object?> eventData, bool failed, bool changed, string uuid, ulong? host,
                                      string hostName, string stdout, int startLine, int endLine, JobVerbosity verbosity)
        : SummaryFieldsContainer, IAdHocCommandJobEvent, IResource, ICacheableResource
    {
        /// <summary>
        /// List Ad Hoc Command Events for an Ad Hoc Command.<br/>
        /// API Path: <c>/api/v2/ad_hoc_commands/<paramref name="adHocCommandId"/>/events/</c>
        /// </summary>
        /// <param name="adHocCommandId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommandJobEvent> FindFromAdHocCommand(ulong adHocCommandId,
                                                                                     NameValueCollection? query = null,
                                                                                     bool getAll = false)
        {
            var path = $"{Resources.AdHocCommand.PATH}{adHocCommandId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommandJobEvent>(path, query, getAll))
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
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public ulong AdHocCommand { get; } = adHocCommand;
        public JobEventEvent Event { get; } = @event;
        public int Counter { get; } = counter;
        public string EventDisplay { get; } = eventDisplay;
        public Dictionary<string, object?> EventData { get; } = eventData;
        public bool Failed { get; } = failed;
        public bool Changed { get; } = changed;
        public string UUID { get; } = uuid;
        public ulong? Host { get; } = host;
        public string HostName { get; } = hostName;
        public string Stdout { get; } = stdout;
        public int StartLine { get; } = startLine;
        public int EndLine { get; } = endLine;
        public JobVerbosity Verbosity { get; } = verbosity;

        public CacheItem GetCacheItem()
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
