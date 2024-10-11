using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Token")]
    [OutputType(typeof(OAuth2AccessToken))]
    public class GetTokenCommand : GetCommandBase<OAuth2AccessToken>
    {
        protected override ResourceType AcceptType => ResourceType.OAuth2AccessToken;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Token", DefaultParameterSetName = "All")]
    [OutputType(typeof(OAuth2AccessToken))]
    public class FindTokenCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.OAuth2Application),
                     nameof(ResourceType.User))]
        public override ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public override ulong Id { get; set; }

        /// <summary>
        /// Filter by Personal Access Token(<c>Personal</c>) or
        /// User Authorized Token(<c>Authorized</c>) or both.<br/>
        /// Ignored when <c>Type</c> parameter is <c>OAuth2Application</c>
        /// </summary>
        [Parameter()]
        public ETokenType TokenType { get; set; } = ETokenType.Both;

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        public enum ETokenType
        {
            Both, Personal, Authorized
        }

        protected override void ProcessRecord()
        {
            Query.Clear();
            if (Type != ResourceType.OAuth2Application)
            {
                switch (TokenType)
                {
                    case ETokenType.Personal:
                        Query.Add("application", "null");
                        break;
                    case ETokenType.Authorized:
                        Query.Add("not__application", "null");
                        break;
                }
            }
            SetupCommonQuery();
            var path = Type switch
            {
                ResourceType.OAuth2Application => $"{Application.PATH}{Id}/tokens/",
                ResourceType.User => $"{User.PATH}{Id}/tokens/",
                _ => OAuth2AccessToken.PATH
            };
            foreach (var resultSet in GetResultSet<OAuth2AccessToken>(path, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "Token", SupportsShouldProcess = true, DefaultParameterSetName = "Application")]
    [OutputType(typeof(OAuth2AccessToken))]
    public class NewTokenCommand : NewCommandBase<OAuth2AccessToken>
    {
        [Parameter(Mandatory = true, ParameterSetName = "User", Position = 0)]
        public SwitchParameter ForMe { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Application", Position = 0)]
        [Parameter(ParameterSetName = "User", Position = 1)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2Application])]
        public ulong? Application { get; set; }

        [Parameter()]
        [ValidateSet("read", "write")]
        public string Scope { get; set; } = "write";

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>();
            if (Description is not null)
                sendData.Add("description", Description);
            sendData.Add("scope", Scope);
            if (ForMe && Application is not null)
                sendData.Add("application", Application);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            ulong? id;
            string path;
            string action;
            if (ForMe)
            {
                id = 0;
                action = "New Personal Access Token";
                path = $"{Resources.User.PATH}{id}/tokens/";
            }
            else if (Application is not null)
            {
                id = Application;
                action = $"New Application Access Token [{id}]";
                path = $"{Resources.Application.PATH}{id}/tokens/";
            }
            else
            {
                throw new ArgumentException("Parameter `ForMe` or `Application` must be supplied.");
            }

            if (TryCreate(path, out var result, action))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Token", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveTokenCommand : RemoveCommandBase<OAuth2AccessToken>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2AccessToken])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsData.Update, "Token", SupportsShouldProcess = true)]
    [OutputType(typeof(OAuth2AccessToken))]
    public class UpdateTokenCommand : UpdateCommandBase<OAuth2AccessToken>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.OAuth2AccessToken])]
        public override ulong Id { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ValidateSet("read", "write")]
        public string? Scope { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (Description is not null)
                sendData.Add("description", Description);
            if (Scope is not null)
                sendData.Add("scope", Scope);

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
