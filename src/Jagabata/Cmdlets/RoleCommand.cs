using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Role")]
    [OutputType(typeof(Role))]
    public class GetRoleCommand : GetCommandBase<Role>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Role)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Role)]
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

    [Cmdlet(VerbsCommon.Find, "Role")]
    [OutputType(typeof(Role))]
    public class FindRoleCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.User, ResourceType.Team)]
        [ResourceCompletions(ResourceType.User, ResourceType.Team)]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "name", "description", "parents", "parents", "content_type",
                           "ancestors", "descendents", "children")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.User => $"{User.PATH}{Resource.Id}/roles/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/roles/",
                _ => Role.PATH
            };
            Find<Role>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "ObjectRole")]
    [OutputType(typeof(Role))]
    public class FindObjectRoleCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.InstanceGroup, ResourceType.Organization, ResourceType.Project, ResourceType.Team,
            ResourceType.Credential, ResourceType.Inventory, ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate
        )]
        [ResourceCompletions(
            ResourceType.InstanceGroup, ResourceType.Organization, ResourceType.Project, ResourceType.Team,
            ResourceType.Credential, ResourceType.Inventory, ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate
        )]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "name", "description", "parents", "parents", "content_type",
                           "ancestors", "descendents", "children")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.InstanceGroup => $"{InstanceGroup.PATH}{Resource.Id}/object_roles/",
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/object_roles/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/object_roles/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/object_roles/",
                ResourceType.Credential => $"{Credential.PATH}{Resource.Id}/object_roles/",
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/object_roles/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/object_roles/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/object_roles/",
                _ => throw new ArgumentException()
            };
            Find<Role>(path);
        }
    }

    [Cmdlet(VerbsSecurity.Grant, "Role", SupportsShouldProcess = true)]
    public class GrantRoleCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ResourceTransformation(ResourceType.Role)]
        public IResource[] Roles { get; set; } = [];

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(ResourceType.User, ResourceType.Team)]
        public IResource To { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = To.Type switch
            {
                ResourceType.User => $"{User.PATH}{To.Id}/roles/",
                ResourceType.Team => $"{Team.PATH}{To.Id}/roles/",
                _ => throw new ArgumentException($"Invalid Resource Type: {To.Type}")
            };

            if (Roles.Length == 0)
                return;

            foreach (var role in Roles)
            {
                if (ShouldProcess($"{To.Type} [{To.Id}]", $"Grant role [{role.Id}]"))
                {
                    var sendData = new Dictionary<string, object>()
                    {
                        { "id", role.Id }
                    };
                    var apiResult = CreateResource<string>(path, sendData);
                    if (apiResult.Response.IsSuccessStatusCode)
                    {
                        WriteVerbose("Success");
                    }
                }
            }
        }
    }

    [Cmdlet(VerbsSecurity.Revoke, "Role", SupportsShouldProcess = true)]
    public class RevokeRoleCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ResourceTransformation(ResourceType.Role)]
        public IResource[] Roles { get; set; } = [];

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(ResourceType.User, ResourceType.Team)]
        public IResource From { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = From.Type switch
            {
                ResourceType.User => $"{User.PATH}{From.Id}/roles/",
                ResourceType.Team => $"{Team.PATH}{From.Id}/roles/",
                _ => throw new ArgumentException($"Invalid Resource Type: {From.Type}")
            };

            if (Roles.Length == 0)
                return;

            foreach (var role in Roles)
            {
                if (ShouldProcess($"{From.Type} [{From.Id}]", $"Revoke role [{role.Id}]"))
                {
                    var sendData = new Dictionary<string, object>()
                    {
                        { "id", role.Id },
                        { "disassociate", true }
                    };
                    var apiResult = CreateResource<string>(path, sendData);
                    if (apiResult.Response.IsSuccessStatusCode)
                    {
                        WriteVerbose("Success");
                    }
                }
            }
        }
    }
}
