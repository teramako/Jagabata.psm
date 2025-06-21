using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IWorkflowJobNode
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
        ulong? Job { get; }
        ulong WorkflowJob { get; }
        ulong? UnifiedJobTemplate { get; }
        ulong[] SuccessNodes { get; }
        ulong[] FailureNodes { get; }
        ulong[] AlwaysNodes { get; }
        bool AllParentsMustConverge { get; }
        /// <summary>
        /// Indicates that a job will not be created when <c>True</c>.
        /// Workflow runtime sematics will mark this <c>True</c> if the node is in a path that will decidedly not be ran.
        /// A value of <c>False</c> means the node may not run.
        /// </summary>
        bool DoNotRun { get; }
        /// <summary>
        /// An identifier coresponding to the workflow job template node that this node was created from.
        /// </summary>
        string Identifier { get; }
    }

    public class WorkflowJobNode(ulong id, ResourceType type, string url, RelatedDictionary related,
                                 SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                 Dictionary<string, object?> extraData, ulong? inventory, string? scmBranch,
                                 string? jobType, string? jobTags, string? skipTags, string? limit, bool? diffMode,
                                 JobVerbosity? verbosity, ulong? executionEnvironment, int? forks, int? jobSliceCount,
                                 int? timeout, ulong? job, ulong workflowJob, ulong? unifiedJobTemplate,
                                 ulong[] successNodes, ulong[] failureNodes, ulong[] alwaysNodes,
                                 bool allParentsMustConverge, bool doNotRun, string identifier)
        : ResourceBase, IWorkflowJobNode
    {
        public const string PATH = "/api/v2/workflow_job_nodes/";

        /// <summary>
        /// Get a Workflow Job Node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Job Node ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<WorkflowJobNode> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobNode>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static WorkflowJobNode Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Workflow Job Nodes
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobNode> FindAsync(HttpQuery? query = null,
                                                                        [EnumeratorCancellation]
                                                                        CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobNode>(PATH, query, ct))
            {
                foreach (var jobNode in result.Contents.Results)
                {
                    yield return jobNode;
                }
            }
        }

        /// <summary>
        /// Find Workflow Job Nodes for a Workflow Job
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/workflow_jobs/<paramref name="workflowJobId"/>/workflow_nodes/</c>
        /// </remarks>
        /// <param name="workflowJobId">Workflow Job ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobNode> FindAsync(ulong workflowJobId,
                                                                        HttpQuery? query = null,
                                                                        [EnumeratorCancellation]
                                                                        CancellationToken ct = default)
        {
            var path = $"{WorkflowJobBase.PATH}{workflowJobId}/workflow_nodes/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobNode>(path, query, ct))
            {
                foreach (var jobNode in result.Contents.Results)
                {
                    yield return jobNode;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static WorkflowJobNode[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Nodes by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static WorkflowJobNode[] Find(string? searchWords = null,
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
        public static WorkflowJobNode[] Find(ulong workflowJobId, HttpQuery query)
        {
            return [.. FindAsync(workflowJobId, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Job Nodes for a Workflow Job by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static WorkflowJobNode[] Find(ulong workflowJobId,
                                             string? searchWords = null,
                                             string orderBy = "-id",
                                             ushort pageSize = 20,
                                             uint startPage = 1)
        {
            return Find(workflowJobId, new QueryBuilder().SetSearchWords(searchWords)
                                                         .SetOrderBy(orderBy)
                                                         .SetPageSize(pageSize)
                                                         .SetStartPage(startPage)
                                                         .Build());
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        [JsonConverter(typeof(Json.SummaryFieldsWorkflowJobNodeConverter))]
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
        public ulong? Job { get; } = job;
        public ulong WorkflowJob { get; } = workflowJob;
        public ulong? UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public ulong[] SuccessNodes { get; } = successNodes;
        public ulong[] FailureNodes { get; } = failureNodes;
        public ulong[] AlwaysNodes { get; } = alwaysNodes;
        public bool AllParentsMustConverge { get; } = allParentsMustConverge;
        public bool DoNotRun { get; } = doNotRun;
        public string Identifier { get; } = identifier;

        /// <summary>
        /// Find success nodes associated with this workflow job node
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
        /// Find success nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] FindSuccessNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobNode>("success_nodes", query)];
        }

        /// <summary>
        /// Find failure nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobNode[] FindFailureNodes(string? searchWords = null,
                                                  string orderBy = "id",
                                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobNode>("failure_nodes",
                                                                searchWords,
                                                                orderBy,
                                                                pageSize)];
        }

        /// <summary>
        /// Find failure nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] FindFailureNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobNode>("failure_nodes", query)];
        }

        /// <summary>
        /// Find always nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobNode[] FindAlwaysNodes(string? searchWords = null,
                                                 string orderBy = "id",
                                                 ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobNode>("always_nodes",
                                                                searchWords,
                                                                orderBy,
                                                                pageSize)];
        }

        /// <summary>
        /// Find always nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] FindAlwaysNodes(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobNode>("always_nodes", query)];
        }

        /// <summary>
        /// Get the parent workflow job of this node.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_jobs/{id}/</c>
        /// </para>
        /// </summary>
        public WorkflowJob? GetWorkflowJob()
        {
            return Related.TryGetPath("workflow_job", out var path)
                ? RestAPI.Get<WorkflowJob>(path)
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

        /// <summary>
        /// Find labels associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/labels/</c>
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
        /// Find labels associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Label[] FindLabels(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Label>("labels", query)];
        }

        /// <summary>
        /// Find credentials related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/credentials/</c>
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
        /// Find credentials related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Find instance groups related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public InstanceGroup[] FindInstanceGroups(string? searchWords = null,
                                                  string orderBy = "name",
                                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups",
                                                              searchWords,
                                                              orderBy,
                                                              pageSize)];
        }

        /// <summary>
        /// Find instance groups related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public InstanceGroup[] FindInstanceGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, string.Empty, string.Empty);
            if (SummaryFields.TryGetValue<WorkflowJobNodeJobSummary>("Job", out var job))
            {
                item.Name = job.Name;
                item.Description = job.Description;
                item.Metadata.Add("Job", $"[{job.Type}:{job.Id}] {job.Name}");
            }
            if (SummaryFields.TryGetValue<WorkflowJobSummary>("WorkflowJob", out var workflowJob))
            {
                item.Metadata.Add("WorkflowJob", $"[{workflowJob.Type}:{workflowJob.Id}] {workflowJob.Name}");
            }
            return item;
        }
    }
}
