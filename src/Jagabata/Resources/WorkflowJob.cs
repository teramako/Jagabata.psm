using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IWorkflowJob : IUnifiedJob
    {
        string Description { get; }
        ulong UnifiedJobTemplate { get; }
        ulong? WorkflowJobTemplate { get; }
        string ExtraVars { get; }
        bool AllowSimultaneous { get; }
        /// <summary>
        /// If automatically created for a sliced job run, the job template the workflow job was created from.
        /// </summary>
        ulong? JobTemplate { get; }
        bool IsSlicedJob { get; }
        /// <summary>
        /// Inventory applied as a prompt, assuming job template prompts for inventory.
        /// </summary>
        ulong? Inventory { get; }
        string? Limit { get; }
        string? ScmBranch { get; }
        string WebhookService { get; }
        ulong? WebhookCredential { get; }
        string WebhookGuid { get; }
        string? SkipTags { get; }
        string? JobTags { get; }

        /// <summary>
        /// Deseriaze string <see cref="ExtraVars">ExtraVars</see>(JSON or YAML) to Dictionary
        /// </summary>
        /// <returns>result of deserialized <see cref="ExtraVars"/> to Dictionary</returns>
        Dictionary<string, object?> GetExtraVars();
    }

    public abstract class WorkflowJobBase : UnifiedJob, IWorkflowJob, ICacheableResource
    {
        public new const string PATH = "/api/v2/workflow_jobs/";

        public abstract string Description { get; }
        public abstract ulong UnifiedJobTemplate { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract ulong? WorkflowJobTemplate { get; }
        public abstract string ExtraVars { get; }
        public abstract bool AllowSimultaneous { get; }
        public abstract ulong? JobTemplate { get; }
        public abstract bool IsSlicedJob { get; }
        public abstract ulong? Inventory { get; }
        public abstract string? Limit { get; }
        public abstract string? ScmBranch { get; }
        public abstract string WebhookService { get; }
        public abstract ulong? WebhookCredential { get; }
        public abstract string WebhookGuid { get; }
        public abstract string? SkipTags { get; }
        public abstract string? JobTags { get; }

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
    }

    public class WorkflowJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                             SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                             string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                             bool failed, DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                             string jobExplanation, LaunchedBy launchedBy, string? workUnitId,
                             ulong? workflowJobTemplate, string extraVars, bool allowSimultaneous, ulong? jobTemplate,
                             bool isSlicedJob, ulong? inventory, string? limit, string? scmBranch, string webhookService,
                             ulong? webhookCredential, string webhookGuid, string? skipTags, string? jobTags)
        : WorkflowJobBase
    {
        /// <summary>
        /// Retrieve a Workflow Job.<br/>
        /// API Path: <c>/api/v2/workflow_jobs/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<WorkflowJob> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJob>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Workflow Jobs.<br/>
        /// API Path: <c>/api/v2/workflow_jobs/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<WorkflowJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJob>(PATH, query, getAll))
            {
                foreach (var workflowJob in result.Contents.Results)
                {
                    yield return workflowJob;
                }
            }
        }
        /// <summary>
        /// List Workflow Jobs for a Workflow Job Templates.<br/>
        /// API Path: <c>/api/v2/workflow_job_templates/<paramref name="wjtId"/>/workflow_jobs/</c>
        /// </summary>
        /// <param name="wjtId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJob> FindFromWorkflowJobTemplate(ulong wjtId,
                                                                                      NameValueCollection? query = null,
                                                                                      bool getAll = false)
        {
            var path = $"{Resources.WorkflowJobTemplate.PATH}{wjtId}/workflow_jobs/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJob>(path, query, getAll))
            {
                foreach (var workflowJob in result.Contents.Results)
                {
                    yield return workflowJob;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public override string Description { get; } = description;
        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public override string Name { get; } = name;
        public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public override JobLaunchType LaunchType { get; } = launchType;
        public override JobStatus Status { get; } = status;
        public override bool Failed { get; } = failed;
        public override DateTime? Started { get; } = started;
        public override DateTime? Finished { get; } = finished;
        public override DateTime? CanceledOn { get; } = canceledOn;
        public override double Elapsed { get; } = elapsed;
        public override string JobExplanation { get; } = jobExplanation;
        public override LaunchedBy LaunchedBy { get; } = launchedBy;
        public override string? WorkUnitId { get; } = workUnitId;
        public override ulong? WorkflowJobTemplate { get; } = workflowJobTemplate;
        public override string ExtraVars { get; } = extraVars;
        public override bool AllowSimultaneous { get; } = allowSimultaneous;
        public override ulong? JobTemplate { get; } = jobTemplate;
        public override bool IsSlicedJob { get; } = isSlicedJob;
        public override ulong? Inventory { get; } = inventory;
        public override string? Limit { get; } = limit;
        public override string? ScmBranch { get; } = scmBranch;
        public override string WebhookService { get; } = webhookService;
        public override ulong? WebhookCredential { get; } = webhookCredential;
        public override string WebhookGuid { get; } = webhookGuid;
        public override string? SkipTags { get; } = skipTags;
        public override string? JobTags { get; } = jobTags;

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                            bool failed, DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                            string jobArgs, string jobCwd, Dictionary<string, string> jobEnv, string jobExplanation,
                            string resultTraceback, LaunchedBy launchedBy, string? workUnitId,
                            ulong? workflowJobTemplate, string extraVars, bool allowSimultaneous, ulong? jobTemplate,
                            bool isSlicedJob, ulong? inventory, string? limit, string? scmBranch, string webhookService,
                            ulong? webhookCredential, string webhookGuid, string? skipTags, string? jobTags)
            : WorkflowJobBase, IJobDetail
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override string Description { get; } = description;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
            public override JobLaunchType LaunchType { get; } = launchType;
            public override JobStatus Status { get; } = status;
            public override bool Failed { get; } = failed;
            public override DateTime? Started { get; } = started;
            public override DateTime? Finished { get; } = finished;
            public override DateTime? CanceledOn { get; } = canceledOn;
            public override double Elapsed { get; } = elapsed;
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public override string JobExplanation { get; } = jobExplanation;
            public string ResultTraceback { get; } = resultTraceback;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override ulong? WorkflowJobTemplate { get; } = workflowJobTemplate;
            public override string ExtraVars { get; } = extraVars;
            public override bool AllowSimultaneous { get; } = allowSimultaneous;
            public override ulong? JobTemplate { get; } = jobTemplate;
            public override bool IsSlicedJob { get; } = isSlicedJob;
            public override ulong? Inventory { get; } = inventory;
            public override string? Limit { get; } = limit;
            public override string? ScmBranch { get; } = scmBranch;
            public override string WebhookService { get; } = webhookService;
            public override ulong? WebhookCredential { get; } = webhookCredential;
            public override string WebhookGuid { get; } = webhookGuid;
            public override string? SkipTags { get; } = skipTags;
            public override string? JobTags { get; } = jobTags;
        }

        public class LaunchResult(ulong workflowJob, Dictionary<string, object?> ignoredFields, ulong id,
                                  ResourceType type, string url, RelatedDictionary related,
                                  SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                  string name, string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, bool failed, DateTime? started, DateTime? finished,
                                  DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                                  Dictionary<string, string> jobEnv, string jobExplanation, string resultTraceback,
                                  LaunchedBy launchedBy, string? workUnitId, ulong? workflowJobTemplate,
                                  string extraVars, bool allowSimultaneous, ulong? jobTemplate, bool isSlicedJob,
                                  ulong? inventory, string? limit, string? scmBranch, string webhookService,
                                  ulong? webhookCredential, string webhookGuid, string? skipTags, string? jobTags)
            : Detail(id, type, url, related, summaryFields, created, modified, name, description, unifiedJobTemplate,
                     launchType, status, failed, started, finished, canceledOn, elapsed, jobArgs, jobCwd, jobEnv,
                     jobExplanation, resultTraceback, launchedBy, workUnitId, workflowJobTemplate, extraVars,
                     allowSimultaneous, jobTemplate, isSlicedJob, inventory, limit, scmBranch, webhookService,
                     webhookCredential, webhookGuid, skipTags, jobTags)
        {
            public ulong WorkflowJob { get; } = workflowJob;
            public Dictionary<string, object?> IgnoredFields { get; } = ignoredFields;
        }
    }
}
