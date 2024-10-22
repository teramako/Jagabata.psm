using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Setting")]
    [OutputType(typeof(Setting))]
    public class GetSettingCommand : APICmdletBase
    {
        [Parameter(Position = 0)]
        public string Name { get; set; } = string.Empty;

        private const string BasePath = "/api/v2/settings/";
        protected override void EndProcessing()
        {
            if (string.IsNullOrEmpty(Name))
            {
                WriteObject(GetSettingList(), true);
            }
            else
            {
                WriteObject(GetSetting(Name), false);
            }
        }
        private Dictionary<string, object?>? GetSetting(string name)
        {
            return GetResource<Dictionary<string, object?>>($"{BasePath}{name}/");
        }
        private Setting[]? GetSettingList()
        {
            var resultSet = GetResultSet<Setting>(BasePath, false).First();
            return resultSet.Results;
        }
    }
}
