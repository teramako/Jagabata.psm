using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ActivityStream")]
    [OutputType(typeof(ActivityStream))]
    public class GetActivityStreamCommand : GetCommandBase<ActivityStream>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ActivityStream])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.ActivityStream)]
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

    [Cmdlet(VerbsCommon.Find, "ActivityStream")]
    [OutputType(typeof(ActivityStream))]
    public class FindActivityStreamCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.OAuth2Application, ResourceType.OAuth2AccessToken, ResourceType.Organization, ResourceType.User,
            ResourceType.Project, ResourceType.Team, ResourceType.Credential, ResourceType.CredentialType,
            ResourceType.Inventory, ResourceType.InventorySource, ResourceType.Group, ResourceType.Host,
            ResourceType.JobTemplate, ResourceType.Job, ResourceType.AdHocCommand, ResourceType.WorkflowJobTemplate,
            ResourceType.WorkflowJob, ResourceType.ExecutionEnvironment
        ])]
        [ResourceCompletions(
            ResourceType.OAuth2Application, ResourceType.OAuth2AccessToken, ResourceType.Organization, ResourceType.User,
            ResourceType.Project, ResourceType.Team, ResourceType.Credential, ResourceType.CredentialType,
            ResourceType.Inventory, ResourceType.InventorySource, ResourceType.Group, ResourceType.Host,
            ResourceType.JobTemplate, ResourceType.Job, ResourceType.AdHocCommand, ResourceType.WorkflowJobTemplate,
            ResourceType.WorkflowJob, ResourceType.ExecutionEnvironment
        )]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "timestamp", "operation", "changes", "object1", "object2", "action_node")]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.OAuth2Application => $"{Application.PATH}{Resource.Id}/activity_stream/",
                ResourceType.OAuth2AccessToken => $"{OAuth2AccessToken.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/activity_stream/",
                ResourceType.User => $"{User.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Credential => $"{Credential.PATH}{Resource.Id}/activity_stream/",
                ResourceType.CredentialType => $"{Resources.CredentialType.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/activity_stream/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Group => $"{Group.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Host => $"{Host.PATH}{Resource.Id}/activity_stream/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/activity_stream/",
                ResourceType.Job => $"{JobTemplateJob.PATH}{Resource.Id}/activity_stream/",
                ResourceType.AdHocCommand => $"{AdHocCommand.PATH}{Resource.Id}/activity_stream/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/activity_stream/",
                ResourceType.WorkflowJob => $"{WorkflowJob.PATH}{Resource.Id}/activity_stream/",
                ResourceType.ExecutionEnvironment => $"{ExecutionEnvironment.PATH}{Resource.Id}/activity_stream/",
                _ => ActivityStream.PATH
            };
            Find<ActivityStream>(path);
        }
    }
}
