using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IJobTemplateJob : IUnifiedJob
    {
        string Description { get; }
        ulong UnifiedJobTemplate { get; }
        string ExecutionNode { get; }
        string ControllerNode { get; }
        JobType JobType { get; }
        ulong Inventory { get; }
        ulong Project { get; }
        string Playbook { get; }
        string ScmBranch { get; }
        byte Forks { get; }
        string Limit { get; }
        JobVerbosity Verbosity { get; }
        string ExtraVars { get; }
        string JobTags { get; }
        bool ForceHandlers { get; }
        string SkipTags { get; }
        string StartAtTask { get; }
        ushort Timeout { get; }
        bool UseFactCache { get; }
        ulong Organization { get; }
        ulong JobTemplate { get; }
        string[] PasswordsNeededToStart { get; }
        bool AllowSimultaneous { get; }
        OrderedDictionary Artifacts { get; }
        string ScmRevision { get; }
        ulong? InstanceGroup { get; }
        bool DiffMode { get; }
        int JobSliceNumber { get; }
        int JobSliceCount { get; }
        string WebhookService { get; }
        uint? WebhookCredential { get; }
        string WebhookGuid { get; }

        /// <summary>
        /// Deseriaze string <see cref="ExtraVars">ExtraVars</see>(JSON or YAML) to Dictionary
        /// </summary>
        /// <returns>result of deserialized <see cref="ExtraVars"/> to Dictionary</returns>
        Dictionary<string, object?> GetExtraVars();
    }

    public abstract class JobTemplateJobBase : UnifiedJob, IJobTemplateJob
    {
        public new const string PATH = "/api/v2/jobs/";

        public abstract string Description { get; }
        public abstract ulong UnifiedJobTemplate { get; }
        public abstract ulong? ExecutionEnvironment { get; }
        public abstract string ExecutionNode { get; }
        public abstract string ControllerNode { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract JobType JobType { get; }
        public abstract ulong Inventory { get; }
        public abstract ulong Project { get; }
        public abstract string Playbook { get; }
        public abstract string ScmBranch { get; }
        public abstract byte Forks { get; }
        public abstract string Limit { get; }
        public abstract JobVerbosity Verbosity { get; }
        public abstract string ExtraVars { get; }
        public abstract string JobTags { get; }
        public abstract bool ForceHandlers { get; }
        public abstract string SkipTags { get; }
        public abstract string StartAtTask { get; }
        public abstract ushort Timeout { get; }
        public abstract bool UseFactCache { get; }
        public abstract ulong Organization { get; }
        public abstract ulong JobTemplate { get; }
        public abstract string[] PasswordsNeededToStart { get; }
        public abstract bool AllowSimultaneous { get; }
        public abstract OrderedDictionary Artifacts { get; }
        public abstract string ScmRevision { get; }
        public abstract ulong? InstanceGroup { get; }
        public abstract bool DiffMode { get; }
        public abstract int JobSliceNumber { get; }
        public abstract int JobSliceCount { get; }
        public abstract string WebhookService { get; }
        public abstract uint? WebhookCredential { get; }
        public abstract string WebhookGuid { get; }

        public JobTemplate? GetTemplate()
        {
            return GetTemplate<JobTemplate>();
        }

        /// <summary>
        /// Get job events for this job
        /// </summary>
        public IEnumerable<JobEvent> GetEvents()
        {
            return GetEvents<JobEvent>("job_events");
        }

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/jobs/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(string? searchWords = null, ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", searchWords, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/jobs/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                    ["Finished"] = $"{Finished}",
                    ["Elapsed"] = $"{Elapsed}"
                }
            };
        }
    }

    public class JobTemplateJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                                SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                                string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                                ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                                DateTime? canceledOn, double elapsed, string jobExplanation, string executionNode,
                                string controllerNode, LaunchedBy launchedBy, string workUnitId, JobType jobType,
                                ulong inventory, ulong project, string playbook, string scmBranch, byte forks,
                                string limit, JobVerbosity verbosity, string extraVars, string jobTags,
                                bool forceHandlers, string skipTags, string startAtTask, ushort timeout,
                                bool useFactCache, ulong organization, ulong jobTemplate,
                                string[] passwordsNeededToStart, bool allowSimultaneous, OrderedDictionary artifacts,
                                string scmRevision, ulong? instanceGroup, bool diffMode, int jobSliceNumber,
                                int jobSliceCount, string webhookService, uint? webhookCredential, string webhookGuid)
        : JobTemplateJobBase
    {
        /// <summary>
        /// Retrieve a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<Detail> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Jobs.<br/>
        /// API Path: <c>/api/v2/jobs/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<JobTemplateJob> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<JobTemplateJob>(PATH, query))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }
        /// <summary>
        /// List Jobs for a Job Template.<br/>
        /// API Path: <c>/api/v2/job_templates/<paramref name="jobTemplateId"/>/jobs/</c>
        /// </summary>
        /// <param name="jobTemplateId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobTemplateJob> FindFromJobTemplate(ulong jobTemplateId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{Resources.JobTemplate.PATH}{jobTemplateId}/jobs/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobTemplateJob>(path, query))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
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
        public override string Name { get; } = name;
        public override string Description { get; } = description;
        public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public override JobLaunchType LaunchType { get; } = launchType;
        public override JobStatus Status { get; } = status;
        public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public override bool Failed { get; } = failed;
        public override DateTime? Started { get; } = started;
        public override DateTime? Finished { get; } = finished;
        public override DateTime? CanceledOn { get; } = canceledOn;
        public override double Elapsed { get; } = elapsed;
        public override string JobExplanation { get; } = jobExplanation;
        public override string ExecutionNode { get; } = executionNode;
        public override string ControllerNode { get; } = controllerNode;
        public override LaunchedBy LaunchedBy { get; } = launchedBy;
        public override string? WorkUnitId { get; } = workUnitId;
        public override JobType JobType { get; } = jobType;
        public override ulong Inventory { get; } = inventory;
        public override ulong Project { get; } = project;
        public override string Playbook { get; } = playbook;
        public override string ScmBranch { get; } = scmBranch;
        public override byte Forks { get; } = forks;
        public override string Limit { get; } = limit;
        public override JobVerbosity Verbosity { get; } = verbosity;
        public override string ExtraVars { get; } = extraVars;
        public override string JobTags { get; } = jobTags;
        public override bool ForceHandlers { get; } = forceHandlers;
        public override string SkipTags { get; } = skipTags;
        public override string StartAtTask { get; } = startAtTask;
        public override ushort Timeout { get; } = timeout;
        public override bool UseFactCache { get; } = useFactCache;
        public override ulong Organization { get; } = organization;
        public override ulong JobTemplate { get; } = jobTemplate;
        public override string[] PasswordsNeededToStart { get; } = passwordsNeededToStart;
        public override bool AllowSimultaneous { get; } = allowSimultaneous;
        public override OrderedDictionary Artifacts { get; } = artifacts;
        public override string ScmRevision { get; } = scmRevision;
        public override ulong? InstanceGroup { get; } = instanceGroup;
        public override bool DiffMode { get; } = diffMode;
        public override int JobSliceNumber { get; } = jobSliceNumber;
        public override int JobSliceCount { get; } = jobSliceCount;
        public override string WebhookService { get; } = webhookService;
        public override uint? WebhookCredential { get; } = webhookCredential;
        public override string WebhookGuid { get; } = webhookGuid;

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, JobType jobType, ulong inventory, ulong project, string playbook,
                            string scmBranch, byte forks, string limit, JobVerbosity verbosity, string extraVars,
                            string jobTags, bool forceHandlers, string skipTags, string startAtTask, ushort timeout,
                            bool useFactCache, ulong organization, ulong unifiedJobTemplate, JobLaunchType launchType,
                            JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                            DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string jobExplanation, string executionNode,
                            string controllerNode, string resultTraceback, bool eventProcessingFinished,
                            LaunchedBy launchedBy, string workUnitId, ulong jobTemplate, string[] passwordsNeededToStart,
                            bool allowSimultaneous, OrderedDictionary artifacts, string scmRevision,
                            ulong? instanceGroup, bool diffMode, int jobSliceNumber, int jobSliceCount,
                            string webhookService, uint? webhookCredential, string webhookGuid,
                            Dictionary<string, int> hostStatusCounts, Dictionary<string, int> playbookCounts,
                            string customVirtualenv)
            : JobTemplateJobBase, IJobDetail
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override string Description { get; } = description;
            public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
            public override JobLaunchType LaunchType { get; } = launchType;
            public override JobStatus Status { get; } = status;
            public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
            public override bool Failed { get; } = failed;
            public override DateTime? Started { get; } = started;
            public override DateTime? Finished { get; } = finished;
            public override DateTime? CanceledOn { get; } = canceledOn;
            public override double Elapsed { get; } = elapsed;
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public override string JobExplanation { get; } = jobExplanation;
            public override string ExecutionNode { get; } = executionNode;
            public override string ControllerNode { get; } = controllerNode;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override JobType JobType { get; } = jobType;
            public override ulong Inventory { get; } = inventory;
            public override ulong Project { get; } = project;
            public override string Playbook { get; } = playbook;
            public override string ScmBranch { get; } = scmBranch;
            public override byte Forks { get; } = forks;
            public override string Limit { get; } = limit;
            public override JobVerbosity Verbosity { get; } = verbosity;
            public override string ExtraVars { get; } = extraVars;
            public override string JobTags { get; } = jobTags;
            public override bool ForceHandlers { get; } = forceHandlers;
            public override string SkipTags { get; } = skipTags;
            public override string StartAtTask { get; } = startAtTask;
            public override ushort Timeout { get; } = timeout;
            public override bool UseFactCache { get; } = useFactCache;
            public override ulong Organization { get; } = organization;
            public override ulong JobTemplate { get; } = jobTemplate;
            public override string[] PasswordsNeededToStart { get; } = passwordsNeededToStart;
            public override bool AllowSimultaneous { get; } = allowSimultaneous;
            public override OrderedDictionary Artifacts { get; } = artifacts;
            public override string ScmRevision { get; } = scmRevision;
            public override ulong? InstanceGroup { get; } = instanceGroup;
            public override bool DiffMode { get; } = diffMode;
            public override int JobSliceNumber { get; } = jobSliceNumber;
            public override int JobSliceCount { get; } = jobSliceCount;
            public override string WebhookService { get; } = webhookService;
            public override uint? WebhookCredential { get; } = webhookCredential;
            public override string WebhookGuid { get; } = webhookGuid;
            public Dictionary<string, int> HostStatusCounts { get; } = hostStatusCounts;
            public Dictionary<string, int> PlaybookCounts { get; } = playbookCounts;
            public string? CustomVirtualenv { get; } = customVirtualenv;
        }

        public class LaunchResult(ulong job, Dictionary<string, object?> ignoredFields, ulong id, ResourceType type,
                                  string url, RelatedDictionary related, SummaryFieldsDictionary summaryFields, DateTime created,
                                  DateTime? modified, string name, string description, JobType jobType, ulong inventory,
                                  ulong project, string playbook, string scmBranch, byte forks, string limit,
                                  JobVerbosity verbosity, string extraVars, string jobTags, bool forceHandlers,
                                  string skipTags, string startAtTask, ushort timeout, bool useFactCache,
                                  ulong organization, ulong unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                                  DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs,
                                  string jobCwd, Dictionary<string, string> jobEnv, string jobExplanation,
                                  string executionNode, string controllerNode, string resultTraceback,
                                  bool eventProcessingFinished, LaunchedBy launchedBy, string workUnitId,
                                  ulong jobTemplate, string[] passwordsNeededToStart, bool allowSimultaneous,
                                  OrderedDictionary artifacts, string scmRevision, ulong? instanceGroup, bool diffMode,
                                  int jobSliceNumber, int jobSliceCount, string webhookService, uint? webhookCredential,
                                  string webhookGuid)
            : JobTemplateJobBase, IJobDetail
        {
            public ulong Job { get; } = job;
            public Dictionary<string, object?> IgnoredFields { get; } = ignoredFields;

            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override string Description { get; } = description;
            public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
            public override JobLaunchType LaunchType { get; } = launchType;
            public override JobStatus Status { get; } = status;
            public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
            public override bool Failed { get; } = failed;
            public override DateTime? Started { get; } = started;
            public override DateTime? Finished { get; } = finished;
            public override DateTime? CanceledOn { get; } = canceledOn;
            public override double Elapsed { get; } = elapsed;
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public override string JobExplanation { get; } = jobExplanation;
            public override string ExecutionNode { get; } = executionNode;
            public override string ControllerNode { get; } = controllerNode;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override JobType JobType { get; } = jobType;
            public override ulong Inventory { get; } = inventory;
            public override ulong Project { get; } = project;
            public override string Playbook { get; } = playbook;
            public override string ScmBranch { get; } = scmBranch;
            public override byte Forks { get; } = forks;
            public override string Limit { get; } = limit;
            public override JobVerbosity Verbosity { get; } = verbosity;
            public override string ExtraVars { get; } = extraVars;
            public override string JobTags { get; } = jobTags;
            public override bool ForceHandlers { get; } = forceHandlers;
            public override string SkipTags { get; } = skipTags;
            public override string StartAtTask { get; } = startAtTask;
            public override ushort Timeout { get; } = timeout;
            public override bool UseFactCache { get; } = useFactCache;
            public override ulong Organization { get; } = organization;
            public override ulong JobTemplate { get; } = jobTemplate;
            public override string[] PasswordsNeededToStart { get; } = passwordsNeededToStart;
            public override bool AllowSimultaneous { get; } = allowSimultaneous;
            public override OrderedDictionary Artifacts { get; } = artifacts;
            public override string ScmRevision { get; } = scmRevision;
            public override ulong? InstanceGroup { get; } = instanceGroup;
            public override bool DiffMode { get; } = diffMode;
            public override int JobSliceNumber { get; } = jobSliceNumber;
            public override int JobSliceCount { get; } = jobSliceCount;
            public override string WebhookService { get; } = webhookService;
            public override uint? WebhookCredential { get; } = webhookCredential;
            public override string WebhookGuid { get; } = webhookGuid;
        }
    }
}

