using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// Job verbosity
    /// <list type="bullet">
    ///     <item><term>0</term><description>Normal (default)</description></item>
    ///     <item><term>1</term><description>Verbose</description></item>
    ///     <item><term>2</term><description>More Verbose</description></item>
    ///     <item><term>3</term><description>Debug</description></item>
    ///     <item><term>4</term><description>Connection Debug</description></item>
    ///     <item><term>5</term><description>WinRM Debug</description></item>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(JsonNumberEnumConverter<JobVerbosity>))]
    public enum JobVerbosity
    {
        Normal = 0,
        Verbose = 1,
        MoreVerbose = 2,
        Debug = 3,
        ConnectionDebug = 4,
        WinRMDebug = 5
    }
}

