using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IJobEvent : IJobEventBase
    {
        int EventLevel { get; }
        string ParentUUID { get; }
        ulong? Host { get; }
        string HostName { get; }
        string Playbook { get; }
        string Play { get; }
        string Task { get; }
        string Role { get; }
        ulong Job { get; }
    }

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
                : IJobEvent, IResource
    {
        public const string PATH = "/api/v2/job_events/";

        /// <summary>
        /// List Job Events for a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="jobId"/>/job_events/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromJob(ulong jobId,
                                                                   NameValueCollection? query = null,
                                                                   bool getAll = false)
        {
            var path = $"{JobTemplateJob.PATH}{jobId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromGroup(ulong groupId,
                                                                     NameValueCollection? query = null,
                                                                     bool getAll = false)
        {
            var path = $"{Group.PATH}{groupId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobEvent> FindFromHost(ulong hostId,
                                                                    NameValueCollection? query = null,
                                                                    bool getAll = false)
        {
            var path = $"{Resources.Host.PATH}{hostId}/job_events/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobEvent>(path, query, getAll))
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
        [JsonConverter(typeof(Json.SummaryFieldsJobEventConverter))]
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public ulong Job { get; } = job;
        public JobEventEvent Event { get; } = @event;
        public int Counter { get; } = counter;
        public string EventDisplay { get; } = eventDisplay;
        public Dictionary<string, object?> EventData { get; } = eventData;
        public int EventLevel { get; } = eventLevel;
        public bool Failed { get; } = failed;
        public bool Changed { get; } = changed;
        public string UUID { get; } = uuid;
        public string ParentUUID { get; } = parentUUID;
        public ulong? Host { get; } = host;
        public string HostName { get; } = hostName;
        public string Playbook { get; } = playbook;
        public string Play { get; } = play;
        public string Task { get; } = task;
        public string Role { get; } = role;
        public string Stdout { get; } = stdout;
        public int StartLine { get; } = startLine;
        public int EndLine { get; } = endLine;
        public JobVerbosity Verbosity { get; } = verbosity;
    }
}
