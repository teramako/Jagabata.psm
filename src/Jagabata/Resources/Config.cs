using System.Text;

namespace Jagabata.Resources
{
    /// <summary>
    /// Resource for <c>/api/v2/config/</c> (GET)
    /// </summary>
    public class Config(string timeZone,
                        Config.ConfigLicenseInfo licenseInfo,
                        string version,
                        string eula,
                        string configAnalyticsStatus,
                        Dictionary<string, Config.ConfigValue> analyticsCollectors,
                        string[][] becomeMethods,
                        bool uiNext,
                        string projectBaseDir,
                        string[] projectLocalPaths,
                        string[] customVirtualenvs)
    {
        /// <summary>
        /// The configured time zone for the server.
        /// </summary>
        public string TimeZone { get; } = timeZone;
        /// <summary>
        /// Information about the current license.
        /// </summary>
        public ConfigLicenseInfo LicenseInfo { get; } = licenseInfo;
        /// <summary>
        /// Version of Ansible Tower package installed.
        /// </summary>
        public string Version { get; } = version;
        /// <summary>
        /// The current End-User License Agreement
        /// </summary>
        public string Eula { get; } = eula;
        public string ConfigAnalyticsStatus { get; } = configAnalyticsStatus;
        public Dictionary<string, ConfigValue> AnalyticsCollectors { get; } = analyticsCollectors;
        public string[][] BecomeMethods { get; } = becomeMethods;
        public bool UiNext { get; } = uiNext;
        /// <summary>
        /// Path on the server where projects and playbooks are stored.
        /// </summary>
        public string ProjectBaseDir { get; } = projectBaseDir;
        /// <summary>
        /// List of directories beneath <c>project_base_dir</c> to use when creating/editing a manual project.
        /// </summary>
        public string[] ProjectLocalPaths { get; } = projectLocalPaths;
        /// <summary>
        /// Deprecated venv locations from before migration to execution environments. Export tooling is in <c>awx-manage</c> commands.
        /// </summary>
        public string[] CustomVirtualenvs { get; } = customVirtualenvs;

        /// <summary>
        /// Information about the current license.
        /// </summary>
        public record ConfigLicenseInfo(string LicenseType, bool ValidKey, string SubscriptionName, string ProductName)
        {
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("{ ");
                if (PrintMembers(sb)) sb.Append(' ');
                sb.Append('}');
                return sb.ToString();
            }
        }

        /// <summary>
        /// Value of <see cref="Config.AnalyticsCollectors"/>
        /// </summary>
        public record ConfigValue(string Name, string Version, string Description)
        {
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("{ ");
                if (PrintMembers(sb)) sb.Append(' ');
                sb.Append('}');
                return sb.ToString();
            }
        }
    }
}

