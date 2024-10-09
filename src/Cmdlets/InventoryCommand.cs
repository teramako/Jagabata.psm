using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Inventory")]
    [OutputType(typeof(Inventory))]
    public class GetInventoryCommand : GetCommandBase<Inventory>
    {
        protected override ResourceType AcceptType => ResourceType.Inventory;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Inventory", DefaultParameterSetName = "All")]
    [OutputType(typeof(Inventory))]
    public class FindInventoryCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.Organization),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.Host))]
        public override ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public override ulong Id { get; set; }

        [Parameter()]
        public InventoryKind Kind { get; set; } = InventoryKind.All;

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        public enum InventoryKind
        {
            All, Normal, Smart, Constructed
        }

        protected override void BeginProcessing()
        {
            switch (Kind)
            {
                case InventoryKind.Normal:
                    Query.Add("kind", "");
                    break;
                case InventoryKind.Smart:
                case InventoryKind.Constructed:
                    Query.Add("kind", Kind.ToString().ToLowerInvariant());
                    break;
                case InventoryKind.All:
                default:
                    break;
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Id}/inventories/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/input_inventories/",
                ResourceType.Host => $"{Host.PATH}{Id}/smart_inventories/",
                _ => Inventory.PATH
            };
            foreach (var resultSet in GetResultSet<Inventory>(path, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "Inventory", SupportsShouldProcess = true, DefaultParameterSetName = "NormalInventory")]
    [OutputType(typeof(Inventory))]
    public class NewInventoryCommand : NewCommandBase<Inventory>
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong Organization { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation]
        public string? Variables { get; set; }

        [Parameter()]
        public SwitchParameter PreventInstanceGroupFallback { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SmartInventory")]
        public SwitchParameter AsSmartInventory { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SmartInventory")]
        public string HostFilter { get; set; } = string.Empty;

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "organization", Organization },
            };
            if (Description != null)
                sendData.Add("description", Description);
            if (Variables != null)
                sendData.Add("variables", Variables);
            if (PreventInstanceGroupFallback)
                sendData.Add("prevent_instance_group_fallback", true);

            if (AsSmartInventory)
            {
                sendData.Add("kind", "smart");
                sendData.Add("host_filter", HostFilter);
            }

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsData.Update, "Inventory", SupportsShouldProcess = true)]
    [OutputType(typeof(Inventory))]
    public class UpdateInventoryCommand : UpdateCommandBase<Inventory>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation]
        public string? Variables { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? HostFilter { get; set; }

        [Parameter()]
        public bool? PreventInstanceGroupFallback { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description != null)
                sendData.Add("description", Description);
            if (Variables != null)
                sendData.Add("variables", Variables);
            if (HostFilter != null)
                sendData.Add("host_filter", HostFilter);
            if (PreventInstanceGroupFallback != null)
                sendData.Add("prevent_instance_group_fallback", PreventInstanceGroupFallback);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Inventory", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public class RemoveInventoryCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong Id { get; set; }

        [Parameter()]
        public SwitchParameter Force { get; set; }

        private void Delete(ulong id)
        {
            if (Force || ShouldProcess($"Inventory [{id}]", "Delete completely"))
            {
                try
                {
                    var apiResult = DeleteResource($"{Inventory.PATH}{id}/");
                    if (apiResult?.IsSuccessStatusCode ?? false)
                    {
                        WriteVerbose($"Inventory {id} is deleted.");
                    }
                }
                catch (RestAPIException) { }
            }
        }
        protected override void ProcessRecord()
        {
            Delete(Id);
        }
    }
}
