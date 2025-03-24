namespace Jagabata.Resources
{
    public class WorkflowApprovalTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                          SummaryFieldsDictionary summaryFields, DateTime created,
                                          DateTime? modified, string name, string description, DateTime? lastJobRun,
                                          bool lastJobFailed, DateTime? nextJobRun, JobTemplateStatus status,
                                          ulong? executionEnvironment, int timeout)
        : ResourceBase
    {
        public const string PATH = "/api/v2/workflow_approval_templates/";

        /// <summary>
        /// Retrieve a Workflow Approval Template.<br/>
        /// API Path: <c>/api/v2/workflow_approval_templates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<WorkflowApprovalTemplate> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowApprovalTemplate>($"{PATH}{id}/");
            return apiResult.Contents;
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public DateTime? LastJobRun { get; } = lastJobRun;
        public bool LastJobFailed { get; } = lastJobFailed;
        public DateTime? NextJobRun { get; } = nextJobRun;
        public JobTemplateStatus Status { get; } = status;
        public ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public int Timeout { get; } = timeout;

        /// <summary>
        /// Get the most recently requested workflow approvals.
        /// Implement API: <c>/api/v2/workflow_approval_templates/{id}/approvals/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public WorkflowApproval[] GetRecentJobs(int count = 20)
        {
            return [.. RestAPI.GetResultSet<WorkflowApproval>($"{PATH}{Id}/approvals/",
                                                              new HttpQuery($"order_by=-id&page_size={count}"))
                              .SelectMany(static apiResult => apiResult.Contents.Results)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
