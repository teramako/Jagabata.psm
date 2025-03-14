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
    /// Implement API: <c>/api/v2/inventries/{id}/tree/</c>.
    /// </summary>
    public Group.Tree[]? GetGroupTree()
    {
        return Related.TryGetPath("tree", out var path)
            ? RestAPI.Get<Group.Tree[]>(path)
            : null;
    }

    /// <summary>
    /// Get an inventory script.
    /// Implement API: <c>/api/v2/inventries/{id}/script/</c>.
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
    /// Implement API: <c>/api/v2/inventries/{id}/script/</c>.
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
    /// </summary>
    /// <param name="count">Number of activity streams to retrieve</param>.
    public IEnumerable<ActivityStream> GetRecentActivityStream(int count = 20)
    {
        return Related.TryGetPath("activity_stream", out var path)
            ? RestAPI.GetResultSet<ActivityStream>($"{path}?order_by=-timestamp&page_size={count}")
            : [];
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
