using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "AdHocCommandJob")]
    [OutputType(typeof(AdHocCommand.Detail))]
    public class GetAdHocCommandJobCommand : GetCommandBase<AdHocCommand.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.AdHocCommand)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.AdHocCommand)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "AdHocCommandJob")]
    [OutputType(typeof(AdHocCommand))]
    public class FindAdHocCommandJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.Inventory, ResourceType.Host, ResourceType.Group)]
        [ResourceCompletions(ResourceType.Inventory, ResourceType.Host, ResourceType.Group)]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "launch_type", "status", "execution_environment",
                           "failed", "started", "finished", "canceled_on", "elapsed", "job_explanation",
                           "execution_node", "controller_node", "work_unit_id", "job_type", "inventory", "limit",
                           "credential", "module_name", "module_args", "forks", "verbosity", "become_enabled",
                           "diff_mode", "hosts", "organization", "schedule", "created_by", "modified_by",
                           "instance_group", "labels")]
        public override string[] OrderBy { get; set; } = ["!id"];


        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/ad_hoc_commands/",
                ResourceType.Host => $"{Host.PATH}{Resource.Id}/ad_hoc_commands/",
                ResourceType.Group => $"{Group.PATH}{Resource.Id}/ad_hoc_commands/",
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
        [ResourceIdTransformation(ResourceType.AdHocCommand)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    public abstract class LaunchAdHocCommandBase : LaunchJobCommandBase
    {
        /// <summary>
        /// Execution target, <c>Inventory</c>, <c>Group</c> or <c>Host</c>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.Host, ResourceType.Host, ResourceType.Inventory)]
        [ResourceCompletions(ResourceType.Host, ResourceType.Host, ResourceType.Inventory)]
        [Alias("remote", "r")]
        public IResource Target { get; set; } = new Resource(0, 0);

        [Parameter(Mandatory = true, Position = 1)]
        [ArgumentCompletions(
            "command", "shell", "yum", "apt", "apt_key", "apt_repository", "apt_rpm", "service",
            "group", "user", "mount", "ping", "selinux", "setup", "win_ping", "win_service",
            "win_updates", "win_group", "win_user"
        )]
        public string ModuleName { get; set; } = string.Empty;

        [Parameter(Position = 2)]
        [AllowEmptyString]
        public string ModuleArgs { get; set; } = string.Empty;

        [Parameter(Mandatory = true, Position = 3)]
        [ResourceIdTransformation(ResourceType.Credential)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Credential)]
        public ulong Credential { get; set; }

        /// <summary>
        /// Affected only when the target is <c>Inventory</c>
        /// </summary>
        [Parameter()]
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
            if (Target.Type == ResourceType.Inventory && !string.IsNullOrEmpty(Limit))
            {
                SendData.Add("limit", Limit);
            }
        }
        private string GetPath()
        {
            return Target.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{Target.Id}/ad_hoc_commands/",
                ResourceType.Host => $"{Host.PATH}{Target.Id}/ad_hoc_commands/",
                ResourceType.Group => $"{Group.PATH}{Target.Id}/ad_hoc_commands/",
                _ => throw new ArgumentException(),
            };
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
