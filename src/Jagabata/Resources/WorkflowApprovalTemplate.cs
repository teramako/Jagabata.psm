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

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
