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
    public class Group(ulong id,
                       ResourceType type,
                       string url,
                       RelatedDictionary related,
                       SummaryFieldsDictionary summaryFields,
                       DateTime created,
                       DateTime? modified,
                       string name,
                       string description,
                       ulong inventory,
                       string variables)
        : IGroup, IResource
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

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong Inventory { get; } = inventory;
        public string Variables { get; } = variables;
    }
}
