using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ActivityStream")]
    [OutputType(typeof(ActivityStream))]
    public class GetActivityStreamCommand : GetCommandBase<ActivityStream>
    {
        protected override ResourceType AcceptType => ResourceType.ActivityStream;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "ActivityStream", DefaultParameterSetName = "All")]
    [OutputType(typeof(ActivityStream))]
    public class FindActivityStreamCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.OAuth2Application),
                     nameof(ResourceType.OAuth2AccessToken),
                     nameof(ResourceType.Organization),
                     nameof(ResourceType.User),
                     nameof(ResourceType.Project),
                     nameof(ResourceType.Team),
                     nameof(ResourceType.Credential),
                     nameof(ResourceType.CredentialType),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.InventorySource),
                     nameof(ResourceType.Group),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.Job),
                     nameof(ResourceType.AdHocCommand),
                     nameof(ResourceType.WorkflowJobTemplate),
                     nameof(ResourceType.WorkflowJob),
                     nameof(ResourceType.ExecutionEnvironment))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.OAuth2Application,
                ResourceType.OAuth2AccessToken,
                ResourceType.Organization,
                ResourceType.User,
                ResourceType.Project,
                ResourceType.Team,
                ResourceType.Credential,
                ResourceType.CredentialType,
                ResourceType.Inventory,
                ResourceType.InventorySource,
                ResourceType.Group,
                ResourceType.Host,
                ResourceType.JobTemplate,
                ResourceType.Job,
                ResourceType.AdHocCommand,
                ResourceType.WorkflowJobTemplate,
                ResourceType.WorkflowJob,
                ResourceType.ExecutionEnvironment
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
                ResourceType.OAuth2Application => $"{Application.PATH}{Id}/activity_stream/",
                ResourceType.OAuth2AccessToken => $"{OAuth2AccessToken.PATH}{Id}/activity_stream/",
                ResourceType.Organization => $"{Organization.PATH}{Id}/activity_stream/",
                ResourceType.User => $"{User.PATH}{Id}/activity_stream/",
                ResourceType.Project => $"{Project.PATH}{Id}/activity_stream/",
                ResourceType.Team => $"{Team.PATH}{Id}/activity_stream/",
                ResourceType.Credential => $"{Credential.PATH}{Id}/activity_stream/",
                ResourceType.CredentialType => $"{CredentialType.PATH}{Id}/activity_stream/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/activity_stream/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Id}/activity_stream/",
                ResourceType.Group => $"{Group.PATH}{Id}/activity_stream/",
                ResourceType.Host => $"{Host.PATH}{Id}/activity_stream/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/activity_stream/",
                ResourceType.Job => $"{JobTemplateJob.PATH}{Id}/activity_stream/",
                ResourceType.AdHocCommand => $"{AdHocCommand.PATH}{Id}/activity_stream/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Id}/activity_stream/",
                ResourceType.WorkflowJob => $"{WorkflowJob.PATH}{Id}/activity_stream/",
                ResourceType.ExecutionEnvironment => $"{ExecutionEnvironment.PATH}{Id}/activity_stream/",
                _ => ActivityStream.PATH
            };
            Find<ActivityStream>(path);
        }
    }
}
