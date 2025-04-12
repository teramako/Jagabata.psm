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
    /// Find the activity stream for this resource
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/activity_stream/</c>
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
    /// Implement API: <c>/api/v2/inventories/{id}/activity_stream/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
    public ActivityStream[] FindActivityStream(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
    }

    /// <summary>
    /// Find groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/groups/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Group[] FindChildGroups(string? searchWords = null,
                                   string orderBy = "name",
                                   ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<Group>("groups",
                                                  searchWords,
                                                  orderBy,
                                                  pageSize)];
    }

    /// <summary>
    /// Find groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/groups/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Group[] FindChildGroups(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<Group>("groups", query)];
    }

    /// <summary>
    /// Find hosts associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/hosts/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
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
    /// Find hosts associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/hosts/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Host[] FindChildHosts(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<Host>("hosts", query)];
    }

    /// <summary>
    /// Find job templates associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/job_templates/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public JobTemplate[] FindJobTemplates(string? searchWords = null,
                                          string orderBy = "name",
                                          ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<JobTemplate>("job_templates",
                                                        searchWords,
                                                        orderBy,
                                                        pageSize)];
    }

    /// <summary>
    /// Find job templates associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/job_templates/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public JobTemplate[] FindJobTemplates(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<JobTemplate>("job_templates", query)];
    }

    /// <summary>
    /// Find ad hoc commands associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/ad_hoc_commands/</c>
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
    /// Find ad hoc commands associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/ad_hoc_commands/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public AdHocCommand[] FindAdHocCommandJobs(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<AdHocCommand>("ad_hoc_commands", query)];
    }

    /// <summary>
    /// Find the access list related to this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/access_list/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
    /// <param name="pageSize">Max number to retrieve</param>.
    public User[] FindAccessList(string? searchWords = null,
                                 string orderBy = "username",
                                 ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<User>("access_list",
                                                 searchWords,
                                                 orderBy,
                                                 pageSize)];
    }

    /// <summary>
    /// Find the access list related to this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/access_list/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
    public User[] FindAccessList(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<User>("access_list", query)];
    }

    /// <summary>
    /// Get the object roles of this inventory
    /// </summary>
    /// <remarks>
    /// This is almost same as:
    /// <code>thisObject.SummaryFields["ObjectRoles"]</code>
    /// </remarks>
    public ObjectRoleSummary[] GetObjectRoles()
    {
        return SummaryFields.TryGetValue<Dictionary<string, ObjectRoleSummary>>("ObjectRoles", out var dict)
            ? [.. dict.Values]
            : [];
    }

    /// <summary>
    /// Find instance groups related to this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/instance_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
    /// <param name="pageSize">Max number to retrieve</param>.
    public InstanceGroup[] FindInstanceGroups(string? searchWords = null,
                                              string orderBy = "name",
                                              ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups",
                                                          searchWords,
                                                          orderBy,
                                                          pageSize)];
    }

    /// <summary>
    /// Find instance groups related to this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/instance_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
    public InstanceGroup[] FindInstanceGroups(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups", query)];
    }

    /// <summary>
    /// Find labels associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/labels/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Label[] FindLabels(string? searchWords = null,
                              string orderBy = "name",
                              ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<Label>("labels",
                                                  searchWords,
                                                  orderBy,
                                                  pageSize)];
    }

    /// <summary>
    /// Find labels associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/labels/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Label[] FindLabels(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<Label>("labels", query)];
    }

    /// <summary>
    /// Find inventory sources associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/inventory_sources/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
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
    /// Find inventory sources associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/inventory_sources/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public InventorySource[] FindInventorySources(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<InventorySource>("inventory_sources", query)];
    }

    /// <summary>
    /// Find root (top-level) groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/root_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="searchWords"></param>
    /// <param name="orderBy">Name(s) of sort key</param>
    /// <param name="pageSize">Max number of groups to retrieve</param>
    public Group[] FindRootGroups(string? searchWords = null,
                                  string orderBy = "name",
                                  ushort pageSize = 20)
    {
        return [.. FindResultsByRelatedKey<Group>("root_groups",
                                                  searchWords,
                                                  orderBy,
                                                  pageSize)];
    }

    /// <summary>
    /// Find root (top-level) groups associated with this inventory
    /// <para>
    /// Implement API: <c>/api/v2/inventories/{id}/root_groups/</c>
    /// </para>
    /// </summary>
    /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
    public Group[] FindRootGroups(HttpQuery query)
    {
        return [.. FindResultsByRelatedKey<Group>("root_groups", query)];
    }

    /// <summary>
    /// Get the organization related this inventory.
    /// </summary>
    public Organization? GetOrganization()
    {
        return Related.TryGetPath("organization", out var path)
            ? RestAPI.Get<Organization>(path)
            : null;
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
