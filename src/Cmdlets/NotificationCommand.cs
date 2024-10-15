using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
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
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.NotificationTemplate),
                     nameof(ResourceType.Job),
                     nameof(ResourceType.WorkflowJob),
                     nameof(ResourceType.SystemJob),
                     nameof(ResourceType.ProjectUpdate),
                     nameof(ResourceType.InventoryUpdate),
                     nameof(ResourceType.AdHocCommand))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public ulong Id { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
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

