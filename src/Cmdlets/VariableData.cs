using System.Management.Automation;
using AWX.Resources;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "VariableData")]
    [OutputType(typeof(Dictionary<string, object?>))]
    public class GetVariableDataCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [ValidateSet(nameof(ResourceType.Inventory),
                     nameof(ResourceType.Group),
                     nameof(ResourceType.Host))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/variable_data/",
                ResourceType.Group => $"{Group.PATH}{Id}/variable_data/",
                ResourceType.Host => $"{Host.PATH}{Id}/variable_data/",
                _ => throw new ArgumentException($"Unkown Resource Type: {Type}")
            };
            var variableData = GetResource<Dictionary<string, object?>>(path);
            WriteObject(variableData, false);
        }
    }
}
