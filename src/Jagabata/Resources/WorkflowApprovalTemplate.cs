namespace Jagabata.Resources
{
    public class WorkflowApprovalTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                          SummaryFieldsDictionary summaryFields, DateTime created,
                                          DateTime? modified, string name, string description, DateTime? lastJobRun,
                                          bool lastJobFailed, DateTime? nextJobRun, JobTemplateStatus status,
                                          ulong? executionEnvironment, int timeout)
        : UnifiedJobTemplate
    {
        public new const string PATH = "/api/v2/workflow_approval_templates/";

        /// <summary>
        /// Get a Workflow Approval Template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_approval_templates/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Approval Template ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<WorkflowApprovalTemplate> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<WorkflowApprovalTemplate>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new WorkflowApprovalTemplate Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
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
