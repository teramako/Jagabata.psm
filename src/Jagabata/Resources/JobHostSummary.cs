using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{

    public class JobHostSummary(ulong id, ResourceType type, string url, RelatedDictionary related, SummaryFieldsDictionary summaryFields,
                          DateTime created, DateTime? modified, ulong job, ulong host, ulong? constructedHost,
                          string hostName, int changed, int dark, int failures, int oK, int processed, int skipped,
                          bool failed, int ignored, int rescued)
        : ResourceBase
    {
        public const string PATH = "/api/v2/job_host_summaries/";

        /// <summary>
        /// Get a Job Host Summary.<br/>
        /// <para>
        /// Implement API: <c>/api/v2/job_host_summaries/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">JobHostSummary ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<JobHostSummary> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<JobHostSummary>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static JobHostSummary Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Hosts associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/job_host_summaries/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Group</item>
        ///     <item>Host</item>
        ///     <item>Job</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobHostSummary> FindAsync(IResource resource,
                                                                       HttpQuery? query = null,
                                                                       [EnumeratorCancellation]
                                                                       CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Group => $"{Group.PATH}{resource.Id}/job_host_summaries/",
                ResourceType.Host => $"{Resources.Host.PATH}{resource.Id}/job_host_summaries/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{resource.Id}/job_host_summaries/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<JobHostSummary>(path, query, ct))
            {
                foreach (var jobHostSummary in result.Contents.Results)
                {
                    yield return jobHostSummary;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static JobHostSummary[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Hosts associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static JobHostSummary[] Find(IResource resource,
                                            string? searchWords = null,
                                            string orderBy = "-id",
                                            ushort pageSize = 20,
                                            uint startPage = 1)
        {
            return Find(resource, new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        [JsonConverter(typeof(Json.SummaryFieldsJobHostSummaryConverter))]
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public ulong Job { get; } = job;
        public ulong Host { get; } = host;
        public ulong? ConstructedHost { get; } = constructedHost;
        public string HostName { get; } = hostName;
        public int Changed { get; } = changed;
        public int Dark { get; } = dark;
        public int Failures { get; } = failures;
        public int OK { get; } = oK;
        public int Processed { get; } = processed;
        public int Skipped { get; } = skipped;
        public bool Failed { get; } = failed;
        public int Ignored { get; } = ignored;
        public int Rescued { get; } = rescued;

        /// <summary>
        /// Get the job detail related to this job host summary
        /// <para>
        /// Implement: <c>/api/v2/jobs/{id}/</c>
        /// </para>
        /// </summary>
        public JobTemplateJob.Detail? GetJobDetail()
        {
            return Related.TryGetPath("job", out var path)
                ? RestAPI.Get<JobTemplateJob.Detail>(path)
                : null;
        }

        /// <summary>
        /// Get the host related to this job host summary
        /// <para>
        /// Implement: <c>/api/v2/hosts/{id}/</c>
        /// </para>
        /// </summary>
        public Host? GetHost()
        {
            return Related.TryGetPath("host", out var path)
                ? RestAPI.Get<Host>(path)
                : null;
        }

        /// <summary>
        /// Get job events for this job host summary's job
        /// <para>
        /// Implement: <c>/api/v2/jobs/{id}/job_events/</c>
        /// </para>
        /// </summary>
        public JobEvent[] GetEvents()
        {
            var path = $"{JobTemplateJobBase.PATH}{Job}/job_events/";
            var query = new HttpQuery("order_by=counter&page_size=200", QueryCount.Infinity);
            return [.. RestAPI.GetResultSet<JobEvent>(path, query)
                              .SelectMany(static apiResult => apiResult.Contents.Results)];
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, HostName, string.Empty);
            if (SummaryFields.TryGetValue<JobExSummary>("Job", out var job))
            {
                item.Metadata.Add("Job", $"[{job.Type}:{job.Id}] {job.Name}");
                item.Metadata.Add("Status", $"{job.Status}");
                item.Metadata.Add("Elapsed", $"{job.Elapsed}");
                item.Metadata.Add("JobTemplate", $"[{ResourceType.JobTemplate}:{job.JobTemplateId}] {job.JobTemplateName}");
            }
            return item;
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{HostName}";
        }
    }
}
