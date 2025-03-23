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
        /// Retrieve a Constructed Inventory.<br/>
        /// API Path: <c>/api/v2/constructed_inventories/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ConstructedInventory> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<ConstructedInventory>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Constructed Inventories.<br/>
        /// API Path: <c>/api/v2/constructed_inventories/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ConstructedInventory> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ConstructedInventory>(PATH, query))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
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
