using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public class HostMetric(ulong id, string hostname, string url, DateTime? firstAutomation, DateTime? lastAutomation,
                             DateTime? lastDeleted, int automatedCounter, int deletedCounter, bool deleted,
                             int? usedInInventories)
        : IResource
    {
        public const string PATH = "/api/v2/host_metrics/";

        /// <summary>
        /// Get a host metric
        /// <para>
        /// Implement API: <c>/api/v2/host_metrics/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Metrics ID</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<HostMetric> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<HostMetric>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static HostMetric Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find host metrics
        /// <para>
        /// Implement API: <c>/api/v2/host_metrics/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<HostMetric> FindAsync(HttpQuery? query = null,
                                                                   [EnumeratorCancellation]
                                                                   CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<HostMetric>(PATH, query, ct))
            {
                foreach (var metric in result.Contents.Results)
                {
                    yield return metric;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static HostMetric[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find host metrics by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/host_metrics/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static HostMetric[] Find(string? searchWords = null,
                                        string orderBy = "hostname",
                                        ushort pageSize = 20,
                                        uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = ResourceType.HostMetrics;
        public string Hostname { get; } = hostname;
        public string Url { get; } = url;
        public DateTime? FirstAutomation { get; } = firstAutomation;
        public DateTime? LastAutomation { get; } = lastAutomation;
        public DateTime? LastDeleted { get; } = lastDeleted;
        public int AutomatedCounter { get; } = automatedCounter;
        public int DeletedCounter { get; } = deletedCounter;
        public bool Deleted { get; } = deleted;
        public int? UsedInInventories { get; } = usedInInventories;

        public override string ToString()
        {
            return $"{Type}:{Id}:{Hostname}";
        }
    }
}
