using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IWorkflowJobTemplate
    {
        /// <summary>
        /// Name of this workflow job template.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this workflow job template.
        /// </summary>
        string Description { get; }
        string ExtraVars { get; }
        /// <summary>
        /// The organization used to determine access to this template.
        /// </summary>
        ulong? Organization { get; }
        bool SurveyEnabled { get; }
        bool AllowSimultaneous { get; }
        bool AskVariablesOnLaunch { get; }
        /// <summary>
        /// Inventory applied as a prompt, assuming job template prompts for inventory.
        /// </summary>
        ulong? Inventory { get; }
        string? Limit { get; }
        string? ScmBranch { get; }
        bool AskInventoryOnLaunch { get; }
        bool AskScmBranchOnLaunch { get; }
        bool AskLimitOnLaunch { get; }
        /// <summary>
        /// Service that webhook requests will be accepted from.
        /// </summary>
        string WebhookService { get; }
        /// <summary>
        /// Personal Access Token for posting back the status to the service API.
        /// </summary>
        ulong? WebhookCredential { get; }
        bool AskLabelsOnLaunch { get; }
        bool AskSkipTagsOnLaunch { get; }
        bool AskTagsOnLaunch { get; }
        string? SkipTags { get; }
        string? JobTags { get; }

        /// <summary>
        /// Deseriaze string <see cref="ExtraVars">ExtraVars</see>(JSON or YAML) to Dictionary
        /// </summary>
        /// <returns>result of deserialized <see cref="ExtraVars"/> to Dictionary</returns>
        Dictionary<string, object?> GetExtraVars();
    }

    public class WorkflowJobTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                     SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                     string name, string description, DateTime? lastJobRun, bool lastJobFailed,
                                     DateTime? nextJobRun, JobTemplateStatus status, string extraVars,
                                     ulong? organization, bool surveyEnabled, bool allowSimultaneous,
                                     bool askVariablesOnLaunch, ulong? inventory, string? limit, string? scmBranch,
                                     bool askInventoryOnLaunch, bool askScmBranchOnLaunch, bool askLimitOnLaunch,
                                     string webhookService, ulong? webhookCredential, bool askLabelsOnLaunch,
                                     bool askSkipTagsOnLaunch, bool askTagsOnLaunch, string? skipTags, string? jobTags)
        : UnifiedJobTemplate, IWorkflowJobTemplate
    {
        public new const string PATH = "/api/v2/workflow_job_templates/";
        /// <summary>
        /// Retrieve a Workflow Job Template.<br/>
        /// API Path: <c>/api/v2/workflow_job_templates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<WorkflowJobTemplate> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobTemplate>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Workflow Job Templates.<br/>
        /// API Path: <c>/api/v2/workflow_job_templates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<WorkflowJobTemplate> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplate>(PATH, query, getAll))
            {
                foreach (var jobTemplate in result.Contents.Results)
                {
                    yield return jobTemplate;
                }
            }
        }
        /// <summary>
        /// List Workflow Job Templates for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/workflow_job_templates/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplate> FindFromOrganization(ulong organizationId,
                                                                                       NameValueCollection? query = null,
                                                                                       bool getAll = false)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/workflow_job_templates/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplate>(path, query, getAll))
            {
                foreach (var jobTemplate in result.Contents.Results)
                {
                    yield return jobTemplate;
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
        public override DateTime? LastJobRun { get; } = lastJobRun;
        public override bool LastJobFailed { get; } = lastJobFailed;
        public override DateTime? NextJobRun { get; } = nextJobRun;
        public override JobTemplateStatus Status { get; } = status;
        public string ExtraVars { get; } = extraVars;
        public ulong? Organization { get; } = organization;
        public bool SurveyEnabled { get; } = surveyEnabled;
        public bool AllowSimultaneous { get; } = allowSimultaneous;
        public bool AskVariablesOnLaunch { get; } = askVariablesOnLaunch;
        public ulong? Inventory { get; } = inventory;
        public string? Limit { get; } = limit;
        public string? ScmBranch { get; } = scmBranch;
        public bool AskInventoryOnLaunch { get; } = askInventoryOnLaunch;
        public bool AskScmBranchOnLaunch { get; } = askScmBranchOnLaunch;
        public bool AskLimitOnLaunch { get; } = askLimitOnLaunch;
        public string WebhookService { get; } = webhookService;
        public ulong? WebhookCredential { get; } = webhookCredential;
        public bool AskLabelsOnLaunch { get; } = askLabelsOnLaunch;
        public bool AskSkipTagsOnLaunch { get; } = askSkipTagsOnLaunch;
        public bool AskTagsOnLaunch { get; } = askTagsOnLaunch;
        public string? SkipTags { get; } = skipTags;
        public string? JobTags { get; } = jobTags;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public JobTemplateAskOnLaunch AskOnLaunch => (AskInventoryOnLaunch ? JobTemplateAskOnLaunch.Inventory : 0) |
                       (AskScmBranchOnLaunch ? JobTemplateAskOnLaunch.ScmBranch : 0) |
                       (AskLabelsOnLaunch ? JobTemplateAskOnLaunch.Labels : 0) |
                       (AskVariablesOnLaunch ? JobTemplateAskOnLaunch.Variables : 0) |
                       (AskLimitOnLaunch ? JobTemplateAskOnLaunch.Limit : 0) |
                       (AskTagsOnLaunch ? JobTemplateAskOnLaunch.JobTags : 0) |
                       (AskSkipTagsOnLaunch ? JobTemplateAskOnLaunch.SkipTags : 0);
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public JobTemplateOptions Options => (SurveyEnabled ? JobTemplateOptions.Survey : 0) |
                       (!string.IsNullOrEmpty(WebhookService) ? JobTemplateOptions.Webhook : 0) |
                       (AllowSimultaneous ? JobTemplateOptions.Simultaneous : 0);

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        /// <summary>
        /// Get the most recently executed workflow jobs.
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/workflow_jobs/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public WorkflowJob[] GetRecentJobs(int count = 20)
        {
            // don't GET request when the recent jobs is empty
            if (!SummaryFields.TryGetValue<RecentJobSummary[]>("RecentJobs", out var recentJobs)
                || recentJobs.Length == 0)
            {
                return [];
            }

            var path = $"{PATH}{Id}/workflow_jobs/?order_by=-id&page_size={count}";
            return [.. RestAPI.GetResultSet<WorkflowJob>(path)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
