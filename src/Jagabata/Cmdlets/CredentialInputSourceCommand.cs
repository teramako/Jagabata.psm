using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
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

    [Cmdlet(VerbsCommon.Find, "CredentialInputSource")]
    [OutputType(typeof(CredentialInputSource))]
    public class FindCredentialInputSourceCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong Credential { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Credential > 0 ? $"{Resources.Credential.PATH}{Credential}/input_sources/" : CredentialInputSource.PATH;
            Find<CredentialInputSource>(path);
        }
    }
}

