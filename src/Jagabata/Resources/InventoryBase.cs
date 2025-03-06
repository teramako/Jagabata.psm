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
