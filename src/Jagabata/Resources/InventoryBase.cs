using System.Web;

namespace Jagabata.Resources;

public interface IInventory
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
}

public abstract class InventoryBase : ResourceBase, IInventory
{
    public abstract DateTime Created { get; }
    public abstract DateTime? Modified { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ulong Organization { get; }
    public abstract string Kind { get; }
    public abstract string Variables { get; }
    public abstract bool HasActiveFailures { get; }
    public abstract int TotalHosts { get; }
    public abstract int HostsWithActiveFailures { get; }
    public abstract int TotalGroups { get; }
    public abstract bool HasInventorySources { get; }
    public abstract int TotalInventorySources { get; }
    public abstract int InventorySourcesWithFailures { get; }
    public abstract bool PendingDeletion { get; }
    public abstract bool PreventInstanceGroupFallback { get; }

    /// <summary>
    /// Desrialize <see cref="Variables" /> to Dictionary
    /// </summary>
    public Dictionary<string, object?> GetVariables()
    {
        return Yaml.DeserializeToDict(Variables);
    }

    /// <summary>
    /// Get  a hierarchical view of groups assiciated with this inventory.
    /// Implement API: <c>/api/v2/inventories/{id}/tree/</c>.
    /// </summary>
    public Group.Tree[]? GetGroupTree()
    {
        return Related.TryGetPath("tree", out var path)
            ? RestAPI.Get<Group.Tree[]>(path)
            : null;
    }

    /// <summary>
    /// Get an inventory script.
    /// Implement API: <c>/api/v2/inventories/{id}/script/</c>.
    /// </summary>
    /// <param name="includeHostVars">
    /// Include all host variables.
    /// The <c>['_meta']['hostvars']</c> object in the response contains an entry for each host with its variables.
    /// </param>
    /// <param name="includeDisabled">
    /// By default, the inventory script will only return hosts that are enabled in the inventory.
    /// This feature returns all hosts (including disabled ones).
    /// </param>
    /// <param name="includeTowerVars">
    /// Add variables to the hostvars of each host that specifies its enabled state and database ID.
    /// </param>
    public Dictionary<string, object?> GetInventoryScript(bool includeHostVars = false,
                                                          bool includeDisabled = false,
                                                          bool includeTowerVars = false)
    {
        if (!Related.TryGetPath("script", out var path))
            return [];

        var query = HttpUtility.ParseQueryString("");
        if (includeHostVars)
            query.Add("hostvars", "1");
        if (includeDisabled)
            query.Add("all", "1");
        if (includeTowerVars)
            query.Add("towervars", "1");
        return RestAPI.Get<Dictionary<string, object?>>(query.Count == 0 ? path : $"{path}?{query}");
    }

    /// <summary>
    /// Get host variables for the specified <paramref name="hostName"/> from the inventory script.
    /// Implement API: <c>/api/v2/inventories/{id}/script/</c>.
    /// </summary>
    /// <param name="hostName">Hostname belonging to the inventory</param>.
    public Dictionary<string, object?> GetInventoryScript(string hostName)
    {
        return Related.TryGetPath("script", out var path)
            ? RestAPI.Get<Dictionary<string, object?>>($"{path}?host={hostName}")
            : [];
    }

    /// <summary>
    /// Get the recent activity stream for this resource
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/activity_stream/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="pageSize">Max number of activity streams to retrieve</param>.
    public ActivityStream[] GetRecentActivityStream(string? searchWords = null, ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", searchWords, "-timestamp", pageSize)];
    }

    /// <summary>
    /// Get the recent activity stream for this resource
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/activity_stream/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
    public ActivityStream[] GetRecentActivityStream(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
    }

    /// <summary>
    /// Get a list of groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/groups/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Group[] GetChildGroups(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<Group>("groups", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get a list of groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/groups/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Group[] GetChildGroups(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<Group>("groups", query)];
    }

    /// <summary>
    /// Get a list of hosts associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/hosts/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Host[] GetChildHosts(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<Host>("hosts", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get a list of hosts associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/hosts/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Host[] GetChildHosts(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<Host>("hosts", query)];
    }

    /// <summary>
    /// Get a list of job templates associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/job_templates/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public JobTemplate[] GetJobTemplates(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<JobTemplate>("job_templates", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get a list of job templates associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/job_templates/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public JobTemplate[] GetJobTemplates(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<JobTemplate>("job_templates", query)];
    }

    /// <summary>
    /// Get a list of ad hoc commands associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/ad_hoc_commands/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public AdHocCommand[] GetAdHocCommandJobs(string? searchWords = null, string orderBy = "-id", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<AdHocCommand>("ad_hoc_commands", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get a list of ad hoc commands associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/ad_hoc_commands/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public AdHocCommand[] GetAdHocCommandJobs(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<AdHocCommand>("ad_hoc_commands", query)];
    }

    /// <summary>
    /// Get the access list related to this organization
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/access_list/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
    /// <param name="pageSize">Max number to retrieve</param>.
    public User[] GetAccessList(string? searchWords = null, string orderBy = "username", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<User>("access_list", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get the access list related to this organization
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/access_list/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
    public User[] GetAccessList(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<User>("access_list", query)];
    }

    /// <summary>
    /// Get a list of root (top-level) groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/root_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Group[] GetRootGroups(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
    {
        return [.. GetResultsByRelatedKey<Group>("root_groups", searchWords, orderBy, pageSize)];
    }

    /// <summary>
    /// Get a list of root (top-level) groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/root_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Group[] GetRootGroups(HttpQuery query)
    {
        return [.. GetResultsByRelatedKey<Group>("root_groups", query)];
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(Kind) ? $"{Type}:{Id}:{Name}" : $"{Type}:{Id}:{Kind}:{Name}";
    }

    protected override CacheItem GetCacheItem()
    {
        return new CacheItem(Type, Id, Name, Description)
        {
            Metadata = {
                    ["Kind"] = Kind
                }
        };
    }
}
