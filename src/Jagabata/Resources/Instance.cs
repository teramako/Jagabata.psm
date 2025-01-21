using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// Role that this node plays in the mesh
    /// </summary>
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<InstanceNodeType>))]
    public enum InstanceNodeType
    {
        /// <summary>
        /// Control plane node
        /// </summary>
        Control,
        /// <summary>
        /// Execution plane node
        /// </summary>
        Excecution,
        /// <summary>
        /// Control and execution
        /// </summary>
        Hybrid,
        /// <summary>
        /// Message passing node, no execution capability
        /// </summary>
        Hop
    }

    public interface IInstance
    {
        string Hostname { get; }
        string CapacityAdjustment { get; }
        bool Enabled { get; }
        bool ManagedByPolicy { get; }
        InstanceNodeType NodeType { get; }
        string NodeState { get; }
        int ListenerPort { get; }
    }


    public class Instance(ulong id,
                          ResourceType type,
                          string url,
                          RelatedDictionary related,
                          SummaryFieldsDictionary summaryFields,
                          string hostname,
                          string uuid,
                          DateTime created,
                          DateTime? modified,
                          DateTime lastSeen,
                          DateTime? healthCheckStarted,
                          bool healthCheckPending,
                          DateTime? lastHealthCheck,
                          string errors,
                          string capacityAdjustment,
                          string version,
                          int capacity,
                          int consumedCapacity,
                          double percentCapacityRemaining,
                          int jobsRunning,
                          int jobsTotal,
                          string cpu,
                          ulong memory,
                          int cpuCapacity,
                          int memCapacity,
                          bool enabled,
                          bool managedByPolicy,
                          InstanceNodeType nodeType,
                          string nodeState,
                          string ipAddress,
                          int listenerPort)
        : IInstance, IResource, ICacheableResource
    {
        public const string PATH = "/api/v2/instances/";
        /// <summary>
        /// Retrieve an Instance.<br/>
        /// API Path: <c>/api/v2/instances/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Instance> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Instance>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Instances.<br/>
        /// API Path: <c>/api/v2/instances/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Instance> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Instance>(PATH, query, getAll))
            {
                foreach (var instance in result.Contents.Results)
                {
                    yield return instance;
                }
            }
        }
        /// <summary>
        /// List instances for an Instance Group<br/>
        /// API Path: <c>/api/v2/instance_groups/<paramref name="instanceGroupId"/>/instances/</c>
        /// </summary>
        /// <param name="instanceGroupId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Instance> FindFromInstanceGroup(ulong instanceGroupId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{InstanceGroup.PATH}{instanceGroupId}/instances/";
            await foreach (var result in RestAPI.GetResultSetAsync<Instance>(path, query, getAll))
            {
                foreach (var instance in result.Contents.Results)
                {
                    yield return instance;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public string Hostname { get; } = hostname;
        public string Uuid { get; } = uuid;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public DateTime LastSeen { get; } = lastSeen;
        public DateTime? HealthCheckStarted { get; } = healthCheckStarted;
        public bool HealthCheckPending { get; } = healthCheckPending;
        public DateTime? LastHealthCheck { get; } = lastHealthCheck;
        public string Errors { get; } = errors;
        public string CapacityAdjustment { get; } = capacityAdjustment;
        public string Version { get; } = version;
        public int Capacity { get; } = capacity;
        public int ConsumedCapacity { get; } = consumedCapacity;
        public double PercentCapacityRemaining { get; } = percentCapacityRemaining;
        public int JobsRunning { get; } = jobsRunning;
        public int JobsTotal { get; } = jobsTotal;
        public string Cpu { get; } = cpu;
        public ulong Memory { get; } = memory;
        public int CpuCapacity { get; } = cpuCapacity;
        public int MemCapacity { get; } = memCapacity;
        public bool Enabled { get; } = enabled;
        public bool ManagedByPolicy { get; } = managedByPolicy;
        public InstanceNodeType NodeType { get; } = nodeType;
        public string NodeState { get; } = nodeState;
        public string IpAddress { get; } = ipAddress;
        public int ListenerPort { get; } = listenerPort;

        public string GetDescription()
        {
            return $"{Hostname} [{NodeType}]";
        }
    }
}
