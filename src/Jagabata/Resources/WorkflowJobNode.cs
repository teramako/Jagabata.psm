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
        /// Retrieve a Workflow Job Node.<br/>
        /// API Path: <c>/api/v2/workflow_job_nodes/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<WorkflowJobNode> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowJobNode>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Workflow Job Nodes.<br/>
        /// API Path: <c>/api/v2/workflow_job_nodes/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobNode> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobNode>(PATH, query))
            {
                foreach (var jobNode in result.Contents.Results)
                {
                    yield return jobNode;
                }
            }
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
        /// Get a list of success nodes associated with this workflow job node
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
        /// Get a list of success nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/success_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] GetSuccessNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobNode>("success_nodes", query)];
        }

        /// <summary>
        /// Get a list of failure nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobNode[] GetFailureNodes(string? searchWords = null, string orderBy = "id", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobNode>("failure_nodes", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of failure nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/failure_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] GetFailureNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobNode>("failure_nodes", query)];
        }

        /// <summary>
        /// Get a list of always nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public WorkflowJobNode[] GetAlwaysNodes(string? searchWords = null, string orderBy = "id", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobNode>("always_nodes", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get a list of always nodes associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/always_nodes/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public WorkflowJobNode[] GetAlwaysNodes(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobNode>("always_nodes", query)];
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
        /// Get a list of labels associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/labels/</c>
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
        /// Get a list of labels associated with this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Label[] GetLabels(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Label>("labels", query)];
        }

        /// <summary>
        /// Get the credentials related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/credentials/</c>
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
        /// Get the credentials related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] GetCredentials(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Get the instance groups related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public InstanceGroup[] GetInstanceGroups(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<InstanceGroup>("instance_groups", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the instance groups related to this workflow job node
        /// <para>
        /// Implement API: <c>/api/v2/workflow_job_nodes/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public InstanceGroup[] GetInstanceGroups(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<InstanceGroup>("instance_groups", query)];
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
