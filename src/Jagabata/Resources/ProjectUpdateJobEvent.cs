namespace Jagabata.Resources
{
    public class ProjectUpdateJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                       SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                       JobEventEvent @event, int counter, string eventDisplay,
                                       Dictionary<string, object?> eventData, int eventLevel, bool failed, bool changed,
                                       string uuid, string hostName, string playbook, string play, string task,
                                       string role, string stdout, int startLine, int endLine, JobVerbosity verbosity,
                                       ulong projectUpdate)
        : JobEventBase
    {
        /// <summary>
        /// List Project Update Events for a Project Update.<br/>
        /// API Path: <c>/api/v2/project_updates/<paramref name="projectUpdateJobId"/>/events/</c>
        /// </summary>
        /// <param name="projectUpdateJobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ProjectUpdateJobEvent> FindFromProjectUpdateJob(ulong projectUpdateJobId,
                                                                                             HttpQuery? query = null)
        {
            var path = $"{ProjectUpdateJobBase.PATH}{projectUpdateJobId}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<ProjectUpdateJobEvent>(path, query))
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
        public int EventLevel { get; } = eventLevel;
        public override bool Failed { get; } = failed;
        public override bool Changed { get; } = changed;
        public override string UUID { get; } = uuid;
        public string HostName { get; } = hostName;
        public string Playbook { get; } = playbook;
        public string Play { get; } = play;
        public string Task { get; } = task;
        public string Role { get; } = role;
        public override string Stdout { get; } = stdout;
        public override int StartLine { get; } = startLine;
        public override int EndLine { get; } = endLine;
        public override JobVerbosity Verbosity { get; } = verbosity;
        public ulong ProjectUpdate { get; } = projectUpdate;

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, $"{Counter}:{Event}")
            {
                Metadata = {
                    ["Play"] = Play,
                    ["Task"] = Task,
                    ["Failed"] = $"{Failed}",
                    ["Job"] = SummaryFields.TryGetValue<ProjectUpdateSummary>("ProjectUpdate", out var pu)
                              ? $"{pu.Type}:{pu.Id}:{pu.Name}"
                              : string.Empty
                }
            };
        }
    }
}
