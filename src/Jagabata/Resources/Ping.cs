using System.Text;

namespace Jagabata.Resources
{
    public class Ping(bool ha,
                      string version,
                      string activeNode,
                      string installUuid,
                      Ping.Instance[] instances,
                      Ping.Group[] instanceGroups)
    {
        public bool HA { get; } = ha;
        public string Version { get; } = version;
        public string ActiveNode { get; } = activeNode;
        public string InstallUuid { get; } = installUuid;
        public Instance[] Instances { get; } = instances;
        public Group[] InstanceGroups { get; } = instanceGroups;

        public record Instance(string Node, string NodeType, string Uuid,
                               string Heartbeat, uint Capacity, string Version)
        {
            public override string ToString()
            {
                StringBuilder sb = new();
                sb.Append("{ ");
                if (PrintMembers(sb))
                {
                    sb.Append(' ');
                }
                sb.Append('}');
                return sb.ToString();
            }
        };
        public record Group(string Name, uint Capacity, string[] Instances)
        {
            public override string ToString()
            {
                return $"{{ Name = {Name}, Capacity = {Capacity}, Instances: [{string.Join(',', Instances)}] }}";
            }
        };

        public const string PATH = "/api/v2/ping/";

        /// <summary>
        /// Get a some basic information about the AWX/AnsibleTower
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public static async Task<Ping> GetAsync(CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Ping>(PATH, cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(CancellationToken)"/>
        public static Ping Get()
        {
            return GetAsync().GetAwaiter().GetResult();
        }
    }
}
