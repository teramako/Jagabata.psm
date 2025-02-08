using System.Management.Automation;
using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "VariableData")]
    [OutputType(typeof(Dictionary<string, object?>))]
    public class GetVariableDataCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Inventory, ResourceType.Group, ResourceType.Host)]
        [ResourceCompletions(ResourceType.Inventory, ResourceType.Group, ResourceType.Host)]
        [Alias("associatedWith", "r")]
        public IResource Resource { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/variable_data/",
                ResourceType.Group => $"{Group.PATH}{Resource.Id}/variable_data/",
                ResourceType.Host => $"{Host.PATH}{Resource.Id}/variable_data/",
                _ => throw new ArgumentException($"Unkown Resource Type: {Resource.Type}")
            };
            var variableData = GetResource<Dictionary<string, object?>>(path);
            WriteObject(variableData, false);
        }
    }
}
