using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ApiHelp")]
    [OutputType(typeof(ApiHelp))]
    public class HelpCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ArgumentCompleter(typeof(ApiPathCompleter))]
        public string Path { get; set; } = string.Empty;

        protected override void EndProcessing()
        {
            var help = GetApiHelp(Path);
            WriteObject(help, false);
        }
    }
}
