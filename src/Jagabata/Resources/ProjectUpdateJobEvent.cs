using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IProjectUpdateJobEvent : IJobEventBase
    {
        int EventLevel { get; }
        string HostName { get; }
        string Playbook { get; }
        string Play { get; }
        string Task { get; }
        string Role { get; }
        ulong ProjectUpdate { get; }
    }

    public class ProjectUpdateJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                       SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                       JobEventEvent @event, int counter, string eventDisplay,
                                       Dictionary<string, object?> eventData, int eventLevel, bool failed, bool changed,
                                       string uuid, string hostName, string playbook, string play, string task,
                                       string role, string stdout, int startLine, int endLine, JobVerbosity verbosity,
                                       ulong projectUpdate)
        : SummaryFieldsContainer, IProjectUpdateJobEvent, IResource, ICacheableResource
    {
        /// <summary>
        /// List Project Update Events for a Project Update.<br/>
        /// API Path: <c>/api/v2/project_updates/<paramref name="projectUpdateJobId"/>/events/</c>
        /// </summary>
        /// <param name="projectUpdateJobId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ProjectUpdateJobEvent> FindFromProjectUpdateJob(ulong projectUpdateJobId,
                                                                                NameValueCollection? query = null,
                                                                                bool getAll = false)
        {
            var path = $"{ProjectUpdateJobBase.PATH}{projectUpdateJobId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<ProjectUpdateJobEvent>(path, query, getAll))
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
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public JobEventEvent Event { get; } = @event;
        public int Counter { get; } = counter;
        public string EventDisplay { get; } = eventDisplay;
        public Dictionary<string, object?> EventData { get; } = eventData;
        public int EventLevel { get; } = eventLevel;
        public bool Failed { get; } = failed;
        public bool Changed { get; } = changed;
        public string UUID { get; } = uuid;
        public string HostName { get; } = hostName;
        public string Playbook { get; } = playbook;
        public string Play { get; } = play;
        public string Task { get; } = task;
        public string Role { get; } = role;
        public string Stdout { get; } = stdout;
        public int StartLine { get; } = startLine;
        public int EndLine { get; } = endLine;
        public JobVerbosity Verbosity { get; } = verbosity;
        public ulong ProjectUpdate { get; } = projectUpdate;

        public CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, $"{Counter}:{Event}")
            {
                Metadata = {
                    ["Play"] = Play,
                    ["Task"] = Task,
                    ["Failed"] = $"{Failed}"
                }
            };
        }
    }
}
