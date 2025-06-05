using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IHost
    {
        /// <summary>
        /// Name of this host.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this host.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Inventory ID.
        /// </summary>
        ulong Inventory { get; }
        /// <summary>
        /// Is this host online and available for running jobs?
        /// </summary>
        bool Enabled { get; }
        /// <summary>
        /// The value used by the remote inventory source to uniquely identify the host.
        /// </summary>
        string InstanceId { get; }
        /// <summary>
        /// Host variables in JSON or YAML format.
        /// </summary>
        string Variables { get; }
    }

    public class Host(ulong id, ResourceType type, string url, RelatedDictionary related,
                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                      string description, ulong inventory, bool enabled, string instanceId, string variables)
        : ResourceBase, IHost
    {
        public const string PATH = "/api/v2/hosts/";

        /// <summary>
        /// Get a Host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Host ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Host> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Host>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Host Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Hosts
        /// <para>
        /// Implement API: <c>/api/v2/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindAsync(HttpQuery? query = null,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(PATH, query, ct))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }

        /// <summary>
        /// Find Hosts associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/{hosts | all_hosts}/</c>
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
        /// When <paramref name="resource"/> is <see cref="Group"/>:
        /// <c>true</c> => all directly or indirectly hosts, <c>false</c> => only direct child hosts of ths Group.
        /// </param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindAsync(IResource resource,
                                                             HttpQuery? query = null,
                                                             bool all = true,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Inventory => $"{Resources.Inventory.PATH}{resource.Id}/hosts/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{resource.Id}/hosts/",
                ResourceType.Group => $"{Group.PATH}{resource.Id}/{(all ? "all_hosts" : "hosts")}/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query, ct))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Host[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Hosts by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Host[] Find(string? searchWords = null,
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
        public static Host[] Find(IResource resource, HttpQuery query, bool all = true)
        {
            return [.. FindAsync(resource, query, all).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Hosts associated with <paramref name="resource"/> by basic parameters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/{hosts | all_hosts}/</c>
        /// </para>
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, bool, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static Host[] Find(IResource resource,
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
        [JsonConverter(typeof(Json.SummaryFieldsHostConverter))]
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;

        public string Description { get; } = description;

        public ulong Inventory { get; } = inventory;

        public bool Enabled { get; } = enabled;

        public string InstanceId { get; } = instanceId;

        public string Variables { get; } = variables;

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
        /// Implement API: <c>/api/v2/hosts/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/hosts/{id}/activity_stream/</c>
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
        /// Find groups that are direct parent of this hosts
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of groups to retrieve</param>
        public Group[] FindParentGroups(string? searchWords = null,
                                        string orderBy = "name",
                                        ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Group>("groups",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find groups that are direct parent of this hosts
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Group[] FindParentGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Group>("groups", query)];
        }

        /// <summary>
        /// Find all groups directly or indirectly this host belonging to
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/all_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of hosts to retrieve</param>
        public Group[] FindAllAncestorGroups(string? searchWords = null,
                                             string orderBy = "name",
                                             ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Group>("all_groups",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find all groups directly or indirectly this host belonging to
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/all_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Group[] FindAllAncestorGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Group>("all_groups", query)];
        }

        /// <summary>
        /// Find job events associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/job_events/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public JobEvent[] FindJobEvents(string? searchWords = null,
                                        string orderBy = "-id",
                                        ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<JobEvent>("job_events",
                                                         searchWords,
                                                         orderBy,
                                                         pageSize)];
        }

        /// <summary>
        /// Find job events associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/job_events/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public JobEvent[] FindJobEvents(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<JobEvent>("job_events", query)];
        }

        /// <summary>
        /// Find job host summaries associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/job_host_summaries/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public JobHostSummary[] FindJobHostSummaries(string? searchWords = null,
                                                     string orderBy = "-id",
                                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<JobHostSummary>("job_host_summaries",
                                                               searchWords,
                                                               orderBy,
                                                               pageSize)];
        }

        /// <summary>
        /// Find job host summaries associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/job_host_summaries/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public JobHostSummary[] FindJobHostSummaries(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<JobHostSummary>("job_host_summaries", query)];
        }

        /// <summary>
        /// Find inventory sources associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/inventory_sources/</c>
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
        /// Find inventory sources associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/inventory_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public InventorySource[] FindInventorySources(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<InventorySource>("inventory_sources", query)];
        }

        /// <summary>
        /// Find ad hoc commands associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of hosts to retrieve</param>
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
        /// Find ad hoc commands associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public AdHocCommand[] FindAdHocCommandJobs(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<AdHocCommand>("ad_hoc_commands", query)];
        }

        /// <summary>
        /// Find ad hoc command events associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/ad_hoc_command_events/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public AdHocCommandJobEvent[] FindAdHocCommandEvents(string? searchWords = null,
                                                             string orderBy = "-id",
                                                             ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<AdHocCommandJobEvent>("ad_hoc_command_events",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find ad hoc command events associated with this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/ad_hoc_command_events/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public AdHocCommandJobEvent[] FindAdHocCommandEvents(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<AdHocCommandJobEvent>("ad_hoc_command_events", query)];
        }

        /// <summary>
        /// Get the facts data of this host
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/ansible_facts/</c>
        /// </para>
        /// </summary>
        public Dictionary<string, object?>? GetFactCache()
        {
            return Related.TryGetPath("ansible_facts", out var path)
                ? RestAPI.Get<Dictionary<string, object?>>(path)
                : null;
        }

        /// <summary>
        /// Find job template jobs related to this host
        /// <para>
        /// Implement API: <c>/api/v2/jobs/?hosts={id}</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number to retrieve</param>
        public JobTemplateJob[] FindJobs(string? searchWords = null,
                                         string orderBy = "-id",
                                         ushort pageSize = 20)
        {
            return FindJobs(new QueryBuilder().SetSearchWords(searchWords)
                                              .SetOrderBy(orderBy)
                                              .SetPageSize(pageSize)
                                              .Build());
        }

        /// <summary>
        /// Find job template jobs related to this host
        /// <para>
        /// Implement API: <c>/api/v2/jobs/?hosts={id}</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public JobTemplateJob[] FindJobs(HttpQuery query)
        {
            query.Set("hosts", $"{Id}");
            return [.. RestAPI.GetResultSet<JobTemplateJob>(JobTemplateJobBase.PATH, query)
                              .SelectMany(static apiResult => apiResult.Contents.Results)];
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
}
