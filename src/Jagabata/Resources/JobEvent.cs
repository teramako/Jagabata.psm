using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<JobEventEvent>))]
    public enum JobEventEvent
    {
        /// <summary>
        /// Host Failed
        /// </summary>
        RunnerOnFailed,
        /// <summary>
        /// Host Started
        /// </summary>
        RunnerOnStart,
        /// <summary>
        /// Host OK
        /// </summary>
        RunnerOnOK,
        /// <summary>
        /// Host Failure
        /// </summary>
        RunnerOnError,
        /// <summary>
        /// Host Skipped
        /// </summary>
        RunnerOnSkipped,
        /// <summary>
        /// Host Unreachable
        /// </summary>
        RunnerOnUnreachable,
        /// <summary>
        /// No Hosts Remaining
        /// </summary>
        RunnerOnNoHosts,
        /// <summary>
        /// Host Polling
        /// </summary>
        RunnerOnAsyncPoll,
        /// <summary>
        /// Host Async OK
        /// </summary>
        RunnerOnAsyncOK,
        /// <summary>
        /// Host Async Failure
        /// </summary>
        RunnerOnAsyncFailed,
        /// <summary>
        /// Item OK
        /// </summary>
        RunnerItemOnOK,
        /// <summary>
        /// Item Failed
        /// </summary>
        RunnerItemOnFailed,
        /// <summary>
        /// Item Skipped
        /// </summary>
        RunnerItemOnSkipped,
        /// <summary>
        /// Host Retry
        /// </summary>
        RunnerRetry,
        /// <summary>
        /// File Differerence
        /// </summary>
        RunnerOnFileDiff,
        /// <summary>
        /// Playbook Started
        /// </summary>
        PlaybookOnStart,
        /// <summary>
        /// Running Handlers
        /// </summary>
        PlaybookOnNotify,
        /// <summary>
        /// Including File
        /// </summary>
        PlaybookOnInclude,
        /// <summary>
        /// No Hosts Matched
        /// </summary>
        PlaybookOnNoHostsMatched,
        /// <summary>
        /// No Hosts Remaining
        /// </summary>
        PlaybookOnNoHostsRemaining,
        /// <summary>
        /// Task Started
        /// </summary>
        PlaybookOnTaskStart,
        /// <summary>
        /// Variables Prompted
        /// </summary>
        PlaybookOnVarsPrompt,
        /// <summary>
        /// Gathering Facts
        /// </summary>
        PlaybookOnSetup,
        /// <summary>
        /// Internal: on Import for Host
        /// </summary>
        PlaybookOnImportForHost,
        /// <summary>
        /// internal: on Not Import for Host
        /// </summary>
        PlaybookOnNotImportForHost,
        /// <summary>
        /// Play Started
        /// </summary>
        PlaybookOnPlayStart,
        /// <summary>
        /// Playbook Complete
        /// </summary>
        PlaybookOnStats,
        Debug,
        Verbose,
        Deprecated,
        Warning,
        SystemWarning,
        Error
    }


    public class JobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                          SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, ulong job,
                          JobEventEvent @event, int counter, string eventDisplay, Dictionary<string, object?> eventData,
                          int eventLevel, bool failed, bool changed, string uuid, string parentUUID, ulong? host,
                          string hostName, string playbook, string play, string task, string role, string stdout,
                          int startLine, int endLine, JobVerbosity verbosity)
        : JobEventBase
    {
        public const string PATH = "/api/v2/job_events/";

        /// <summary>
        /// List Job Events for a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="jobId"/>/job_events/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromJob(ulong jobId,
                                                                   HttpQuery? query = null)
        {
            var path = $"{JobTemplateJobBase.PATH}{jobId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query))
            {
                foreach (var jobEvent in result.Contents.Results)
                {
                    yield return jobEvent;
                }
            }
        }
        /// <summary>
        /// List Job Events for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/job_events/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromGroup(ulong groupId,
                                                                     HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query))
            {
                foreach (var jobEvent in result.Contents.Results)
                {
                    yield return jobEvent;
                }
            }
        }
        /// <summary>
        /// List Job Events for a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/job_events/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromHost(ulong hostId,
                                                                    HttpQuery? query = null)
        {
            var path = $"{Resources.Host.PATH}{hostId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query))
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
        [JsonConverter(typeof(Json.SummaryFieldsJobEventConverter))]
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public ulong Job { get; } = job;
        public override JobEventEvent Event { get; } = @event;
        public override int Counter { get; } = counter;
        public override string EventDisplay { get; } = eventDisplay;
        public override Dictionary<string, object?> EventData { get; } = eventData;
        public int EventLevel { get; } = eventLevel;
        public override bool Failed { get; } = failed;
        public override bool Changed { get; } = changed;
        public override string UUID { get; } = uuid;
        public string ParentUUID { get; } = parentUUID;
        public ulong? Host { get; } = host;
        public string HostName { get; } = hostName;
        public string Playbook { get; } = playbook;
        public string Play { get; } = play;
        public string Task { get; } = task;
        public string Role { get; } = role;
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
                    ["Play"] = Play,
                    ["Task"] = Task,
                    ["Failed"] = $"{Failed}",
                    ["Changed"] = $"{Changed}",
                    ["Job"] = SummaryFields.TryGetValue<JobExSummary>("Job", out var pu)
                              ? $"{pu.Type}:{pu.Id}:{pu.Name}"
                              : string.Empty
                }
            };
        }
    }
}
