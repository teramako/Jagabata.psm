namespace Jagabata.Resources
{
    public interface IWorkflowJobTemplateNode
    {
        Dictionary<string, object?> ExtraData { get; }
        /// <summary>
        /// Inventory applied as a prompt, assuming job template for inventory.
        /// </summary>
        ulong? Inventory { get; }
        string? ScmBranch { get; }
        string? JobType { get; }
        string? JobTags { get; }
        string? SkipTags { get; }
        string? Limit { get; }
        bool? DiffMode { get; }
        JobVerbosity? Verbosity { get; }
        /// <summary>
        /// The container image to be used for execution.
        /// </summary>
        ulong? ExecutionEnvironment { get; }
        int? Forks { get; }
        int? JobSliceCount { get; }
        int? Timeout { get; }
        ulong WorkflowJobTemplate { get; }
        ulong? UnifiedJobTemplate { get; }
        bool AllParentsMustConverge { get; }
        string Identifier { get; }
    }


    public class WorkflowJobTemplateNode(ulong id, ResourceType type, string url, RelatedDictionary related,
                                         SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                         Dictionary<string, object?> extraData, ulong? inventory, string? scmBranch,
                                         string? jobType, string? jobTags, string? skipTags, string? limit,
                                         bool? diffMode, JobVerbosity? verbosity, ulong? executionEnvironment,
                                         int? forks, int? jobSliceCount, int? timeout, ulong workflowJobTemplate,
                                         ulong? unifiedJobTemplate, ulong[] successNodes, ulong[] failureNodes,
                                         ulong[] alwaysNodes, bool allParentsMustConverge, string identifier)
        : ResourceBase, IWorkflowJobTemplateNode
    {
        public const string PATH = "/api/v2/workflow_job_template_nodes/";
        /// <summary>
        /// Retrieve a Workflow Job Template Node.<br/>
        /// API Path: <c>/api/v2/workflow_job_template_nodes/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<WorkflowJobTemplateNode> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobTemplateNode>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Workflow Job Template Nodes.<br/>
        /// API Path: <c>/api/v2/workflow_job_templates_nodes/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplateNode> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplateNode>(PATH, query))
            {
                foreach (var jobTemplateNode in result.Contents.Results)
                {
                    yield return jobTemplateNode;
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
        public Dictionary<string, object?> ExtraData { get; } = extraData;
        public ulong? Inventory { get; } = inventory;
        public string? ScmBranch { get; } = scmBranch;
        public string? JobType { get; } = jobType;
        public string? JobTags { get; } = jobTags;
        public string? SkipTags { get; } = skipTags;
        public string? Limit { get; } = limit;
        public bool? DiffMode { get; } = diffMode;
        public JobVerbosity? Verbosity { get; } = verbosity;
        public ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public int? Forks { get; } = forks;
        public int? JobSliceCount { get; } = jobSliceCount;
        public int? Timeout { get; } = timeout;
        public ulong WorkflowJobTemplate { get; } = workflowJobTemplate;
        public ulong? UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public ulong[] SuccessNodes { get; } = successNodes;
        public ulong[] FailureNodes { get; } = failureNodes;
        public ulong[] AlwaysNodes { get; } = alwaysNodes;
        public bool AllParentsMustConverge { get; } = allParentsMustConverge;
        public string Identifier { get; } = identifier;

        /// <summary>
        /// Get the inventory related to this workflow job template node.
        /// </summary>
        public Inventory? GetInventory()
        {
            return Related.TryGetPath("inventory", out var path)
                ? RestAPI.Get<Inventory>(path)
                : null;
        }

        /// <summary>
        /// Get a list of labels associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public Label[] GetLabels(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Label>("labels", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of labels associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Label[] GetLabels(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Label>("labels", query)];
        }

        /// <summary>
        /// Get the credentials related to this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] GetCredentials(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Credential>("credentials", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the credentials related to this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] GetCredentials(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Get a list of success nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] GetSuccessNodes(string? searchWords = null, string orderBy = "id", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("success_nodes", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of success nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] GetSuccessNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("success_nodes", query)];
        }

        /// <summary>
        /// Get a list of failure nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] GetFailureNodes(string? searchWords = null, string orderBy = "id", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("failure_nodes", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of failure nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] GetFailureNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("failure_nodes", query)];
        }

        /// <summary>
        /// Get a list of always nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] GetAlwaysNodes(string? searchWords = null, string orderBy = "id", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("always_nodes", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of always nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] GetAlwaysNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplateNode>("always_nodes", query)];
        }

        /// <summary>
        /// Get the parent workflow job template of this node.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates/{id}/</c>
        /// </para>
        /// </summary>
        public WorkflowJobTemplate? GetWorkflowJobTemplate()
        {
            return Related.TryGetPath("workflow_job_template", out var path)
                ? RestAPI.Get<WorkflowJobTemplate>(path)
                : null;
        }

        /// <summary>
        /// Get the template applied to this node.
        /// </summary>
        public UnifiedJobTemplate? GetTemplate()
        {
            return Related.TryGetPath("unified_job_template", out var path)
                   && SummaryFields.TryGetValue<UnifiedJobTemplateSummary>("UnifiedJobTemplate", out var template)
                ? template.Type switch
                {
                    ResourceType.InventorySource => RestAPI.Get<InventorySource>(path),
                    ResourceType.JobTemplate => RestAPI.Get<JobTemplate>(path),
                    ResourceType.Project => RestAPI.Get<Project>(path),
                    ResourceType.WorkflowJobTemplate => RestAPI.Get<WorkflowJobTemplate>(path),
                    ResourceType.WorkflowApprovalTemplate => RestAPI.Get<WorkflowApprovalTemplate>(path),
                    _ => throw new NotSupportedException($"Not supported type: {template.Type}")
                }
                : null;
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, string.Empty, string.Empty);
            if (SummaryFields.TryGetValue<UnifiedJobTemplateSummary>("UnifiedJobTemplate", out var template))
            {
                item.Name = template.Name;
                item.Description = template.Description;
                item.Metadata.Add("Template", $"[{template.Type}:{template.Id}] {template.Name}");
            }
            if (SummaryFields.TryGetValue<WorkflowJobTemplateSummary>("WorkflowJobTemplate", out var wjTemplate))
            {
                item.Metadata.Add("WorkflowJobTemplate", $"[{wjTemplate.Type}:{wjTemplate.Id}] {wjTemplate.Name}");
            }
            return item;
        }
    }
}
