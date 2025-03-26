using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Notification")]
    [OutputType(typeof(Notification))]
    public class GetNotificationCommand : GetCommandBase<Notification>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Notification)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Notification)]
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

    [Cmdlet(VerbsCommon.Find, "Notification")]
    [OutputType(typeof(Notification))]
    public class FindNotificationCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.NotificationTemplate, ResourceType.Job, ResourceType.WorkflowJob, ResourceType.SystemJob,
            ResourceType.ProjectUpdate, ResourceType.InventoryUpdate, ResourceType.AdHocCommand
        )]
        [ResourceCompletions(
            ResourceType.NotificationTemplate, ResourceType.Job, ResourceType.WorkflowJob, ResourceType.SystemJob,
            ResourceType.ProjectUpdate, ResourceType.InventoryUpdate, ResourceType.AdHocCommand
        )]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "notification_template", "error", "status",
                           "notifications_sent", "notification_type", "recipients", "subject", "body")]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.NotificationTemplate => $"{NotificationTemplate.PATH}{Resource.Id}/notifications/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{Resource.Id}/notifications/",
                ResourceType.WorkflowJob => $"{WorkflowJobBase.PATH}{Resource.Id}/notifications/",
                ResourceType.SystemJob => $"{SystemJobBase.PATH}{Resource.Id}/notifications/",
                ResourceType.ProjectUpdate => $"{ProjectUpdateJobBase.PATH}{Resource.Id}/notifications/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJobBase.PATH}{Resource.Id}/notifications/",
                ResourceType.AdHocCommand => $"{AdHocCommandBase.PATH}{Resource.Id}/notifications/",
                _ => Notification.PATH
            };
            Find<Notification>(path);
        }
    }
}

