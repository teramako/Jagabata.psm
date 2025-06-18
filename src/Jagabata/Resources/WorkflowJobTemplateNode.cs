using System.Runtime.CompilerServices;

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
        /// Get a Workflow Job Template Node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Job Template Node ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<WorkflowJobTemplateNode> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobTemplateNode>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static WorkflowJobTemplateNode Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Workflow Job Template Nodes
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_templates_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplateNode> FindAsync(HttpQuery? query = null,
                                                                                [EnumeratorCancellation]
                                                                                CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplateNode>(PATH, query, ct))
            {
                foreach (var jobTemplateNode in result.Contents.Results)
                {
                    yield return jobTemplateNode;
                }
            }
        }

        /// <summary>
        /// Find Workflow Job Template Nodes for a Workflow Job Template
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/workflow_job_templates/<paramref name="workflowJobTemplateId"/>/workflow_nodes/</c>
        /// </remarks>
        /// <param name="workflowJobTemplateId">Workflow Job Template ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplateNode> FindAsync(ulong workflowJobTemplateId,
                                                                                HttpQuery? query = null,
                                                                                [EnumeratorCancellation]
                                                                                CancellationToken ct = default)
        {
            var path = $"{Resources.WorkflowJobTemplate.PATH}{workflowJobTemplateId}/workflow_nodes/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplateNode>(path, query, ct))
            {
                foreach (var jobNode in result.Contents.Results)
                {
                    yield return jobNode;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static WorkflowJobTemplateNode[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Template Nodes by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static WorkflowJobTemplateNode[] Find(string? searchWords = null,
                                                     string orderBy = "-id",
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
        public static WorkflowJobTemplateNode[] Find(ulong workflowJobTemplateId, HttpQuery query)
        {
            return [.. FindAsync(workflowJobTemplateId, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Template Nodes for a Workflow Job Template by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static WorkflowJobTemplateNode[] Find(ulong workflowJobTemplateId,
                                                     string? searchWords = null,
                                                     string orderBy = "-id",
                                                     ushort pageSize = 20,
                                                     uint startPage = 1)
        {
            return Find(workflowJobTemplateId, new QueryBuilder().SetSearchWords(searchWords)
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
        /// Find labels associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public Label[] FindLabels(string? searchWords = null,
                                  string orderBy = "name",
                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Label>("labels",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find labels associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Label[] FindLabels(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Label>("labels", query)];
        }

        /// <summary>
        /// Find credentials related to this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] FindCredentials(string? searchWords = null,
                                            string orderBy = "name",
                                            ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials",
                                                           searchWords,
                                                           orderBy,
                                                           pageSize)];
        }

        /// <summary>
        /// Find credentials related to this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Find success nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] FindSuccessNodes(string? searchWords = null,
                                                          string orderBy = "id",
                                                          ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("success_nodes",
                                                                        searchWords,
                                                                        orderBy,
                                                                        pageSize)];
        }

        /// <summary>
        /// Find success nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] FindSuccessNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("success_nodes", query)];
        }

        /// <summary>
        /// Find failure nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] FindFailureNodes(string? searchWords = null,
                                                          string orderBy = "id",
                                                          ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("failure_nodes",
                                                                        searchWords,
                                                                        orderBy,
                                                                        pageSize)];
        }

        /// <summary>
        /// Find failure nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] FindFailureNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("failure_nodes", query)];
        }

        /// <summary>
        /// Find always nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobTemplateNode[] FindAlwaysNodes(string? searchWords = null,
                                                         string orderBy = "id",
                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("always_nodes",
                                                                        searchWords,
                                                                        orderBy,
                                                                        pageSize)];
        }

        /// <summary>
        /// Find always nodes associated with this workflow job template node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_template_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobTemplateNode[] FindAlwaysNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplateNode>("always_nodes", query)];
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
