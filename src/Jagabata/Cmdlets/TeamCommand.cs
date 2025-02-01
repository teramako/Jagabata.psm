using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Team")]
    [OutputType(typeof(Team))]
    public class GetTeamCommand : GetCommandBase<Team>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Team])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Team)]
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

    [Cmdlet(VerbsCommon.Find, "Team")]
    [OutputType(typeof(Team))]
    public class FindTeamCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.Organization, ResourceType.User, ResourceType.Project, ResourceType.Credential,
            ResourceType.Role
        ])]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.User, ResourceType.Project, ResourceType.Credential,
            ResourceType.Role
        )]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "created_by", "modified_by")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/teams/",
                ResourceType.User => $"{User.PATH}{Resource.Id}/teams/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/teams/",
                ResourceType.Credential => $"{Credential.PATH}{Resource.Id}/owner_teams/",
                ResourceType.Role => $"{Role.PATH}{Resource.Id}/teams/",
                _ => Team.PATH
            };
            Find<Team>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Team", SupportsShouldProcess = true)]
    [OutputType(typeof(Team))]
    public class NewTeamCommand : NewCommandBase<Team>
    {
        [Parameter(Mandatory = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong Organization { get; set; }

        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string Description { get; set; } = string.Empty;

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "description", Description },
                { "organization", Organization },
            };
            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Team", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveTeamCommand : RemoveCommandBase<Team>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Team])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsData.Update, "Team", SupportsShouldProcess = true)]
    [OutputType(typeof(Team))]
    public class UpdateTeamCommand : UpdateCommandBase<Team>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Team])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong Organization { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Organization > 0)
                sendData.Add("organization", Organization);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }
}
