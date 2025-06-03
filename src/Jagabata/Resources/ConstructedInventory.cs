using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public class ConstructedInventory(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      string name, string description, ulong organization, string kind, string variables,
                                      bool hasActiveFailures, int totalHosts, int hostsWithActiveFailures,
                                      int totalGroups, bool hasInventorySources, int totalInventorySources,
                                      int inventorySourcesWithFailures, bool pendingDeletion,
                                      bool preventInstanceGroupFallback, string sourceVars, int updateCacheTimeout,
                                      string limit, JobVerbosity verbosity)
        : InventoryBase
    {
        public const string PATH = "/api/v2/constructed_inventories/";

        /// <summary>
        /// Get a Constructed Inventory
        /// <para>
        /// Implement API: <c>/api/v2/constructed_inventories/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Inventory ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<ConstructedInventory> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<ConstructedInventory>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static ConstructedInventory Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Constructed Inventories
        /// <para>
        /// Implement API: <c>/api/v2/constructed_inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ConstructedInventory> FindAsync(HttpQuery? query = null,
                                                                             [EnumeratorCancellation]
                                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ConstructedInventory>(PATH, query, ct))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static ConstructedInventory[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Constructed Inventories by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/constructed_inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <returns></returns>
        public static ConstructedInventory[] Find(string? searchWords = null,
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
        public override ulong Organization { get; } = organization;
        public override string Kind { get; } = kind;
        public override string Variables { get; } = variables;
        public override bool HasActiveFailures { get; } = hasActiveFailures;
        public override int TotalHosts { get; } = totalHosts;
        public override int HostsWithActiveFailures { get; } = hostsWithActiveFailures;
        public override int TotalGroups { get; } = totalGroups;
        public override bool HasInventorySources { get; } = hasInventorySources;
        public override int TotalInventorySources { get; } = totalInventorySources;
        public override int InventorySourcesWithFailures { get; } = inventorySourcesWithFailures;
        public override bool PendingDeletion { get; } = pendingDeletion;
        public override bool PreventInstanceGroupFallback { get; } = preventInstanceGroupFallback;
        /// <summary>
        /// The source_vars for the related auto-create inventory source, special to constructed inventory.
        /// </summary>
        public string SourceVars { get; } = sourceVars;
        /// <summary>
        /// The cache timeout for the related auto-created inventory source, special to constructed inventory.
        /// </summary>
        public int UpdateCacheTimeout { get; } = updateCacheTimeout;
        /// <summary>
        /// The limit to restrict the returned hosts for the related auto-created inventory source,
        /// special to constructed inventory.
        /// </summary>
        public string Limit { get; } = limit;
        /// <summary>
        /// The verbosity level for the related auto-created inventory source, special to constructed inventory.
        /// </summary>
        public JobVerbosity Verbosity { get; } = verbosity;
    }
}
