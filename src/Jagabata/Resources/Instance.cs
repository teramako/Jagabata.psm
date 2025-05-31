using System.Runtime.CompilerServices;
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


    public class Instance(ulong id, ResourceType type, string url, RelatedDictionary related,
                          SummaryFieldsDictionary summaryFields, string hostname, string uuid, DateTime created,
                          DateTime? modified, DateTime lastSeen, DateTime? healthCheckStarted, bool healthCheckPending,
                          DateTime? lastHealthCheck, string errors, string capacityAdjustment, string version,
                          int capacity, int consumedCapacity, double percentCapacityRemaining, int jobsRunning,
                          int jobsTotal, string cpu, ulong memory, int cpuCapacity, int memCapacity, bool enabled,
                          bool managedByPolicy, InstanceNodeType nodeType, string nodeState, string ipAddress,
                          int listenerPort)
        : ResourceBase, IInstance
    {
        public const string PATH = "/api/v2/instances/";

        /// <summary>
        /// Get an Instance
        /// <para>
        /// Implement API: <c>/api/v2/instances/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Instance ID</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<Instance> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Instance>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Instance Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Instances
        /// <para>
        /// Implement API: <c>/api/v2/instances/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Instance> FindAsync(HttpQuery? query = null,
                                                                 [EnumeratorCancellation]
                                                                 CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Instance>(PATH, query, ct))
            {
                foreach (var instance in result.Contents.Results)
                {
                    yield return instance;
                }
            }
        }

        /// <summary>
        /// Find Instances for an Instance Group
        /// <para>
        /// Implement API: <c>/api/v2/instance_groups/{id}/instances/</c>
        /// </para>
        /// </summary>
        /// <param name="instanceGroupId">Instance Group ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Instance> FindAsync(ulong instanceGroupId,
                                                                 HttpQuery? query = null,
                                                                 [EnumeratorCancellation]
                                                                 CancellationToken ct = default)
        {
            var path = $"{InstanceGroup.PATH}{instanceGroupId}/instances/";
            await foreach (var result in RestAPI.GetResultSetAsync<Instance>(path, query, ct))
            {
                foreach (var instance in result.Contents.Results)
                {
                    yield return instance;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Instance[] Find(HttpQuery? query = null)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Instances by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/instances/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Instance[] Find(string? searchWords = null,
                                      string orderBy = "name",
                                      ushort pageSize = 20,
                                      uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        public static Instance[] Find(ulong instanceGroupId, HttpQuery? query = null)
        {
            return [.. FindAsync(instanceGroupId, query).ToBlockingEnumerable()];
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
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

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Hostname, string.Empty)
            {
                Metadata = {
                    ["NodeType"] = $"{NodeType}"
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Hostname}";
        }
    }
}
