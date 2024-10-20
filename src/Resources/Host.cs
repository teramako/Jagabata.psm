using System.Collections.Specialized;

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

    public class Host(ulong id,
                      ResourceType type,
                      string url,
                      RelatedDictionary related,
                      Host.Summary summaryFields,
                      DateTime created,
                      DateTime? modified,
                      string name,
                      string description,
                      ulong inventory,
                      bool enabled,
                      string instanceId,
                      string variables)
        : IHost, IResource<Host.Summary>
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromInventory(ulong inventoryId,
                                                                     NameValueCollection? query = null,
                                                                     bool getAll = false)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromInventorySource(ulong inventorySourceId,
                                                                           NameValueCollection? query = null,
                                                                           bool getAll = false)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindAllFromGroup(ulong groupId,
                                                                    NameValueCollection? query = null,
                                                                    bool getAll = false)
        {
            var path = $"{Group.PATH}{groupId}/all_hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Host> FindFromGroup(ulong groupId,
                                                                 NameValueCollection? query = null,
                                                                 bool getAll = false)
        {
            var path = $"{Group.PATH}{groupId}/hosts/";
            await foreach (var result in RestAPI.GetResultSetAsync<Host>(path, query, getAll))
            {
                foreach (var host in result.Contents.Results)
                {
                    yield return host;
                }
            }
        }

        public record Summary(InventorySummary Inventory,
                              HostLastJobSummary? LastJob,
                              LastJobHostSummary? LastJobHostSummary,
                              Capability UserCapabilities,
                              ListSummary<GroupSummary> Groups,
                              HostRecentJobSummary[] RecentJobs);


        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;

        public string Description { get; } = description;

        public ulong Inventory { get; } = inventory;

        public bool Enabled { get; } = enabled;

        public string InstanceId { get; } = instanceId;

        public string Variables { get; } = variables;
    }
}
