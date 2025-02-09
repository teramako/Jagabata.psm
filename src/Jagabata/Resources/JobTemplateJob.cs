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
        : UnifiedJob(id, type, url, created, modified, name, launchType, status, executionEnvironment, failed,
                     started, finished, canceledOn, elapsed, jobExplanation, launchedBy, workUnitId),
          IJobTemplateJob, IResource, ICacheableResource
    {
        public new const string PATH = "/api/v2/jobs/";
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<JobTemplateJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<JobTemplateJob>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobTemplateJob> FindFromJobTemplate(ulong jobTemplateId,
                                                                                 NameValueCollection? query = null,
                                                                                 bool getAll = false)
        {
            var path = $"{Resources.JobTemplate.PATH}{jobTemplateId}/jobs/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobTemplateJob>(path, query, getAll))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }

        public RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

        public string Description { get; } = description;
        public ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public string ExecutionNode { get; } = executionNode;
        public string ControllerNode { get; } = controllerNode;

        #region JobTemplateJob Properties
        public JobType JobType { get; } = jobType;
        public ulong Inventory { get; } = inventory;
        public ulong Project { get; } = project;
        public string Playbook { get; } = playbook;
        public string ScmBranch { get; } = scmBranch;
        public byte Forks { get; } = forks;
        public string Limit { get; } = limit;
        public JobVerbosity Verbosity { get; } = verbosity;
        public string ExtraVars { get; } = extraVars;
        public string JobTags { get; } = jobTags;
        public bool ForceHandlers { get; } = forceHandlers;
        public string SkipTags { get; } = skipTags;
        public string StartAtTask { get; } = startAtTask;
        public ushort Timeout { get; } = timeout;
        public bool UseFactCache { get; } = useFactCache;
        public ulong Organization { get; } = organization;
        public ulong JobTemplate { get; } = jobTemplate;
        public string[] PasswordsNeededToStart { get; } = passwordsNeededToStart;
        public bool AllowSimultaneous { get; } = allowSimultaneous;
        public OrderedDictionary Artifacts { get; } = artifacts;
        public string ScmRevision { get; } = scmRevision;
        public ulong? InstanceGroup { get; } = instanceGroup;
        public bool DiffMode { get; } = diffMode;
        public int JobSliceNumber { get; } = jobSliceNumber;
        public int JobSliceCount { get; } = jobSliceCount;
        public string WebhookService { get; } = webhookService;
        public uint? WebhookCredential { get; } = webhookCredential;
        public string WebhookGuid { get; } = webhookGuid;
        #endregion

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        public CacheItem GetCacheItem()
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
            : JobTemplateJob(id, type, url, related, summaryFields, created, modified, name, description,
                             unifiedJobTemplate, launchType, status, executionEnvironment, failed, started,
                             finished, canceledOn, elapsed, jobExplanation, executionNode, controllerNode,
                             launchedBy, workUnitId, jobType, inventory, project, playbook, scmBranch, forks,
                             limit, verbosity, extraVars, jobTags, forceHandlers, skipTags, startAtTask, timeout,
                             useFactCache, organization, jobTemplate, passwordsNeededToStart, allowSimultaneous,
                             artifacts, scmRevision, instanceGroup, diffMode, jobSliceNumber, jobSliceCount,
                             webhookService, webhookCredential, webhookGuid),
               IJobTemplateJob, IJobDetail, IResource
        {

            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;


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
            : JobTemplateJob(id, type, url, related, summaryFields, created, modified, name, description,
                             unifiedJobTemplate, launchType, status, executionEnvironment, failed, started,
                             finished, canceledOn, elapsed, jobExplanation, executionNode, controllerNode,
                             launchedBy, workUnitId, jobType, inventory, project, playbook, scmBranch, forks,
                             limit, verbosity, extraVars, jobTags, forceHandlers, skipTags, startAtTask, timeout,
                             useFactCache, organization, jobTemplate, passwordsNeededToStart, allowSimultaneous,
                             artifacts, scmRevision, instanceGroup, diffMode, jobSliceNumber, jobSliceCount,
                             webhookService, webhookCredential, webhookGuid),
               IJobTemplateJob, IJobDetail, IResource
        {
            public ulong Job { get; } = job;
            public Dictionary<string, object?> IgnoredFields { get; } = ignoredFields;

            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
        }
    }
}

