using System.Text;

namespace Jagabata.Resources
{
    /// <summary>
    /// Dashboard details
    /// </summary>
    public class Dashboard(Dashboard.InventoriesRecord inventories,
                           Dictionary<string, Dashboard.LabeledRecord> inventorySources,
                           Dashboard.GroupsRecord groups,
                           Dashboard.TotalAndFailedRecord hosts,
                           Dashboard.TotalAndFailedRecord projects,
                           Dictionary<string, Dashboard.LabeledRecord> scmTypes,
                           Dashboard.TotalRecord users,
                           Dashboard.TotalRecord organizations,
                           Dashboard.TotalRecord teams,
                           Dashboard.TotalRecord credentials,
                           Dashboard.TotalRecord jobTemplates)
    {
        public const string PATH = "/api/v2/dashboard/";

        public Dashboard.InventoriesRecord Inventories { get; } = inventories;
        public Dictionary<string, Dashboard.LabeledRecord> InventorySources { get; } = inventorySources;
        public Dashboard.GroupsRecord Groups { get; } = groups;
        public Dashboard.TotalAndFailedRecord Hosts { get; } = hosts;
        public Dashboard.TotalAndFailedRecord Projects { get; } = projects;
        public Dictionary<string, Dashboard.LabeledRecord> ScmTypes { get; } = scmTypes;
        public Dashboard.TotalRecord Users { get; } = users;
        public Dashboard.TotalRecord Organizations { get; } = organizations;
        public Dashboard.TotalRecord Teams { get; } = teams;
        public Dashboard.TotalRecord Credentials { get; } = credentials;
        public Dashboard.TotalRecord JobTemplates { get; } = jobTemplates;

        /// <summary>
        /// For Users, Organizations, Teams, Credentials and JobTemplates
        /// </summary>
        public record TotalRecord(string Url, uint Total)
        {
            public sealed override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("{ ");
                if (PrintMembers(sb)) sb.Append(' ');
                sb.Append('}');
                return sb.ToString();
            }
        }

        /// <summary>
        /// For Inventories
        /// </summary>
        public record InventoriesRecord(string Url, uint Total, uint TotalWithInventorySource, uint JobFailed, uint InventoryFailed)
            : TotalRecord(Url, Total);

        /// <summary>
        /// For InventorySources and ScmTypes
        /// </summary>
        public record LabeledRecord(string Url, string FailuresUrl, string Label, uint Total, uint Failed)
            : TotalRecord(Url, Total);

        /// <summary>
        /// For Groups
        /// </summary>
        public record GroupsRecord(string Url, uint Total, uint InventoryFailed)
            : TotalRecord(Url, Total);

        /// <summary>
        /// For Hosts and Projects
        /// </summary>
        public record TotalAndFailedRecord(string Url, string FailuresUrl, uint Total, uint Failed)
            : TotalRecord(Url, Total);
    }
}
