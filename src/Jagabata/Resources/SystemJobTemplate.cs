using System.Collections.Specialized;

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
        /// Retrieve a System Job Template.<br/>
        /// API Path: <c>/api/v2/system_job_templates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<SystemJobTemplate> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<SystemJobTemplate>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List System Job Templates.<br/>
        /// API Path: <c>/api/v2/system_job_templates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<SystemJobTemplate> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJobTemplate>(PATH, query, getAll))
            {
                foreach (var systemJobTemplate in result.Contents.Results)
                {
                    yield return systemJobTemplate;
                }
            }
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
        /// Get the most recently executed jobs
        /// </summary>
        /// <param name="count">Max count</param>
        public SystemJob[] GetRecentJobs(int count = 20)
        {
            var path = $"{PATH}{Id}/jobs/?order_by=-id&page_size={count}";
            return [.. RestAPI.GetResultSet<SystemJob>(path)];
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
