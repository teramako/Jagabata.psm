using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Notification")]
    [OutputType(typeof(Notification))]
    public class GetNotificationCommand : GetCommandBase<Notification>
    {
        protected override ResourceType AcceptType => ResourceType.Notification;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Notification", DefaultParameterSetName = "All")]
    [OutputType(typeof(Notification))]
    public class FindNotificationCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.NotificationTemplate),
                     nameof(ResourceType.Job),
                     nameof(ResourceType.WorkflowJob),
                     nameof(ResourceType.SystemJob),
                     nameof(ResourceType.ProjectUpdate),
                     nameof(ResourceType.InventoryUpdate),
                     nameof(ResourceType.AdHocCommand))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineVariable", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.NotificationTemplate,
                ResourceType.Job,
                ResourceType.WorkflowJob,
                ResourceType.SystemJob,
                ResourceType.ProjectUpdate,
                ResourceType.InventoryUpdate,
                ResourceType.AdHocCommand
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
                ResourceType.NotificationTemplate => $"{NotificationTemplate.PATH}{Id}/notifications/",
                ResourceType.Job => $"{JobTemplateJob.PATH}{Id}/notifications/",
                ResourceType.WorkflowJob => $"{WorkflowJob.PATH}{Id}/notifications/",
                ResourceType.SystemJob => $"{SystemJob.PATH}{Id}/notifications/",
                ResourceType.ProjectUpdate => $"{ProjectUpdateJob.PATH}{Id}/notifications/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJob.PATH}{Id}/notifications/",
                ResourceType.AdHocCommand => $"{AdHocCommand.PATH}{Id}/notifications/",
                _ => Notification.PATH
            };
            Find<Notification>(path);
        }
    }
}

