using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface IGroup
    {
        /// <summary>
        /// Name of this group.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this group.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Inventory ID
        /// </summary>
        ulong Inventory { get; }
        /// <summary>
        /// Group variables in JSON or YAML format.
        /// </summary>
        string Variables { get; }
    }

    public abstract class GroupBase : ResourceBase, IGroup
    {
        public abstract DateTime Created { get; }
        public abstract DateTime? Modified { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ulong Inventory { get; }
        public abstract string Variables { get; }

        /// <summary>
        /// Desrialize <see cref="Variables" /> to Dictionary
        /// </summary>
        public Dictionary<string, object?> GetVariables()
        {
            return Yaml.DeserializeToDict(Variables);
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] FindActivityStream(string? searchWords = null,
                                                   string orderBy = "-timestamp",
                                                   ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream",
                                                               searchWords,
                                                               orderBy,
                                                               pageSize)];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the inventory associated with this rersource.
        /// </summary>
        public Inventory? GetInventory()
        {
            return Related.TryGetPath("inventory", out var path)
                ? RestAPI.Get<Inventory>(path)
                : null;
        }

        /// <summary>
        /// Find groups associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/children/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public Group[] FindChildGroups(string? searchWords = null,
                                       string orderBy = "name",
                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Group>("children",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find groups associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/children/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Group[] FindChildGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Group>("children", query)];
        }

        /// <summary>
        /// Find hosts that are direct children of this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of hosts to retrieve</param>
        public Host[] FindChildHosts(string? searchWords = null,
                                     string orderBy = "name",
                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Host>("hosts",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find hosts that are direct children of this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Host[] FindChildHosts(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Host>("hosts", query)];
        }

        /// <summary>
        /// Find all hosts directly or indirectly belonging to this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/all_hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of hosts to retrieve</param>
        public Host[] FindAllDescendantHosts(string? searchWords = null,
                                             string orderBy = "name",
                                             ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Host>("all_hosts",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find all hosts directly or indirectly belonging to this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/all_hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Host[] FindAllDescendantHosts(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Host>("all_hosts", query)];
        }

        /// <summary>
        /// Find job events associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/job_events/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public JobEvent[] FindJobEvents(string? searchWords = null, string orderBy = "-id", ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<JobEvent>("job_events", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Find job events associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/job_events/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public JobEvent[] FindJobEvents(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<JobEvent>("job_events", query)];
        }

        /// <summary>
        /// Get job host summaries associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/job_host_summaries/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public JobHostSummary[] FindJobHostSummaries(string? searchWords = null, string orderBy = "-id", ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<JobHostSummary>("job_host_summaries", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Find job host summaries associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/job_host_summaries/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public JobHostSummary[] FindJobHostSummaries(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<JobHostSummary>("job_host_summaries", query)];
        }

        /// <summary>
        /// Find inventory sources associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/inventory_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public InventorySource[] FindInventorySources(string? searchWords = null,
                                                      string orderBy = "name",
                                                      ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<InventorySource>("inventory_sources",
                                                                searchWords,
                                                                orderBy,
                                                                pageSize)];
        }

        /// <summary>
        /// Find inventory sources associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/inventory_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public InventorySource[] FindInventorySources(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<InventorySource>("inventory_sources", query)];
        }

        /// <summary>
        /// Find ad hoc commands associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public AdHocCommand[] FindAdHocCommandJobs(string? searchWords = null,
                                                   string orderBy = "-id",
                                                   ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<AdHocCommand>("ad_hoc_commands",
                                                             searchWords,
                                                             orderBy,
                                                             pageSize)];
        }

        /// <summary>
        /// Find ad hoc commands associated with this group
        /// <para>
        /// Implement API: <c>/api/v2/groups/{id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public AdHocCommand[] FindAdHocCommandJobs(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<AdHocCommand>("ad_hoc_commands", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, Description);
            if (SummaryFields.TryGetValue<InventorySummary>("Inventory", out var inventory))
            {
                item.Metadata.Add("Inventory", $"[{inventory.Type}:{inventory.Id}] {inventory.Name}");
            }
            return item;
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }

    public class Group(ulong id, ResourceType type, string url, RelatedDictionary related,
                       SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                       string description, ulong inventory, string variables)
        : GroupBase
    {
        public const string PATH = "/api/v2/groups/";

        /// <summary>
        /// Get a Group
        /// <para>
        /// Implement API: <c>/api/v2/groups/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<Group> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Group>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Group Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Groups
        /// <para>
        /// Implement API: <c>/api/v2/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindAsync(HttpQuery? query = null,
                                                              [EnumeratorCancellation]
                                                              CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(PATH, query, ct))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }

        /// <summary>
        /// Find Groups associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/{groups | root_groups | all_groups}/</c>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Inventory</item>
        ///     <item>InventorySource</item>
        ///     <item>Host</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with this group</param>
        /// <param name="query"></param>
        /// <param name="all">
        /// When <paramref name="resource"/> is:
        /// <list type="table">
        ///     <item>
        ///         <term>Inventory</term>
        ///         <description><c>true</c> => all groups, <c>false</c> => root (top-level) groups associated with the Inventory.</description>
        ///     </item>
        ///     <item>
        ///         <term>Host</term>
        ///         <description><c>true</c> => all directly or indirectly groups, <c>false</c> => only direct parent group of ths Host.</description>
        ///     </item>
        ///     <item>
        ///         <term>otherwise</term>
        ///         <description>No affected</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindAsync(IResource resource,
                                                              HttpQuery? query = null,
                                                              bool all = true,
                                                              [EnumeratorCancellation]
                                                              CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Inventory => $"{Resources.Inventory.PATH}{resource.Id}/{(all ? "groups" : "root_groups")}/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{resource.Id}/groups/",
                ResourceType.Host => $"{Host.PATH}{resource.Id}/{(all ? "all_groups" : "groups")}/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, ct))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Group[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Groups by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Group[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, bool, CancellationToken)"/>
        public static Group[] Find(IResource resource, HttpQuery query, bool all = true)
        {
            return [.. FindAsync(resource, query, all).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Groups associated with <paramref name="resource"/> by basic parameters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/{groups | root_groups | all_groups}/</c>
        /// </para>
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, bool, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static Group[] Find(IResource resource,
                                   string? searchWords = null,
                                   bool all = true,
                                   string orderBy = "name",
                                   ushort pageSize = 20,
                                   uint startPage = 1)
        {
            return Find(resource,
                        new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build(),
                        all);
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
        public override ulong Inventory { get; } = inventory;
        public override string Variables { get; } = variables;

        public sealed class Tree(ulong id, ResourceType type, string url, RelatedDictionary related,
                           SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                           string description, ulong inventory, string variables, Tree[]? children)
            : GroupBase
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override string Description { get; } = description;
            public override ulong Inventory { get; } = inventory;
            public override string Variables { get; } = variables;
            public Tree[]? Children { get; } = children;
        }
    }
}
