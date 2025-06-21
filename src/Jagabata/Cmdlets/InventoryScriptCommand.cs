using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets;

/// <summary>
/// GET <c>/api/v2/inventories/{id}/script/</c>
/// </summary>
[Cmdlet(VerbsCommon.Get, "InventoryScript", DefaultParameterSetName = "InventoryScript")]
[OutputType(typeof(Dictionary<string, object?>))]
public class GetInventoryScriptCommand : GetCommandBase<Dictionary<string, object?>>
{
    [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
    [ResourceIdTransformation(ResourceType.Inventory)]
    [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Inventory)]
    [Alias("inventory")]
    public override ulong[] Id { get; set; } = [];

    /// <summary>
    /// Include all host variables.
    /// The <c>['_meta']['hostvars']</c> object in the response contains an entry for each host with its variables.
    /// </summary>
    [Parameter(ParameterSetName = "InventoryScript")]
    [Alias("hostvars")]
    public SwitchParameter IncludeHostVars { get; set; }

    /// <summary>
    /// By default, the inventory script will only return hosts that are enabled in the inventory.
    /// This feature returns all hosts (including disabled ones).
    /// </summary>
    [Parameter(ParameterSetName = "InventoryScript")]
    [Alias("all")]
    public SwitchParameter IncludeDisabled { get; set; }

    /// <summary>
    /// Add variables to the hostvars of each host that specifies its enabled state and database ID.
    /// </summary>
    [Parameter(ParameterSetName = "InventoryScript")]
    [Alias("towervars")]
    public SwitchParameter IncludeTowerVars { get; set; }

    /// <summary>
    /// Get host variables for the specified hostname from the inventory script.
    /// </summary>
    [Parameter(ParameterSetName = "HostVariables", Mandatory = true, Position = 1)]
    public string? Hostname { get; set; }

    protected override string ApiPath => Inventory.PATH;

    protected override void BeginProcessing()
    {
        if (Hostname is not null)
        {
            Query.Add("host", Hostname);
        }
        else
        {
            if (IncludeHostVars)
                Query.Set("hostvars", "1");
            if (IncludeDisabled)
                Query.Set("all", "1");
            if (IncludeTowerVars)
            {
                Query.Set("hostvars", "1");
                Query.Set("towervars", "1");
            }
        }
    }
    protected override void ProcessRecord()
    {
        foreach (var result in GetResource("script/"))
        {
            WriteObject(result, false);
        }
    }
}
