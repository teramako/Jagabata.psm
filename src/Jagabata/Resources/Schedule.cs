using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface ISchedule
    {
        string Name { get; }
        string Description { get; }
        string Rrule { get; }
        Dictionary<string, object?> ExtraData { get; }
        ulong? Inventory { get; }
        string? ScmBranch { get; }
        string? JobType { get; }
        string? JobTags { get; }
        string? SkipTags { get; }
        string? Limit { get; }
        bool? DiffMode { get; }
        JobVerbosity? Verbosity { get; }
        ulong? ExecutionEnvironment { get; }
        int? Forks { get; }
        int? JobSliceCount { get; }
        int? Timeout { get; }
        ulong UnifiedJobTemplate { get; }
        bool Enabled { get; }

    }

    public class Schedule(string rrule, ulong id, ResourceType type, string url, RelatedDictionary related,
                          SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                          string description, Dictionary<string, object?> extraData, ulong? inventory, string? scmBranch,
                          string? jobType, string? jobTags, string? skipTags, string? limit, bool? diffMode,
                          JobVerbosity? verbosity, ulong? executionEnvironment, int? forks, int? jobSliceCount,
                          int? timeout, ulong unifiedJobTemplate, bool enabled, DateTime? dtStart, DateTime? dtEnd,
                          DateTime? nextRun, string timezone, string until)
        : ResourceBase, ISchedule
    {
        public const string PATH = "/api/v2/schedules/";
        /// <summary>
        /// Retrieve a Schedule.<br/>
        /// API Path: <c>/api/v2/schedules/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Schedule> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Schedule>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Schedules.<br/>
        /// API Path: <c>/api/v2/schedules/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Schedule> Find(HttpQuery? query)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Schedule>(PATH, query))
            {
                foreach (var schedule in result.Contents.Results)
                {
                    yield return schedule;
                }
            }
        }

        public string Rrule { get; } = rrule;

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;

        public string Name { get; } = name;
        public string Description { get; } = description;
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
        public ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public bool Enabled { get; } = enabled;
        [JsonPropertyName("dtstart")]
        public DateTime? DtStart { get; } = dtStart;
        [JsonPropertyName("dtend")]
        public DateTime? DtEnd { get; } = dtEnd;
        public DateTime? NextRun { get; } = nextRun;
        public string TimeZone { get; } = timezone;
        public string Until { get; } = until;

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, Description);
            if (SummaryFields.TryGetValue<UnifiedJobTemplateSummary>("UnifiedJobTemplate", out var template))
            {
                item.Metadata.Add("Template", $"[{template.Type}:{template.Id}] {template.Name}");
            }
            if (NextRun is not null)
            {
                item.Metadata.Add("NextRun", $"{NextRun}");
            }
            return item;
        }
    }
}
