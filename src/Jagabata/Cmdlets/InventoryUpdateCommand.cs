using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "InventoryUpdateJob")]
    [OutputType(typeof(InventoryUpdateJob.Detail))]
    public class GetInventoryUpdateJobCommand : GetCommandBase<InventoryUpdateJob.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.InventoryUpdate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.InventoryUpdate)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "InventoryUpdateJob")]
    [OutputType(typeof(InventoryUpdateJob))]
    public class FindInventoryUpdateJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.ProjectUpdate, ResourceType.InventorySource)]
        [ResourceCompletions(ResourceType.ProjectUpdate, ResourceType.InventorySource)]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "unified_job_template", "launch_type", "status", "execution_environment", "failed",
                           "started", "finished", "canceled_on", "elapsed", "job_explanation", "execution_node",
                           "controller_node", "work_unit_id", "source", "source_path", "source_vars", "scm_branch",
                           "enabled_var", "enabled_value", "enabled_value", "overwrite", "overwrite_vars",
                           "timeout", "verbosity", "limit", "inventory", "inventory_source", "license_error",
                           "org_host_limit_error", "source_project_update", "instance_group", "scm_revision")]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.ProjectUpdate => $"{ProjectUpdateJobBase.PATH}{Resource.Id}/scm_inventory_updates/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/inventory_updates/",
                _ => InventoryUpdateJobBase.PATH
            };
            Find<InventoryUpdateJob>(path);
        }
    }

    public class LaunchInventoryUpdateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.Inventory, ResourceType.InventorySource)]
        [ResourceCompletions(ResourceType.Inventory, ResourceType.InventorySource)]
        public IResource Source { get; set; } = new Resource(0, 0);

        [Parameter(Mandatory = true, ParameterSetName = "Check")]
        public SwitchParameter Check { get; set; }

        protected void CheckCanUpdate(IResource source)
        {
            switch (source.Type)
            {
                case ResourceType.Inventory:
                    CheckCanUpdateInventory(source.Id);
                    break;
                case ResourceType.InventorySource:
                    CheckCanUpdateInventorySource(source.Id);
                    break;
            }
        }
        protected void CheckCanUpdateInventorySource(ulong id)
        {
            var res = GetResource<CanUpdateInventorySource>($"{InventorySource.PATH}{id}/update/");
            var psobject = new PSObject();
            psobject.Members.Add(new PSNoteProperty("Id", id));
            psobject.Members.Add(new PSNoteProperty("Type", ResourceType.InventorySource));
            psobject.Members.Add(new PSNoteProperty("CanUpdate", res.CanUpdate));
            WriteObject(psobject, false);
        }
        protected void CheckCanUpdateInventory(ulong id)
        {
            var results = GetResource<CanUpdateInventorySource[]>($"{Inventory.PATH}{id}/update_inventory_sources/");
            foreach (var res in results)
            {
                var psobject = new PSObject();
                psobject.Members.Add(new PSNoteProperty("Id", res.InventorySource));
                psobject.Members.Add(new PSNoteProperty("Type", ResourceType.InventorySource));
                psobject.Members.Add(new PSNoteProperty("CanUpdate", res.CanUpdate));
                WriteObject(psobject, false);
            }
        }
        protected bool TryUpdateInventorySource(ulong id, [MaybeNullWhen(false)] out InventoryUpdateJob.Detail job)
        {
            job = CreateResource<InventoryUpdateJob.Detail>($"{InventorySource.PATH}{id}/update/").Contents;
            return job is not null;
        }
        protected bool TryUpdateInventory(ulong id, [MaybeNullWhen(false)] out InventoryUpdateJob.Detail[] jobs)
        {
            jobs = CreateResource<InventoryUpdateJob.Detail[]>($"{Inventory.PATH}{id}/update_inventory_sources/").Contents;
            return jobs is not null;
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "InventoryUpdate", DefaultParameterSetName = "Launch")]
    [OutputType(typeof(InventoryUpdateJob), ParameterSetName = ["Launch"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["Check"])]
    public class InvokeInventoryUpdateCommand : LaunchInventoryUpdateCommandBase
    {
        [Parameter(ParameterSetName = "Launch")]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter(ParameterSetName = "Launch")]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (Check)
            {
                CheckCanUpdate(Source);
                return;
            }

            switch (Source.Type)
            {
                case ResourceType.Inventory:
                    if (TryUpdateInventory(Source.Id, out var jobs))
                    {
                        foreach (var updateJob in jobs)
                        {
                            WriteVerbose($"Update InventorySource:{updateJob.InventorySource} => Job:[{updateJob.Id}]");
                            JobProgressManager.Add(updateJob);
                        }
                    }
                    break;
                case ResourceType.InventorySource:
                    if (TryUpdateInventorySource(Source.Id, out var sourceUpdateJob))
                    {
                        WriteVerbose($"Update InventorySource:{sourceUpdateJob.InventorySource} => Job:[{sourceUpdateJob.Id}]");
                        JobProgressManager.Add(sourceUpdateJob);
                    }
                    break;
            }
        }
        protected override void EndProcessing()
        {
            if (Check)
            {
                return;
            }
            WaitJobs("Update InventorySource", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "InventoryUpdate", DefaultParameterSetName = "Launch")]
    [OutputType(typeof(InventoryUpdateJob.Detail), ParameterSetName = ["Launch"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["Check"])]
    public class StartInventoryUpdateCommand : LaunchInventoryUpdateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (Check)
            {
                CheckCanUpdate(Source);
                return;
            }

            switch (Source.Type)
            {
                case ResourceType.Inventory:
                    if (TryUpdateInventory(Source.Id, out var jobs))
                    {
                        WriteObject(jobs, true);
                    }
                    break;
                case ResourceType.InventorySource:
                    if (TryUpdateInventorySource(Source.Id, out var job))
                    {
                        WriteObject(job, false);
                    }
                    break;
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "InventoryUpdateJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveInventoryUpdateCommand : RemoveCommandBase<InventoryUpdateJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.InventoryUpdate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.InventoryUpdate)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
