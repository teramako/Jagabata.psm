using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public class SystemJobTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                   SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                   string name, string description, DateTime? lastJobRun, bool lastJobFailed,
                                   DateTime? nextJobRun, JobTemplateStatus status, ulong? executionEnvironment,
                                   string jobType)
        : UnifiedJobTemplate
    {
        public new const string PATH = "/api/v2/system_job_templates/";

        /// <summary>
        /// Get a System Job Template
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">System Job Template ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<SystemJobTemplate> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<SystemJobTemplate>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new SystemJobTemplate Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find System Job Templates
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<SystemJobTemplate> FindAsync(HttpQuery? query = null,
                                                                              [EnumeratorCancellation]
                                                                              CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJobTemplate>(PATH, query, ct))
            {
                foreach (var systemJobTemplate in result.Contents.Results)
                {
                    yield return systemJobTemplate;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new SystemJobTemplate[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find System Job Templates by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new SystemJobTemplate[] Find(string? searchWords = null,
                                                   string orderBy = "name",
                                                   ushort pageSize = 20,
                                                   uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
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
        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public override string Name { get; } = name;
        public override string Description { get; } = description;
        public override DateTime? LastJobRun { get; } = lastJobRun;
        public override bool LastJobFailed { get; } = lastJobFailed;
        public override DateTime? NextJobRun { get; } = nextJobRun;
        public override JobTemplateStatus Status { get; } = status;
        public ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public string JobType { get; } = jobType;

        /// <summary>
        /// Get the most recently executed jobs.
        /// Implement API: <c>/api/v2/system_job_templates/{id}/jobs/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public SystemJob[] GetRecentJobs(ushort count = 20)
        {
            return [.. FindResultsByRelatedKey<SystemJob>("jobs", null, "id", count)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(string? searchWords = null,
                                                                       string orderBy = "name",
                                                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this system job template.
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                }
            };
        }
    }
}
