using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ExecutionEnvironment")]
    [OutputType(typeof(ExecutionEnvironment))]
    public class GetExecutionEnvironmentCommand : GetCommandBase<ExecutionEnvironment>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
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

    [Cmdlet(VerbsCommon.Find, "ExecutionEnvironment")]
    [OutputType(typeof(ExecutionEnvironment))]
    public class FindExecutionEnvironmentCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        public ulong Organization { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "created", "modified", "name", "description", "organization",
                                   "image", "managed", "credential", "pull"])]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Organization > 0 ? $"{Resources.Organization.PATH}{Organization}/execution_environments/" : ExecutionEnvironment.PATH;
            Find<ExecutionEnvironment>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "ExecutionEnvironment", SupportsShouldProcess = true)]
    [OutputType(typeof(ExecutionEnvironment))]
    public class NewExecutionEnvironmentCommand : NewCommandBase<ExecutionEnvironment>
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string Description { get; set; } = string.Empty;

        [Parameter(Mandatory = true)]
        public string Image { get; set; } = string.Empty;

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong? Organization { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? Credential { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ValidateSet("", "always", "missing", "never")]
        public string? Pull { get; set; } = string.Empty;

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "description", Description },
                { "image", Image },
            };
            if (Organization is not null)
                sendData.Add("organization", Organization);
            if (Credential is not null)
                sendData.Add("credential", Credential);
            if (Pull is not null)
                sendData.Add("pull", Pull);

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

    [Cmdlet(VerbsData.Update, "ExecutionEnvironment", SupportsShouldProcess = true)]
    [OutputType(typeof(ExecutionEnvironment))]
    public class UpdateExecutionEnvironmentCommand : UpdateCommandBase<ExecutionEnvironment>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public string? Image { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong? Organization { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? Credential { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ValidateSet("", "always", "missing", "never")]
        public string? Pull { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Image is not null)
                sendData.Add("image", Image);
            if (Organization is not null)
                sendData.Add("organization", Organization == 0 ? null : Organization);
            if (Credential is not null)
                sendData.Add("credential", Credential == 0 ? null : Credential);
            if (Pull is not null)
                sendData.Add("pull", Pull);

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

    [Cmdlet(VerbsCommon.Remove, "ExecutionEnvironment", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveExecutionEnvironmentCommand : RemoveCommandBase<ExecutionEnvironment>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
