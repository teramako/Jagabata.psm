using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public class Inventory(ulong id, ResourceType type, string url, RelatedDictionary related,
                           SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                           string description, ulong organization, string kind, string hostFilter, string variables,
                           bool hasActiveFailures, int totalHosts, int hostsWithActiveFailures, int totalGroups,
                           bool hasInventorySources, int totalInventorySources, int inventorySourcesWithFailures,
                           bool pendingDeletion, bool preventInstanceGroupFallback)
        : InventoryBase
    {
        public const string PATH = "/api/v2/inventories/";

        /// <summary>
        /// Retrieve an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Inventory> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Inventory>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Inventories.<br/>
        /// API Path: <c>/api/v2/inventories/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Inventory> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Inventory>(PATH, query, getAll))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }
        /// <summary>
        /// List Inventories for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/inventories/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Inventory> FindFromOrganization(ulong organizationId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/inventories/";
            await foreach (var result in RestAPI.GetResultSetAsync<Inventory>(path, query, getAll))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }
        /// <summary>
        /// List Inventories for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/input_inventories/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Inventory> FindInputInventoires(ulong inventoryId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{PATH}{inventoryId}/input_inventories/";
            await foreach (var result in RestAPI.GetResultSetAsync<Inventory>(path, query, getAll))
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
        /// <summary>
        /// Filter that will be applied to the hosts of this inventory.
        /// </summary>
        public string HostFilter { get; } = hostFilter;
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
    }
}
