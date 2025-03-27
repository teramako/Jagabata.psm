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
        /// Retrieve a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Host> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Host>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Hosts.<br/>
        /// API Path: <c>/api/v2/hosts/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(PATH, query))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }
        /// <summary>
        /// List Hosts for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/hosts/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromInventory(ulong inventoryId,
                                                                     HttpQuery? query = null)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }
        /// <summary>
        /// List Hosts for an Inventory Source.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventorySourceId"/>/hosts/</c>
        /// </summary>
        /// <param name="inventorySourceId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromInventorySource(ulong inventorySourceId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }
        /// <summary>
        /// List All Hosts for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/all_hosts/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindAllFromGroup(ulong groupId,
                                                                    HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/all_hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }
        /// <summary>
        /// List Hosts for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/hosts/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromGroup(ulong groupId,
                                                                 HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
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
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/hosts/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
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
