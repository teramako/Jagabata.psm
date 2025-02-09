using System.Collections.Specialized;

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


    public class WorkflowJobTemplateNode(ulong id,
                                         ResourceType type,
                                         string url,
                                         RelatedDictionary related,
                                         SummaryFieldsDictionary summaryFields,
                                         DateTime created,
                                         DateTime? modified,
                                         Dictionary<string, object?> extraData,
                                         ulong? inventory,
                                         string? scmBranch,
                                         string? jobType,
                                         string? jobTags,
                                         string? skipTags,
                                         string? limit,
                                         bool? diffMode,
                                         JobVerbosity? verbosity,
                                         ulong? executionEnvironment,
                                         int? forks,
                                         int? jobSliceCount,
                                         int? timeout,
                                         ulong workflowJobTemplate,
                                         ulong? unifiedJobTemplate,
                                         ulong[] successNodes,
                                         ulong[] failureNodes,
                                         ulong[] alwaysNodes,
                                         bool allParentsMustConverge,
                                         string identifier)
                : IWorkflowJobTemplateNode, IResource, ICacheableResource
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowJobTemplateNode> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowJobTemplateNode>(PATH, query, getAll))
            {
                foreach (var jobTemplateNode in result.Contents.Results)
                {
                    yield return jobTemplateNode;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
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

        public CacheItem GetCacheItem()
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
