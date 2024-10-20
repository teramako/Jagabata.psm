using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;

namespace Jagabata.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "ApiConfig")]
    [OutputType(typeof(ApiConfig))]
    public class GetApiConfigCommand : Cmdlet
    {
        protected override void EndProcessing()
        {
            WriteObject(ApiConfig.Instance);
        }
    }

    [Cmdlet(VerbsCommon.New, "ApiConfig")]
    [OutputType(typeof(ApiConfig))]
    public class NewApiConfigCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public Uri? Uri { get; set; }
        [Parameter()]
        public FileInfo SaveAs { get; set; } = new FileInfo(ApiConfig.DefaultConfigPath);

        const string banner = """
                _        _    ____
               / \      / \  |  _ \       _ __  ___ _ __ ___
              / _ \    / _ \ | |_) |     | '_ \/ __| '_ ` _ \
             / ___ \  / ___ \|  __/   _  | |_) \__ \ | | | | |
            /_/   \_\/_/   \_\_|     (_) | .__/|___/_| |_| |_|
                                         |_|

            """;
        private ApiConfig? config = null;
        private SecureString? GetToken()
        {
            var fd = new FieldDescription("Personal Token");
            fd.SetParameterType(typeof(SecureString));
            var fdc = new Collection<FieldDescription>() { fd };
            Dictionary<string, PSObject?>? result = Host.UI.Prompt(string.Empty,
                                                                   "Please enter the your Personal Access Token(PAT)",
                                                                   fdc);
            if (result is null) return null;
            foreach (PSObject? o in result.Values)
            {
                if (o?.BaseObject is SecureString secureString)
                {
                    return secureString;
                }
            }
            return null;
        }

        protected override void BeginProcessing()
        {
            if (Uri is null)
            {
                throw new ArgumentNullException(nameof(Uri));
            }

            Host.UI.WriteLine(ConsoleColor.Red, Console.BackgroundColor, banner);

            var secureString = GetToken();
            if (secureString is null)
            {
                WriteVerbose("Canceled.");
                return;
            }
            config = new ApiConfig(Uri, secureString);
        }
        protected override void EndProcessing()
        {
            if (config is null) return;
            var currentConfig = ApiConfig.Instance;
            RestAPI.SetClient(ApiConfig.Load(config));
            try
            {
                Host.UI.WriteLine($"Try to retrieve the user information from: {config.Origin}");
                var me = config.LoadUser(save: false);
                Host.UI.WriteLine($"Success: {me.Username}({me.Email})");
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "ApiError", ErrorCategory.ResourceUnavailable, config));
                Host.UI.WriteLine($"Fallback to the before config.");
                RestAPI.SetClient(ApiConfig.Load(currentConfig));
                return;
            }

            config.Save(SaveAs);
            Host.UI.WriteLine($"Save config to: {SaveAs}");
            WriteObject(config);
            Host.UI.WriteLine($"Sccess 🎉");
        }
    }

    [Cmdlet(VerbsCommon.Switch, "ApiConfig")]
    [OutputType(typeof(ApiConfig))]
    public class SwitchApiConfigCommand : PSCmdlet
    {
        [Parameter(Position = 0)]
        public string Path { get; set; } = string.Empty;

        protected override void EndProcessing()
        {
            FileInfo file;
            if (string.IsNullOrEmpty(Path))
            {
                file = new FileInfo(ApiConfig.DefaultConfigPath);
                WriteVerbose($"Switch to default config; {file}");
            }
            else
            {
                var path = SessionState.Path.GetResolvedPSPathFromPSPath(Path).First();
                if (path is null)
                {
                    return;
                }
                file = new FileInfo(path.Path);
            }
            if (!file.Exists)
            {
                WriteError(new ErrorRecord(new FileNotFoundException($"File Not Found: {file}"),
                                           "AnsibleError",
                                           ErrorCategory.InvalidArgument,
                                           file));
                return;
            }
            else
            {
                WriteVerbose($"Switch to: {file}");
            }
            var config = ApiConfig.Load(file);
            RestAPI.SetClient(config);
            WriteObject(config);
        }
    }
}

