using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "JobEvent")]
    [OutputType(typeof(IJobEventBase))]
    public class FindJobEventCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Job),
                     nameof(ResourceType.ProjectUpdate),
                     nameof(ResourceType.InventoryUpdate),
                     nameof(ResourceType.SystemJob),
                     nameof(ResourceType.AdHocCommand),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.Group))]
        public ResourceType Type { get; set; } = ResourceType.None;

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; } = 0;

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Job,
                ResourceType.ProjectUpdate,
                ResourceType.InventoryUpdate,
                ResourceType.SystemJob,
                ResourceType.AdHocCommand,
                ResourceType.Host,
                ResourceType.Group
        ])]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        public SwitchParameter AdHocCommandEvent { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["counter"];

        protected override void ProcessRecord()
        {
            if (Id > 0 && Type > 0)
                Resource = new Resource(Type, Id);

            Query.Clear();
            SetupCommonQuery();

            switch (Resource.Type)
            {
                case ResourceType.Job:
                    Find<JobEvent>($"{JobTemplateJob.PATH}{Resource.Id}/job_events/");
                    break;
                case ResourceType.Host:
                    if (AdHocCommandEvent)
                    {
                        Find<AdHocCommandJobEvent>($"{Host.PATH}{Resource.Id}/ad_hoc_command_events/");
                    }
                    else
                    {
                        if (OrderBy.Length == 1 && OrderBy[0] == "counter")
                        {
                            Query.Set("order_by", "-job,counter");
                        }
                        Find<JobEvent>($"{Host.PATH}{Resource.Id}/job_events/");
                    }
                    break;
                case ResourceType.Group:
                    if (OrderBy.Length == 1 && OrderBy[0] == "counter")
                    {
                        Query.Set("order_by", "-job,counter");
                    }
                    Find<JobEvent>($"{Group.PATH}{Resource.Id}/job_events/");
                    break;
                case ResourceType.ProjectUpdate:
                    Find<ProjectUpdateJobEvent>($"{ProjectUpdateJob.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.InventoryUpdate:
                    Find<InventoryUpdateJobEvent>($"{InventoryUpdateJob.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.SystemJob:
                    Find<SystemJobEvent>($"{SystemJob.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.AdHocCommand:
                    Find<AdHocCommandJobEvent>($"{AdHocCommand.PATH}{Resource.Id}/events/");
                    break;
            }
        }
    }
}
