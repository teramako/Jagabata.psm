using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "AdHocCommandJob")]
    [OutputType(typeof(AdHocCommand.Detail))]
    public class GetAdHocCommandJobCommand : GetCommandBase<AdHocCommand.Detail>
    {
        protected override ResourceType AcceptType => ResourceType.AdHocCommand;

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "AdHocCommandJob", DefaultParameterSetName = "All")]
    [OutputType(typeof(AdHocCommand))]
    public class FindAdHocCommandJobCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Inventory),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.Group))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Inventory,
                ResourceType.Host,
                ResourceType.Group
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["!id"];


        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/ad_hoc_commands/",
                ResourceType.Host => $"{Host.PATH}{Id}/ad_hoc_commands/",
                ResourceType.Group => $"{Group.PATH}{Id}/ad_hoc_commands/",
                _ => AdHocCommand.PATH
            };
            Find<AdHocCommand>(path);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "AdHocCommandJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveAdHocCommandJobCommand : RemoveCommandBase<AdHocCommand>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.AdHocCommand])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    public abstract class LaunchAdHocCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Host", ValueFromPipeline = true, Position = 0)]
        public Host? Host { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Group", ValueFromPipeline = true, Position = 0)]
        public Group? Group { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Inventory", ValueFromPipeline = true, Position = 0)]
        public Inventory? Inventory { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "InventoryId", ValueFromPipeline = true, Position = 0)]
        public ulong InventoryId { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string ModuleName { get; set; } = string.Empty;

        [Parameter(Position = 2)]
        [AllowEmptyString]
        public string ModuleArgs { get; set; } = string.Empty;

        [Parameter(Mandatory = true, Position = 3)]
        public ulong Credential { get; set; }

        [Parameter(ParameterSetName = "Inventory")]
        [Parameter(ParameterSetName = "InventoryId")]
        public string Limit { get; set; } = string.Empty;

        [Parameter()]
        public SwitchParameter Check { get; set; }

        protected Hashtable SendData { get; set; } = [];
        protected override void BeginProcessing()
        {
            SendData.Add("module_name", ModuleName);
            SendData.Add("module_args", ModuleArgs);
            SendData.Add("credential", Credential);
            if (Check)
            {
                SendData.Add("job_type", "check");
            }
            if (!string.IsNullOrEmpty(Limit))
            {
                SendData.Add("limit", Limit);
            }
        }
        private string GetPath()
        {
            if (InventoryId > 0)
            {
                return $"{Inventory.PATH}{InventoryId}/ad_hoc_commands/";
            }
            else if (Inventory is not null)
            {
                return $"{Inventory.PATH}{Inventory.Id}/ad_hoc_commands/";
            }
            else if (Host is not null)
            {
                return $"{Host.PATH}{Host.Id}/ad_hoc_commands/";
            }
            else if (Group is not null)
            {
                return $"{Group.PATH}{Group.Id}/ad_hoc_commands/";
            }
            throw new ArgumentException();
        }
        protected AdHocCommand? Launch()
        {
            var apiResult = CreateResource<AdHocCommand>(GetPath(), SendData);
            return apiResult.Contents;
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "AdHocCommand")]
    [OutputType(typeof(AdHocCommand))]
    public class InvokeAdHocCommand : LaunchAdHocCommandBase
    {
        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            AdHocCommand? job = Launch();
            if (job is null)
            {
                return;
            }
            WriteVerbose($"Invoke AdHocCommand:{job.Name} => Job:[{job.Id}]");
            JobProgressManager.Add(job);
        }
        protected override void EndProcessing()
        {
            WaitJobs("Invoke AdHocCommand", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "AdHocCommand")]
    [OutputType(typeof(AdHocCommand))]
    public class StartAdHocCommand : LaunchAdHocCommandBase
    {
        protected override void ProcessRecord()
        {
            AdHocCommand? job = Launch();
            if (job is null)
            {
                return;
            }
            WriteVerbose($"Invoke AdHocCommand:{job.Name} => Job:[{job.Id}]");
            WriteObject(job, false);
        }
    }
}
