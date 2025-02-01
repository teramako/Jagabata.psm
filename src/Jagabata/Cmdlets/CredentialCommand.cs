using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Credential")]
    [OutputType(typeof(Credential))]
    public class GetCredentialCommand : GetCommandBase<Credential>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Credential)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Credential)]
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

    [Cmdlet(VerbsCommon.Find, "Credential")]
    [OutputType(typeof(Credential))]
    public class FindCredentialCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.Organization, ResourceType.User, ResourceType.Team, ResourceType.CredentialType,
            ResourceType.InventorySource, ResourceType.InventoryUpdate, ResourceType.JobTemplate, ResourceType.Job,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode, ResourceType.WorkflowJobNode
        )]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.User, ResourceType.Team, ResourceType.CredentialType,
            ResourceType.InventorySource, ResourceType.InventoryUpdate, ResourceType.JobTemplate, ResourceType.Job,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode, ResourceType.WorkflowJobNode
        )]
        public IResource? Resource { get; set; }

        [Parameter()]
        [Alias("Kind")]
        public CredentialTypeKind[]? CredentialTypeKind { get; set; }

        [Parameter()]
        [ArgumentCompletions("aim", "aws", "aws_secretsmanager_credential", "azure_kv", "azure_rm", "centrify_vault_kv",
                             "conjur", "controller", "galaxy_api_token", "gce", "github_token", "gitlab_token",
                             "gpg_public_key", "hashivault_kv", "hashivault_ssh", "insights", "kubernetes_bearer_token",
                             "net", "openstack", "registry", "rhv", "satellite6", "scm", "ssh", "thycotic_dsv",
                             "thycotic_tss", "vault", "vmware")]
        [Alias("Namespace")]
        public string[]? CredentialTypeNamespace { get; set; }

        /// <summary>
        /// Only affected for an Organization
        /// </summary>
        [Parameter()]
        public SwitchParameter Galaxy { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "credential_type", "managed", "created_by", "modified_by")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            if (CredentialTypeKind is not null)
            {
                Query.Add("credential_type__kind__in", string.Join(',', CredentialTypeKind));
            }
            if (CredentialTypeNamespace is not null)
            {
                Query.Add("credential_type__namespace__in", string.Join(',', CredentialTypeNamespace));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/" + (Galaxy ? "galaxy_credentials/" : "credentials/"),
                ResourceType.User => $"{User.PATH}{Resource.Id}/credentials/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/credentials/",
                ResourceType.CredentialType => $"{Resources.CredentialType.PATH}{Resource.Id}/credentials/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/credentials/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJob.PATH}{Resource.Id}/credentials/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/credentials/",
                ResourceType.Job => $"{JobTemplateJob.PATH}{Resource.Id}/credentials/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{Resource.Id}/credentials/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{Resource.Id}/credentials/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{Resource.Id}/credentials/",
                _ => Credential.PATH
            };
            Find<Credential>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Credential", SupportsShouldProcess = true)]
    [OutputType(typeof(Credential))]
    public class NewCredentialCommand : NewCommandBase<Credential>
    {
        [Parameter(Mandatory = true)]
        [ResourceIdTransformation(ResourceType.CredentialType)]
        public ulong CredentialType { get; set; }

        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public IDictionary Inputs { get; set; } = new Hashtable();

        [Parameter()]
        [ResourceTransformation(ResourceType.Organization, ResourceType.Team, ResourceType.User)]
        public IResource? Owner { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "credential_type", CredentialType },
                { "inputs", Inputs }
            };
            if (Description is not null)
                sendData.Add("description", Description);
            if (Owner is not null)
            {
                switch (Owner.Type)
                {
                    case ResourceType.Organization:
                        sendData.Add("organization", Owner.Id);
                        break;
                    case ResourceType.Team:
                        sendData.Add("team", Owner.Id);
                        break;
                    case ResourceType.User:
                        sendData.Add("user", Owner.Id);
                        break;
                }
            }
            else
            {
                var userId = ApiConfig.Instance.UserId;
                if (userId is not null)
                    sendData.Add("user", userId);
            }

            // FIXME: Validation of Inputs value from CredentialType data

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

    [Cmdlet(VerbsLifecycle.Register, "Credential", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterCredentialCommand : RegistrationCommandBase<Credential>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(
            ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode
        )]
        public IResource To { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = To.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{To.Id}/galaxy_credentials/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{To.Id}/credentials/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{To.Id}/credentials/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{To.Id}/credentials/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{To.Id}/credentials/",
                _ => throw new ArgumentException($"Invalid resource type: {To.Type}")
            };
            Register(path, Id, To);
        }
    }
    [Cmdlet(VerbsLifecycle.Unregister, "Credential", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class UnregisterCredentialCommand : RegistrationCommandBase<Credential>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(
            ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode
        )]
        public IResource From { get; set; } = new Resource(0 ,0);

        protected override void ProcessRecord()
        {
            var path = From.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{From.Id}/galaxy_credentials/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{From.Id}/credentials/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{From.Id}/credentials/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{From.Id}/credentials/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{From.Id}/credentials/",
                _ => throw new ArgumentException($"Invalid resource type: {From.Type}")
            };
            Unregister(path, Id, From);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Credential", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveCredentialCommand : RemoveCommandBase<Credential>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsData.Update, "Credential", SupportsShouldProcess = true)]
    [OutputType(typeof(Credential))]
    public class UpdateCredentialCommand : UpdateCommandBase<Credential>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Credential)]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.CredentialType)]
        public ulong? CredentialType { get; set; }

        [Parameter()]
        public IDictionary? Inputs { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.Organization)]
        public ulong? Organization { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (CredentialType is not null)
                sendData.Add("credential_type", CredentialType);
            if (Inputs is not null)
                sendData.Add("inputs", Inputs);
            if (Organization is not null)
                sendData.Add("organization", Organization);

            return sendData;
        }
        protected override void ProcessRecord()
        {
            // FIXME: Validation of Inputs value from CredentialType data

            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }
}
