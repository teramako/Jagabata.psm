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
        /// Retrieve a Job Host Summary.<br/>
        /// API Path: <c>/api/v2/job_host_summaries/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<JobHostSummary> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<JobHostSummary>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Job Host Summaries for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/job_host_summaries/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobHostSummary> FindFromGroup(ulong groupId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/job_host_summaries/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobHostSummary>(path, query))
            {
                foreach (var jobHostSummary in result.Contents.Results)
                {
                    yield return jobHostSummary;
                }
            }
        }
        /// <summary>
        /// List Job Host Summaries for a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/job_host_summaries/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobHostSummary> FindFromHost(ulong hostId,
                                                                          HttpQuery? query = null)
        {
            var path = $"{Resources.Host.PATH}{hostId}/job_host_summaries/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobHostSummary>(path, query))
            {
                foreach (var jobHostSummary in result.Contents.Results)
                {
                    yield return jobHostSummary;
                }
            }
        }
        /// <summary>
        /// List Job Host Summaries for a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="jobId"/>/job_host_summaries/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<JobHostSummary> FindFromJob(ulong jobId,
                                                                         HttpQuery? query = null)
        {
            var path = $"{JobTemplateJobBase.PATH}{jobId}/job_host_summaries/";
            await foreach (var result in RestAPI.GetResultSetAsync<JobHostSummary>(path, query))
            {
                foreach (var jobHostSummary in result.Contents.Results)
                {
                    yield return jobHostSummary;
                }
            }
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
