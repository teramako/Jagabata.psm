using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "CredentialInputSource")]
    [OutputType(typeof(CredentialInputSource))]
    public class GetCredentialInputSourceCommand : GetCommandBase<CredentialInputSource>
    {
        protected override ResourceType AcceptType => ResourceType.CredentialInputSource;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "CredentialInputSource", DefaultParameterSetName = "All")]
    [OutputType(typeof(CredentialInputSource))]
    public class FindCredentialInputSourceCommand : FindCommandBase
    {
        [Parameter(ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.Credential))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public ulong Id { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Id > 0 ? $"{Credential.PATH}{Id}/input_sources/" : CredentialInputSource.PATH;
            Find<CredentialInputSource>(path);
        }
    }
}

