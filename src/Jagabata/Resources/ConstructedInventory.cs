using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IConstructedInventory
    {
        /// <summary>
        /// Name of this inventory.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this inventry.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Organization containing this inventory.
        /// </summary>
        ulong Organization { get; }
        /// <summary>
        /// Kind of inventory being represented.
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>""</c></term>
        ///         <description>Hosts have a direct link to this inventory.(default></description>
        ///     </item>
        ///     <item>
        ///         <term><c>"smart"</c></term>
        ///         <description>Hosts for inventory generated using the host_filter property</description>
        ///     </item>
        ///     <item>
        ///         <term><c>"constructed"</c></term>
        ///         <description>Parse list of source inventories with the constructed inventory plugin.</description>
        ///     </item>
        /// </list>
        /// </summary>
        string Kind { get; }
        /// <summary>
        /// Inventory variables in JSON or YAML
        /// </summary>
        string Variables { get; }
        /// <summary>
        /// If enabled, the inventory will prevent adding any organization instance groups
        /// to the list of preferred instances groups to run associated job templates on.
        /// If this setting is enabled and you provided an empty list, the global instance groups
        /// will be applied.
        /// </summary>
        bool PreventInstanceGroupFallback { get; }
        /// <summary>
        /// The source_vars for the related auto-create inventory source, special to constructed inventory.
        /// </summary>
        string SourceVars { get; }
        /// <summary>
        /// The cache timeout for the related auto-created inventory source, special to constructed inventory.
        /// </summary>
        int UpdateCacheTimeout { get; }
        /// <summary>
        /// The limit to restrict the returned hosts for the related auto-created inventory source,
        /// special to constructed inventory.
        /// </summary>
        string Limit { get; }
        /// <summary>
        /// The verbosity level for the related auto-created inventory source, special to constructed inventory.
        /// </summary>
        JobVerbosity Verbosity { get; }
    }

    public class ConstructedInventory(ulong id,
                                      ResourceType type,
                                      string url,
                                      RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields,
                                      DateTime created,
                                      DateTime? modified,
                                      string name,
                                      string description,
                                      ulong organization,
                                      string kind,
                                      string variables,
                                      bool hasActiveFailures,
                                      int totalHosts,
                                      int hostsWithActiveFailures,
                                      int totalGroups,
                                      bool hasInventorySources,
                                      int totalInventorySources,
                                      int inventorySourcesWithFailures,
                                      bool pendingDeletion,
                                      bool preventInstanceGroupFallback,
                                      string sourceVars,
                                      int updateCacheTimeout,
                                      string limit,
                                      JobVerbosity verbosity)
        : SummaryFieldsContainer, IConstructedInventory, IResource, ICacheableResource
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ConstructedInventory> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ConstructedInventory>(PATH, query, getAll))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong Organization { get; } = organization;
        public string Kind { get; } = kind;
        public string Variables { get; } = variables;
        public bool HasActiveFailures { get; } = hasActiveFailures;
        public int TotalHosts { get; } = totalHosts;
        public int HostsWithActiveFailures { get; } = hostsWithActiveFailures;
        public int TotalGroups { get; } = totalGroups;
        public bool HasInventorySources { get; } = hasInventorySources;
        public int TotalInventorySources { get; } = totalInventorySources;
        public int InventorySourcesWithFailures { get; } = inventorySourcesWithFailures;
        public bool PendingDeletion { get; } = pendingDeletion;
        public bool PreventInstanceGroupFallback { get; } = preventInstanceGroupFallback;
        public string SourceVars { get; } = sourceVars;
        public int UpdateCacheTimeout { get; } = updateCacheTimeout;
        public string Limit { get; } = limit;
        public JobVerbosity Verbosity { get; } = verbosity;

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }

        public CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Kind"] = Kind
                }
            };
        }
    }
}
