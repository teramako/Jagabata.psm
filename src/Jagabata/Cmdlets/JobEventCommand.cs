using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "JobEvent")]
    [OutputType(typeof(IJobEventBase))]
    public class FindJobEventCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.Job, ResourceType.ProjectUpdate, ResourceType.InventoryUpdate, ResourceType.SystemJob,
            ResourceType.AdHocCommand, ResourceType.Host, ResourceType.Group
        )]
        [ResourceCompletions(
            ResourceType.Job, ResourceType.ProjectUpdate, ResourceType.InventoryUpdate, ResourceType.SystemJob,
            ResourceType.AdHocCommand, ResourceType.Host, ResourceType.Group
        )]
        [Alias("associatedWith", "r")]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        public SwitchParameter AdHocCommandEvent { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "event", "counter", "event_data", "failed", "changed",
                           "uuid", "stdout", "start_line", "end_line", "verbosity")]
        public override string[] OrderBy { get; set; } = ["counter"];

        protected override void ProcessRecord()
        {
            Query.Clear();
            SetupCommonQuery();

            switch (Resource.Type)
            {
                case ResourceType.Job:
                    Find<JobEvent>($"{JobTemplateJobBase.PATH}{Resource.Id}/job_events/");
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
                    Find<ProjectUpdateJobEvent>($"{ProjectUpdateJobBase.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.InventoryUpdate:
                    Find<InventoryUpdateJobEvent>($"{InventoryUpdateJobBase.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.SystemJob:
                    Find<SystemJobEvent>($"{SystemJobBase.PATH}{Resource.Id}/events/");
                    break;
                case ResourceType.AdHocCommand:
                    Find<AdHocCommandJobEvent>($"{AdHocCommandBase.PATH}{Resource.Id}/events/");
                    break;
            }
        }
    }
}
