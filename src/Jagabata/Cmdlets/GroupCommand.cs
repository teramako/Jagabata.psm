using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Group")]
    [OutputType(typeof(Group))]
    public class GetGroupCommand : GetCommandBase<Group>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Group")]
    [OutputType(typeof(Group))]
    public class FindGroupCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.Inventory, ResourceType.Group, ResourceType.InventorySource, ResourceType.Host
        )]
        [ResourceCompletions(
            ResourceType.Inventory, ResourceType.Group, ResourceType.InventorySource, ResourceType.Host
        )]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        /// <summary>
        /// List only root(Top-level) groups.
        /// Only affected for an Inventory Type
        /// </summary>
        [Parameter()]
        public SwitchParameter OnlyRoot { get; set; }

        /// <summary>
        /// List only directly member groups.
        /// Only affected for a Host Type
        /// </summary>
        [Parameter()]
        public SwitchParameter OnlyParnets { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "inventory", "variables",
                           "parents", "created_by", "modified_by", "children", "hosts")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/" + (OnlyRoot ? "root_groups/" : "groups/"),
                ResourceType.Group => $"{Group.PATH}{Resource.Id}/children/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/groups/",
                ResourceType.Host => $"{Host.PATH}{Resource.Id}/" + (OnlyParnets ? "groups/" : "all_groups/"),
                _ => Group.PATH
            };
            Find<Group>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Group", SupportsShouldProcess = true)]
    [OutputType(typeof(Group))]
    public class NewGroupCommand : NewCommandBase<Group>
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Inventory)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Inventory)]
        public ulong Inventory { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation]
        public string? Variables { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "inventory", Inventory },
            };
            if (Description is not null)
                sendData.Add("description", Description);
            if (Variables is not null)
                sendData.Add("variables", Variables);

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

    [Cmdlet(VerbsData.Update, "Group", SupportsShouldProcess = true)]
    [OutputType(typeof(Group))]
    public class UpdateGroupCommand : UpdateCommandBase<Group>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
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

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Variables is not null)
                sendData.Add("variables", Variables);

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

    [Cmdlet(VerbsLifecycle.Register, "Group", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterGroupCommand : RegistrationCommandBase<Group>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public ulong To { get; set; }

        protected override void ProcessRecord()
        {
            var parentGroup = new Resource(ResourceType.Group, To);
            var path = $"{Group.PATH}{parentGroup.Id}/children/";
            Register(path, Id, parentGroup);
        }
    }

    [Cmdlet(VerbsLifecycle.Unregister, "Group", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class UnregisterGroupCommand : RegistrationCommandBase<Group>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public ulong From { get; set; }

        protected override void ProcessRecord()
        {
            var parentGroup = new Resource(ResourceType.Group, From);
            var path = $"{Group.PATH}{parentGroup.Id}/children/";
            Unregister(path, Id, parentGroup);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Group", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveGroupCommand : RemoveCommandBase<Group>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Group)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Group)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}

