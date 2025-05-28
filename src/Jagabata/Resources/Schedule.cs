using System.Runtime.CompilerServices;
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
        /// Get a Schedule
        /// <para>
        /// Implement API: <c>/api/v2/schedules/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Schedule ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Schedule> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Schedule>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Schedule Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Schedules
        /// <para>
        /// Implement API: <c>/api/v2/schedules/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Schedule> FindAsync(HttpQuery? query,
                                                                 [EnumeratorCancellation]
                                                                 CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Schedule>(PATH, query, ct))
            {
                foreach (var schedule in result.Contents.Results)
                {
                    yield return schedule;
                }
            }
        }

        /// <summary>
        /// Find Schedules associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/schedules/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Project</item>
        ///     <item>InventorySource</item>
        ///     <item>JobTemplate</item>
        ///     <item>SystemJobTemplate</item>
        ///     <item>WorkflowJobTemplate</item>
        /// </list>
        /// </summary>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Schedule> FindAsync(IResource resource,
                                                                 HttpQuery? query = null,
                                                                 [EnumeratorCancellation]
                                                                 CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Project => $"{Project.PATH}{resource.Id}/schedules/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{resource.Id}/schedules/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/schedules/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{resource.Id}/schedules/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{resource.Id}/schedules/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Schedule>(path, query, ct))
            {
                foreach (var schedule in result.Contents.Results)
                {
                    yield return schedule;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Schedule[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Schedules by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/schedules/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Schedule[] Find(string? searchWords = null,
                                      string orderBy = "id",
                                      ushort pageSize = 20,
                                      uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Schedule[] Find(IResource resource, HttpQuery? query = null)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
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

        /// <summary>
        /// Get the most recently jobs executed by this schedule.
        /// <para>
        /// Implement API: <c>/api/v2/schedules/{id}/jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public UnifiedJob[] GetRecentJobs(ushort count = 20)
        {
            return Related.TryGetPath("unified_jobs", out var path)
                ? [.. RestAPI.GetResultSetAsync(path, new QueryBuilder().SetOrderBy("-id")
                                                                        .SetPageSize(count)
                                                                        .Build())
                             .ToBlockingEnumerable()
                             .SelectMany(static apiResult => apiResult.Contents.Results)
                             .OfType<UnifiedJob>()]
                : [];
        }

        /// <summary>
        /// Find labels associated with this schedule
        /// <para>
        /// Implement API: <c>/api/v2/schedules/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public Label[] FindLabels(string? searchWords = null,
                                  string orderBy = "name",
                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Label>("labels",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find labels associated with this schedule
        /// <para>
        /// Implement API: <c>/api/v2/schedules/{id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Label[] FindLabels(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Label>("labels", query)];
        }

        /// <summary>
        /// Find credentials related to this schedule
        /// <para>
        /// Implement API: <c>/api/v2/schedules/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] FindCredentials(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Find credentials related to this schedule
        /// <para>
        /// Implement API: <c>/api/v2/schedules/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Get the template applied to this schedule.
        /// </summary>
        public UnifiedJobTemplate? GetTemplate()
        {
            return Related.TryGetPath("unified_job_template", out var path)
                   && SummaryFields.TryGetValue<UnifiedJobTemplateSummary>("UnifiedJobTemplate", out var template)
                ? template.Type switch
                {
                    ResourceType.InventorySource => RestAPI.Get<InventorySource>(path),
                    ResourceType.JobTemplate => RestAPI.Get<JobTemplate>(path),
                    ResourceType.Project => RestAPI.Get<Project>(path),
                    ResourceType.SystemJobTemplate => RestAPI.Get<SystemJobTemplate>(path),
                    ResourceType.WorkflowJobTemplate => RestAPI.Get<WorkflowJobTemplate>(path),
                    _ => throw new NotSupportedException($"Not supported type: {template.Type}")
                }
                : null;
        }

        /// <summary>
        /// Get the project related to this schedule
        /// </summary>
        public Project? GetProject()
        {
            return Related.TryGetPath("project", out var path) ? RestAPI.Get<Project>(path) : null;
        }

        /// <summary>
        /// Get the inventory related to this schedule
        /// </summary>
        public Inventory? GetInventory()
        {
            return Related.TryGetPath("inventory", out var path) ? RestAPI.Get<Inventory>(path) : null;
        }

        /// <summary>
        /// Get future schedules based on this schedule's <seealso cref="Rrule"/>
        /// <para>
        /// Implement API: <c>/api/v2/schedules/preview/</c>
        /// </para>
        /// </summary>
        public DateTime[] Preview()
        {
            var data = new Dictionary<string, string>
            {
                ["rrule"] = Rrule
            };
            var task = RestAPI.PostJsonAsync<SchedulePreview>(SchedulePreview.PATH, data);
            task.Wait();
            return task.Result.Contents is not null
                ? [.. task.Result.Contents.Local.Select(DateTime.Parse)]
                : [];
        }

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
