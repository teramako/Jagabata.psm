using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface IInstanceGroup
    {
        string Name { get; }
        int MaxConcurrentJobs { get; }
        int MaxForks { get; }
        bool IsContainerGroup { get; }
        ulong? Credential { get; }
        double PolicyInstancePercentage { get; }
        int PolicyInstanceMinimum { get; }
        string[] PolicyInstanceList { get; }
        string PodSpecOverride { get; }
    }

    public class InstanceGroup(ulong id, ResourceType type, string url, RelatedDictionary related,
                               SummaryFieldsDictionary summaryFields, string name, DateTime created, DateTime? modified,
                               int capacity, int consumedCapacity, double percentCapacityRemaining, int jobsRunning,
                               int maxConcurrentJobs, int maxForks, int jobsTotal, int instances, bool isContainerGroup,
                               ulong? credential, double policyInstancePercentage, int policyInstanceMinimum,
                               string[] policyInstanceList, string podSpecOverride)
        : ResourceBase, IInstanceGroup
    {
        public const string PATH = "/api/v2/instance_groups/";

        /// <summary>
        /// Get an Instance Group.<br/>
        /// <para>
        /// Impelement API: <c>api/v2/instance_groups/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">InstanceGroup ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<InstanceGroup> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<InstanceGroup>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static InstanceGroup Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Instance Groups.<br/>
        /// <para>
        /// Implement API: <c>/api/v2/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InstanceGroup> FindAsync(HttpQuery? query = null,
                                                                      [EnumeratorCancellation]
                                                                      CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<InstanceGroup>(PATH, query, ct))
            {
                foreach (var instanceGroup in result.Contents.Results)
                {
                    yield return instanceGroup;
                }
            }
        }

        /// <summary>
        /// Find InstanceGroup associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Instance</item>
        ///     <item>Organization</item>
        ///     <item>Inventory</item>
        ///     <item>JobTemplate</item>
        ///     <item>Schedule</item>
        ///     <item>WorkflowJobTemplateNode</item>
        ///     <item>WorkflowJobNode</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with this group</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InstanceGroup> FindAsync(IResource resource,
                                                                      HttpQuery? query = null,
                                                                      [EnumeratorCancellation]
                                                                      CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Instance => $"{Instance.PATH}{resource.Id}/instance_groups/",
                ResourceType.Organization => $"{Organization.PATH}{resource.Id}/instance_groups/",
                ResourceType.Inventory => $"{Inventory.PATH}{resource.Id}/instance_groups/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/instance_groups/",
                ResourceType.Schedule => $"{Schedule.PATH}{resource.Id}/instance_groups/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{resource.Id}/instance_groups/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{resource.Id}/instance_groups/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<InstanceGroup>(path, query, ct))
            {
                foreach (var instanceGroup in result.Contents.Results)
                {
                    yield return instanceGroup;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static InstanceGroup[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Insntance Groups by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static InstanceGroup[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static InstanceGroup[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find InstanceGroup associated with <paramref name="resource"/> by basic parameters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static InstanceGroup[] Find(IResource resource,
                                           string? searchWords = null,
                                           string orderBy = "name",
                                           ushort pageSize = 20,
                                           uint startPage = 1)
        {
            return Find(resource, new QueryBuilder().SetSearchWords(searchWords)
                                                    .SetOrderBy(orderBy)
                                                    .SetPageSize(pageSize)
                                                    .SetStartPage(startPage)
                                                    .Build());
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public string Name { get; } = name;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public int Capacity { get; } = capacity;
        public int ConsumedCapacity { get; } = consumedCapacity;
        public double PercentCapacityRemaining { get; } = percentCapacityRemaining;
        public int JobsRunning { get; } = jobsRunning;
        public int MaxConcurrentJobs { get; } = maxConcurrentJobs;
        public int MaxForks { get; } = maxForks;
        public int JobsTotal { get; } = jobsTotal;
        public int Instances { get; } = instances;
        public bool IsContainerGroup { get; } = isContainerGroup;
        public ulong? Credential { get; } = credential;
        public double PolicyInstancePercentage { get; } = policyInstancePercentage;
        public int PolicyInstanceMinimum { get; } = policyInstanceMinimum;
        public string[] PolicyInstanceList { get; } = policyInstanceList;
        public string PodSpecOverride { get; } = podSpecOverride;

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, string.Empty)
            {
                Metadata = {
                    ["IsContainerGroup"] = $"{IsContainerGroup}",
                    ["Instances"] = $"{Instances}"
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
