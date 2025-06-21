using System.Runtime.CompilerServices;
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
        /// Get a Workflow Job Template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Job Template ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<WorkflowJobTemplate> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobTemplate>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new WorkflowJobTemplate Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Workflow Job Templates
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<WorkflowJobTemplate> FindAsync(HttpQuery? query = null,
                                                                                [EnumeratorCancellation]
                                                                                CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplate>(PATH, query, ct))
            {
                foreach (var wjt in result.Contents.Results)
                {
                    yield return wjt;
                }
            }
        }

        /// <summary>
        /// Find Workflow Job Templates for an Organization
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/organizations/<paramref name="organizationId"/>/workflow_job_templates/</c>
        /// </remarks>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplate> FindAsync(ulong organizationId,
                                                                            HttpQuery? query = null,
                                                                            [EnumeratorCancellation]
                                                                            CancellationToken ct = default)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/workflow_job_templates/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplate>(path, query, ct))
            {
                foreach (var wjt in result.Contents.Results)
                {
                    yield return wjt;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new WorkflowJobTemplate[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Templates by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new WorkflowJobTemplate[] Find(string? searchWords = null,
                                                     string orderBy = "name",
                                                     ushort pageSize = 20,
                                                     uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        public static WorkflowJobTemplate[] Find(ulong organizationId, HttpQuery query)
        {
            return [.. FindAsync(organizationId, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Templates for an Organization by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static WorkflowJobTemplate[] Find(ulong organizationId,
                                                 string? searchWords = null,
                                                 string orderBy = "name",
                                                 ushort pageSize = 20,
                                                 uint startPage = 1)
        {
            return Find(organizationId, new QueryBuilder().SetSearchWords(searchWords)
                                                          .SetOrderBy(orderBy)
                                                          .SetPageSize(pageSize)
                                                          .SetStartPage(startPage)
                                                          .Build());
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

        [JsonIgnore]
        public LabelSummary[] Labels =>
            SummaryFields.TryGetValue<ListSummary<LabelSummary>>("Labels", out var labels)
            ? labels.Results
            : [];

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        /// <summary>
        /// Get the most recently executed workflow jobs.
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/workflow_jobs/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public WorkflowJob[] GetRecentJobs(ushort count = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJob>("workflow_jobs", null, "-id", count)];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] FindActivityStream(string? searchWords = null,
                                                   string orderBy = "-timestamp",
                                                   ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream",
                                                               searchWords,
                                                               orderBy,
                                                               pageSize)];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the webhook key for this workflow job template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/webhook_key/</c>
        /// </para>
        /// </summary>
        public Dictionary<string, string>? GetWebhookKey()
        {
            return !string.IsNullOrEmpty(WebhookService) && Related.TryGetPath("webhook_key", out var path)
                ? RestAPI.Get<Dictionary<string, string>>(path)
                : null;
        }

        /// <summary>
        /// Find workflow nodes related to this workflow job template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/workflow_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public WorkflowJobTemplateNode[] FindWorkflowNodes(string? searchWords = null,
                                                           string orderBy = "id",
                                                           ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("workflow_nodes",
                                                                        searchWords,
                                                                        orderBy,
                                                                        pageSize)];
        }

        /// <summary>
        /// Find workflow nodes related to this workflow job template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/workflow_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public WorkflowJobTemplateNode[] FindWorkflowNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("workflow_nodes", query)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(string? searchWords = null,
                                                                       string orderBy = "name",
                                                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        /// <summary>
        /// Find notification templates that have approvals notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnApprovals(string? searchWords = null,
                                                                           string orderBy = "name",
                                                                           ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have approvals notification enabled for this workflow job template.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnApprovals(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals", query)];
        }

        /// <summary>
        /// Find the access list related to this workflow job template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindAccessList(string? searchWords = null,
                                     string orderBy = "username",
                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<User>("access_list",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find the access list related to this workflow job template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAccessList(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Get the object roles related to this workflow job template
        /// </summary>
        /// <remarks>
        /// This is almost same as:
        /// <code>thisObject.SummaryFields["ObjectRoles"]</code>
        /// </remarks>
        public ObjectRoleSummary[] GetObjectRoles()
        {
            return SummaryFields.TryGetValue<Dictionary<string, ObjectRoleSummary>>("ObjectRoles", out var dict)
                ? [.. dict.Values]
                : [];
        }

        /// <summary>
        /// Get the survey spec for this workflow job template
        /// </summary>
        public Survey? GetSurveySpec()
        {
            return SummaryFields.ContainsKey("Survey") && Related.TryGetPath("survey_spec", out var path)
                ? RestAPI.Get<Survey>(path)
                : null;
        }

        /// <summary>
        /// Get the organization related to this workflow job template
        /// </summary>
        public Organization? GetOrganization()
        {
            return Related.TryGetPath("organization", out var path) ? RestAPI.Get<Organization>(path) : null;
        }

        /// <summary>
        /// Get the inventory related to this workflow job template
        /// </summary>
        public Inventory? GetInventory()
        {
            return Related.TryGetPath("inventory", out var path) ? RestAPI.Get<Inventory>(path) : null;
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
