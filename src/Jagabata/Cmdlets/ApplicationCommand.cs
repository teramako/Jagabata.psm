using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Application")]
    [OutputType(typeof(Application))]
    public class GetApplicationCommand : GetCommandBase<Application>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2Application])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.OAuth2Application)]
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
    [Cmdlet(VerbsCommon.Find, "Application")]
    [OutputType(typeof(Application))]
    public class FindApplicationCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.Organization, ResourceType.User])]
        [ResourceCompletions(ResourceType.Organization, ResourceType.User)]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "name", "description", "client_type", "redirect_uris",
                           "authorization_grant_type", "skip_authorization", "organization", "user")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/applications/",
                ResourceType.User => $"{User.PATH}{Resource.Id}/applications/",
                _ => Application.PATH
            };
            Find<Application>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Application", SupportsShouldProcess = true)]
    [OutputType(typeof(Application))]
    public class NewApplicationCommand : NewCommandBase<Application>
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true)]
        public ulong Organization { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateSet("password", "authorization-code")]
        public string AuthorizationGrantType { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? RedirectUris { get; set; }

        [Parameter(Mandatory = true)]
        public ApplicationClientType ClientType { get; set; }

        [Parameter()]
        public SwitchParameter SkipAuthorization { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "organization", Organization },
                { "authorization_grant_type", AuthorizationGrantType },
                { "client_type", $"{ClientType}".ToLowerInvariant() }
            };
            if (Description is not null)
                sendData.Add("description", Description);
            if (RedirectUris is not null)
                sendData.Add("redirect_uris", RedirectUris);
            if (SkipAuthorization)
                sendData.Add("skip_authorization", true);

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

    [Cmdlet(VerbsData.Update, "Application", SupportsShouldProcess = true)]
    [OutputType(typeof(Application))]
    public class UpdateApplicationCommand : UpdateCommandBase<Application>
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2Application])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public ulong? Organization { get; set; }

        [Parameter()]
        public string? RedirectUris { get; set; }

        [Parameter()]
        public ApplicationClientType? ClientType { get; set; }

        [Parameter()]
        public SwitchParameter SkipAuthorization { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Organization is not null)
                sendData.Add("organization", Organization);
            if (RedirectUris is not null)
                sendData.Add("redirect_uris", RedirectUris);
            if (ClientType is not null)
                sendData.Add("client_type", $"{ClientType}".ToLowerInvariant());

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

    [Cmdlet(VerbsCommon.Remove, "Application", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveApplicationCommand : RemoveCommandBase<Application>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2Application])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
