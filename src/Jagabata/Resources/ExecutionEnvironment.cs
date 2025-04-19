using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface IExecutionEnvironment
    {
        /// <summary>
        /// Name of this execution environment.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this execution environment.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The organization used to determine access to this execution environment.
        /// </summary>
        ulong? Organization { get; }
        /// <summary>
        /// The full image location, including the container registory, image name, and version tag.
        /// </summary>
        string Image { get; }
        ulong? Credential { get; }
        /// <summary>
        /// Pull image before running?
        /// <list type="bullet">
        /// <item><term><c>""</c></term><description>----- (default)</description></item>
        /// <item><term><c>"always"</c></term><description>Always pull container before running</description></item>
        /// <item><term><c>"missing"</c></term><description>Only pull the image if not present before running</description></item>
        /// <item><term><c>"never"</c></term><description>Never pull container before running</description></item>
        /// </list>
        /// </summary>
        string Pull { get; }
    }

    public class ExecutionEnvironment(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      string name, string description, ulong? organization, string image, bool managed,
                                      ulong? credential, string pull)
        : ResourceBase, IExecutionEnvironment
    {
        public const string PATH = "/api/v2/execution_environments/";

        /// <summary>
        /// Get an Execution Environment
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<ExecutionEnvironment> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<ExecutionEnvironment>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <summary>
        /// Get an Execution Environment
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        public static ExecutionEnvironment Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Execution Environments
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ExecutionEnvironment> FindAsync(HttpQuery? query = null,
                                                                             [EnumeratorCancellation]
                                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ExecutionEnvironment>(PATH, query, ct))
            {
                foreach (var exeEnv in result.Contents.Results)
                {
                    yield return exeEnv;
                }
            }
        }
        /// <summary>
        /// Find Execution Environments for an Organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/<paramref name="organizationId"/>/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ExecutionEnvironment> FindAsync(ulong organizationId,
                                                                             HttpQuery? query = null,
                                                                             [EnumeratorCancellation]
                                                                             CancellationToken ct = default)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/execution_environments/";
            await foreach (var result in RestAPI.GetResultSetAsync<ExecutionEnvironment>(path, query, ct))
            {
                foreach (var exeEnv in result.Contents.Results)
                {
                    yield return exeEnv;
                }
            }
        }

        /// <summary>
        /// Find Execution Environments
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        public static ExecutionEnvironment[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Execution Environments by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static ExecutionEnvironment[] Find(string? searchWords = null,
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

        /// <summary>
        /// Find ExecutionEnvironments for an Organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/<paramref name="organizationId"/>/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static ExecutionEnvironment[] Find(ulong organizationId, HttpQuery? query = null)
        {
            return [.. FindAsync(organizationId, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find ExecutionEnvironments for an Organization by basic parameters
        /// <para>
        /// Implement API: <c>/api/v2/organizations/<paramref name="organizationId"/>/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <returns></returns>
        public static ExecutionEnvironment[] Find(ulong organizationId,
                                                  string? searchWords = null,
                                                  string orderBy = "name",
                                                  ushort pageSize = 20,
                                                  uint startPage = 1)
        {
            return Find(organizationId, new QueryBuilder().SetSearchWords(searchWords)
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
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong? Organization { get; } = organization;
        public string Image { get; } = image;
        public bool Managed { get; } = managed;
        public ulong? Credential { get; } = credential;
        public string Pull { get; } = pull;

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] FindActivityStream(string? searchWords = null,
                                                   string orderBy = "-timestamp",
                                                   ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream",
                                                               searchWords,
                                                               orderBy,
                                                               pageSize)];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find unified job templates related to this execution environment
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/{id}/unified_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public UnifiedJobTemplate[] FindUnifiedJobTemplates(string? searchWords = null,
                                                            string orderBy = "name",
                                                            ushort pageSize = 20)
        {
            return Related.TryGetPath("unified_job_templates", out var path)
                ? [.. RestAPI.GetResultSetAsync(path, new QueryBuilder().SetSearchWords(searchWords)
                                                                        .SetOrderBy(orderBy)
                                                                        .SetPageSize(pageSize)
                                                                        .Build())
                             .ToBlockingEnumerable()
                             .SelectMany(static apiResult => apiResult.Contents.Results)
                             .OfType<UnifiedJobTemplate>()]
                : [];
        }

        /// <summary>
        /// Find unified job templates related to this execution environment
        /// <para>
        /// Implement API: <c>/api/v2/execution_environments/{id}/unified_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public UnifiedJobTemplate[] FindUnifiedJobTemplates(HttpQuery query)
        {
            return Related.TryGetPath("unified_job_templates", out var path)
                ? [.. RestAPI.GetResultSetAsync(path, query)
                             .ToBlockingEnumerable()
                             .SelectMany(static apiResult => apiResult.Contents.Results)
                             .OfType<UnifiedJobTemplate>()]
                : [];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Image"] = Image
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
