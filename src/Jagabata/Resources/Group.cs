using System.Collections.Specialized;

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
        /// Get the recent activity stream for this resource
        /// </summary>
        /// <param name="count">Number of activity streams to retrieve</param>.
        public IEnumerable<ActivityStream> GetRecentActivityStream(int count = 20)
        {
            return Related.TryGetPath("activity_stream", out var path)
                ? RestAPI.GetResultSet<ActivityStream>($"{path}?order_by=-timestamp&page_size={count}")
                : [];
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
        /// Retrieve a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Group> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Group>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Groups.<br/>
        /// API Path: <c>/api/v2/groups/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(PATH, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }
        /// <summary>
        /// List Groups for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/groups/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindFromInventory(ulong inventoryId,
                                                                      NameValueCollection? query = null,
                                                                      bool getAll = false)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/groups/";
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }
        /// <summary>
        /// List Root Groups for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/root_groups/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindOnlyRootFromInventory(ulong inventoryId,
                                                                              NameValueCollection? query = null,
                                                                              bool getAll = false)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/root_groups/";
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }
        /// <summary>
        /// List Groups for an Inventory Source.<br/>
        /// API Path: <c>/api/v2/inventory_sources/<paramref name="inventorySourceId"/>/groups/</c>
        /// </summary>
        /// <param name="inventorySourceId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindFromInventorySource(ulong inventorySourceId,
                                                                            NameValueCollection? query = null,
                                                                            bool getAll = false)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/groups/";
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }
        /// <summary>
        /// List All Groups for a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/all_hosts/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindAllFromHost(ulong hostId,
                                                                    NameValueCollection? query = null,
                                                                    bool getAll = false)
        {
            var path = $"{Host.PATH}{hostId}/all_groups/";
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
                }
            }
        }
        /// <summary>
        /// List Groups for a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/hosts/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Group> FindFromHost(ulong hostId,
                                                                 NameValueCollection? query = null,
                                                                 bool getAll = false)
        {
            var path = $"{Host.PATH}{hostId}/groups/";
            await foreach (var result in RestAPI.GetResultSetAsync<Group>(path, query, getAll))
            {
                foreach (var group in result.Contents.Results)
                {
                    yield return group;
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
