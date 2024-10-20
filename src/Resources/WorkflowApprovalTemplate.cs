namespace Jagabata.Resources
{
    public class WorkflowApprovalTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                          WorkflowApprovalTemplate.Summary summaryFields, DateTime created,
                                          DateTime? modified, string name, string description, DateTime? lastJobRun,
                                          bool lastJobFailed, DateTime? nextJobRun, JobTemplateStatus status,
                                          ulong? executionEnvironment, int timeout)
        : UnifiedJobTemplate(id, type, url, created, modified, name, description, lastJobRun, lastJobFailed, nextJobRun, status),
          IResource<WorkflowApprovalTemplate.Summary>
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

        public record Summary(WorkflowJobTemplateSummary WorkflowJobTemplate,
                              LastJobSummary? LastJob,
                              LastUpdateSummary? LastUpdate,
                              UserSummary? CreatedBy,
                              UserSummary? ModifiedBy,
                              EnvironmentSummary? ResolvedEnvironment);

        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;
        public ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public int Timeout { get; } = timeout;
    }
}
