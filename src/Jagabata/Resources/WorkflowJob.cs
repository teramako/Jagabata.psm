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


    public class WorkflowJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                             SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                             string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                             ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                             DateTime? canceledOn, double elapsed, string jobExplanation, LaunchedBy launchedBy,
                             string? workUnitId, ulong? workflowJobTemplate, string extraVars, bool allowSimultaneous,
                             ulong? jobTemplate, bool isSlicedJob, ulong? inventory, string? limit, string? scmBranch,
                             string webhookService, ulong? webhookCredential, string webhookGuid, string? skipTags,
                             string? jobTags)
        : UnifiedJob(id, type, url, created, modified, name, launchType, status, executionEnvironment, failed,
                     started, finished, canceledOn, elapsed, jobExplanation, launchedBy, workUnitId),
          IWorkflowJob, IResource
    {
        public new const string PATH = "/api/v2/workflow_jobs/";
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

        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public string Description { get; } = description;
        public ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public ulong? WorkflowJobTemplate { get; } = workflowJobTemplate;
        public string ExtraVars { get; } = extraVars;
        public bool AllowSimultaneous { get; } = allowSimultaneous;
        public ulong? JobTemplate { get; } = jobTemplate;
        public bool IsSlicedJob { get; } = isSlicedJob;
        public ulong? Inventory { get; } = inventory;
        public string? Limit { get; } = limit;
        public string? ScmBranch { get; } = scmBranch;
        public string WebhookService { get; } = webhookService;
        public ulong? WebhookCredential { get; } = webhookCredential;
        public string WebhookGuid { get; } = webhookGuid;
        public string? SkipTags { get; } = skipTags;
        public string? JobTags { get; } = jobTags;

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related, SummaryFieldsDictionary summaryFields,
                            DateTime created, DateTime? modified, string name, string description,
                            ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                            ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                            DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string jobExplanation, string resultTraceback,
                            LaunchedBy launchedBy, string? workUnitId, ulong? workflowJobTemplate, string extraVars,
                            bool allowSimultaneous, ulong? jobTemplate, bool isSlicedJob, ulong? inventory,
                            string? limit, string? scmBranch, string webhookService, ulong? webhookCredential,
                            string webhookGuid, string? skipTags, string? jobTags)
            : WorkflowJob(id, type, url, related, summaryFields, created, modified, name, description, unifiedJobTemplate,
                          launchType, status, executionEnvironment, failed, started, finished, canceledOn, elapsed,
                          jobExplanation, launchedBy, workUnitId, workflowJobTemplate, extraVars, allowSimultaneous,
                          jobTemplate, isSlicedJob, inventory, limit, scmBranch, webhookService, webhookCredential,
                          webhookGuid, skipTags, jobTags),
              IWorkflowJob, IJobDetail, IResource
        {
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
        }
        public class LaunchResult(ulong workflowJob, Dictionary<string, object?> ignoredFields, ulong id,
                                  ResourceType type, string url, RelatedDictionary related,
                                  SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                                  string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                                  DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs,
                                  string jobCwd, Dictionary<string, string> jobEnv, string jobExplanation,
                                  string resultTraceback, LaunchedBy launchedBy, string? workUnitId,
                                  ulong? workflowJobTemplate, string extraVars, bool allowSimultaneous,
                                  ulong? jobTemplate, bool isSlicedJob, ulong? inventory, string? limit,
                                  string? scmBranch, string webhookService, ulong? webhookCredential, string webhookGuid,
                                  string? skipTags, string? jobTags)
            : Detail(id, type, url, related, summaryFields, created, modified, name, description, unifiedJobTemplate,
                     launchType, status, executionEnvironment, failed, started, finished, canceledOn, elapsed, jobArgs,
                     jobCwd, jobEnv, jobExplanation, resultTraceback, launchedBy, workUnitId, workflowJobTemplate,
                     extraVars, allowSimultaneous, jobTemplate, isSlicedJob, inventory, limit, scmBranch,
                     webhookService, webhookCredential, webhookGuid, skipTags, jobTags),
              IWorkflowJob, IJobDetail, IResource

        {
            public ulong WorkflowJob { get; } = workflowJob;
            public Dictionary<string, object?> IgnoredFields { get; } = ignoredFields;
        }
    }
}
