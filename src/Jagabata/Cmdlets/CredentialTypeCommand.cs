using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.CredentialType;
using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "CredentialType")]
    [OutputType(typeof(Resources.CredentialType))]
    public class GetCredentialTypeCommand : GetCommandBase<Resources.CredentialType>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.CredentialType])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.CredentialType)]
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

    [Cmdlet(VerbsCommon.Find, "CredentialType")]
    [OutputType(typeof(Resources.CredentialType))]
    public class FindCredentialTypeCommand : FindCommandBase
    {
        [Parameter()]
        public CredentialTypeKind[]? Kind { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "created", "modified", "name", "description", "kind", "namespace",
                                   "managed", "inputs", "injectors", "created_by", "modified_by"])]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            if (Kind is not null)
            {
                Query.Add("kind__in", string.Join(',', Kind));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            Find<Resources.CredentialType>(Resources.CredentialType.PATH);
        }
    }

    [Cmdlet(VerbsCommon.New, "CredentialType", SupportsShouldProcess = true)]
    [OutputType(typeof(Resources.CredentialType))]
    public class NewCredentialTypeCommand : NewCommandBase<Resources.CredentialType>
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
        [DictionaryTransformation(typeof(FieldList))]
        public IDictionary Inputs { get; set; } = new Hashtable();

        [Parameter()]
        [DictionaryTransformation(typeof(Injectors))]
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

            if (Description is not null)
            {
                sendData.Add("description", Description);
            }

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
    [OutputType(typeof(void))]
    public class RemoveCredentialTypeCommand : RemoveCommandBase<Resources.CredentialType>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.CredentialType])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsData.Update, "CredentialType", SupportsShouldProcess = true)]
    [OutputType(typeof(Resources.CredentialType))]
    public class UpdateCredentialTypeCommand : UpdateCommandBase<Resources.CredentialType>
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
        [DictionaryTransformation(typeof(FieldList))]
        public IDictionary? Inputs { get; set; }

        [Parameter()]
        [DictionaryTransformation(typeof(Injectors))]
        public IDictionary? Injectors { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Kind is not null)
                sendData.Add("kind", Kind);
            if (Inputs is not null)
                sendData.Add("inputs", Inputs);
            if (Injectors is not null)
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
