using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public abstract class WorkflowApprovalBase : UnifiedJob
    {
        public new const string PATH = "/api/v2/workflow_approvals/";

        public abstract string Description { get; }
        public abstract ulong? UnifiedJobTemplate { get; }
        public abstract ulong? ExecutionEnvironment { get; }
        public abstract bool CanApproveOrDeny { get; }
        public abstract DateTime? ApprovalExpiration { get; }
        public abstract bool TimedOut { get; }

        public WorkflowApprovalTemplate? GetTemplate()
        {
            return GetTemplate<WorkflowApprovalTemplate>();
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, string.Empty, Description);
            if (SummaryFields.TryGetValue<UnifiedJobTemplateSummary>("UnifiedJobTemplate", out var template))
            {
                item.Metadata.Add("Template", $"[{template.Type}:{template.Id}] {template.Name}");
            }
            return item;
        }
    }

    public class WorkflowApproval(ulong id, ResourceType type, string url, RelatedDictionary related,
                                  SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                  string name, string description, ulong? unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                                  DateTime? finished, DateTime? canceledOn, double elapsed, string jobExplanation,
                                  string? workUnitId, bool canApproveOrDeny, DateTime? approvalExpiration, bool timedOut)
        : WorkflowApprovalBase
    {
        /// <summary>
        /// Get a Workflow Approval
        /// <para>
        /// Implement API: <c>/api/v2/workflow_approvals/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Approval ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<Detail> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new Detail Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Workflow Approvals
        /// <para>
        /// Implement API: <c>/api/v2/workflow_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<WorkflowApproval> FindAsync(HttpQuery? query = null,
                                                                             [EnumeratorCancellation]
                                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowApproval>(PATH, query, ct))
            {
                foreach (var approval in result.Contents.Results)
                {
                    yield return approval;
                }
            }
        }

        /// <summary>
        /// Find Workflow Approvals for a Workflow Approval Template
        /// <para>
        /// Implement API: <c>/api/v2/workflow_approval_templates/<paramref name="id"/>/approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Workflow Approval Template ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<WorkflowApproval> FindAsync(ulong id,
                                                                         HttpQuery? query = null,
                                                                         [EnumeratorCancellation]
                                                                         CancellationToken ct = default)
        {
            var path = $"{WorkflowApprovalTemplate.PATH}{id}/approvals/";
            await foreach (var result in RestAPI.GetResultSetAsync<WorkflowApproval>(path, query, ct))
            {
                foreach (var approval in result.Contents.Results)
                {
                    yield return approval;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new WorkflowApproval[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Workflow Approvals by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/workflow_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new WorkflowApproval[] Find(string? searchWords = null,
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
        public static WorkflowApproval[] Find(ulong id, HttpQuery? query = null)
        {
            return [.. FindAsync(id, query).ToBlockingEnumerable()];
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
        public override ulong? UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public override JobLaunchType LaunchType { get; } = launchType;
        public override JobStatus Status { get; } = status;
        public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public override bool Failed { get; } = failed;
        public override DateTime? Started { get; } = started;
        public override DateTime? Finished { get; } = finished;
        public override DateTime? CanceledOn { get; } = canceledOn;
        public override double Elapsed { get; } = elapsed;
        public override string JobExplanation { get; } = jobExplanation;
        public override string? WorkUnitId { get; } = workUnitId;
        public override bool CanApproveOrDeny { get; } = canApproveOrDeny;
        public override DateTime? ApprovalExpiration { get; } = approvalExpiration;
        public override bool TimedOut { get; } = timedOut;

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, ulong? unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                            ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                            DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string jobExplanation, string resultTraceback,
                            bool eventProcessingFinished, string? workUnitId, bool canApproveOrDeny,
                            DateTime? approvalExpiration, bool timedOut)
            : WorkflowApprovalBase, IJobDetail
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override string Description { get; } = description;
            public override ulong? UnifiedJobTemplate { get; } = unifiedJobTemplate;
            public override JobLaunchType LaunchType { get; } = launchType;
            public override JobStatus Status { get; } = status;
            public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
            public override bool Failed { get; } = failed;
            public override DateTime? Started { get; } = started;
            public override DateTime? Finished { get; } = finished;
            public override DateTime? CanceledOn { get; } = canceledOn;
            public override double Elapsed { get; } = elapsed;
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public override string JobExplanation { get; } = jobExplanation;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override string? WorkUnitId { get; } = workUnitId;
            public override bool CanApproveOrDeny { get; } = canApproveOrDeny;
            public override DateTime? ApprovalExpiration { get; } = approvalExpiration;
            public override bool TimedOut { get; } = timedOut;
        }
    }
}
