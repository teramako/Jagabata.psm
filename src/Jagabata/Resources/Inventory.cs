using System.Runtime.CompilerServices;

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
        /// Get an Inventory
        /// <para>
        /// Implement API: <c>/api/v2/inventories/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Inventory ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Inventory> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Inventory>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Inventory Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Inventories
        /// <para>
        /// Implement API: <c>/api/v2/inventories//</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Inventory> FindAsync(HttpQuery? query = null,
                                                                  [EnumeratorCancellation]
                                                                  CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Inventory>(PATH, query, ct))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }

        /// <summary>
        /// Find Inventories associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API:
        /// <list type="bullet">
        ///     <item><c>/api/v2/{organizations | projects}/{Id}/inventories/</c></item>
        ///     <item><c>/api/v2/inventories/{Id}/input_inventories/</c></item>
        ///     <item><c>/api/v2/hosts/{Id}/smart_inventories/</c></item>
        /// </list>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Organization</item>
        ///     <item>Project</item>
        ///     <item>Inventory</item>
        ///     <item>Host</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Inventory> FindAsync(IResource resource,
                                                                  HttpQuery? query = null,
                                                                  [EnumeratorCancellation]
                                                                  CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Organization => $"{Resources.Organization.PATH}{resource.Id}/inventories/",
                ResourceType.Project => $"{Project.PATH}{resource.Id}/inventories/",
                ResourceType.Inventory => $"{PATH}{resource.Id}/input_inventories/",
                ResourceType.Host => $"{Host.PATH}{resource.Id}/smart_inventories/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Inventory>(path, query, ct))
            {
                foreach (var inventory in result.Contents.Results)
                {
                    yield return inventory;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Inventory[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Insntance Groups by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Inventory[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Inventory[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Inventories associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static Inventory[] Find(IResource resource,
                                       string? searchWords = null,
                                       string orderBy = "name",
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
