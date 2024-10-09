using AWX.Resources;
using System.Collections;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "CredentialType")]
    [OutputType(typeof(CredentialType))]
    public class GetCredentialTypeCommand : GetCommandBase<CredentialType>
    {
        protected override ResourceType AcceptType => ResourceType.CredentialType;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "CredentialType")]
    [OutputType(typeof(CredentialType))]
    public class FindCredentialTypeCommand : FindCommandBase
    {
        public override ResourceType Type { get; set; }
        public override ulong Id { get; set; }

        [Parameter()]
        public CredentialTypeKind[]? Kind { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            if (Kind != null)
            {
                Query.Add("kind__in", string.Join(',', Kind));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            foreach (var resultSet in GetResultSet<CredentialType>(CredentialType.PATH, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "CredentialType", SupportsShouldProcess = true)]
    [OutputType(typeof(CredentialType))]
    public class NewCredentialTypeCommand : NewCommandBase<CredentialType>
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ValidateSet("net", "cloud")]
        public string Kind { get; set; } = string.Empty;

        [Parameter()]
        public IDictionary Inputs { get; set; } = new Hashtable();

        [Parameter()]
        public IDictionary Injectors { get; set; } = new Hashtable();

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "kind", Kind },
                { "inputs", Inputs },
                { "injectors", Injectors }
            };
            if (Description != null)
                sendData.Add("description", Description);

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

    [Cmdlet(VerbsCommon.Remove, "CredentialType", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public class RemoveCredentialTypeCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.CredentialType])]
        public ulong Id { get; set; }

        [Parameter()]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            if (Force || ShouldProcess($"CredentialType [{Id}]", "Delete completely"))
            {
                try
                {
                    var apiResult = DeleteResource($"{CredentialType.PATH}{Id}/");
                    if (apiResult?.IsSuccessStatusCode ?? false)
                    {
                        WriteVerbose($"CredentialType {Id} is removed.");
                    }
                }
                catch (RestAPIException) { }
            }
        }
    }

    [Cmdlet(VerbsData.Update, "CredentialType", SupportsShouldProcess = true)]
    [OutputType(typeof(CredentialType))]
    public class UpdateCredentialTypeCommand : UpdateCommandBase<CredentialType>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.CredentialType])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ValidateSet("net", "cloud")]
        public string? Kind { get; set; }

        [Parameter()]
        public IDictionary? Inputs { get; set; }

        [Parameter()]
        public IDictionary? Injectors { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description != null)
                sendData.Add("description", Description);
            if (Kind != null)
                sendData.Add("kind", Kind);
            if (Inputs != null)
                sendData.Add("inputs", Inputs);
            if (Injectors != null)
                sendData.Add("injectors", Injectors);

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
