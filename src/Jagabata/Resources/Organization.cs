using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IOrganization
    {
        string Name { get; }
        string Description { get; }
        int MaxHosts { get; }
        int? DefaultEnvironment { get; }
    }

    public class Organization(ulong id, ResourceType type, string url, RelatedDictionary related,
                              SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                              string description, int maxHosts, string? customVirtualenv, int? defaultEnvironment)
        : ResourceBase, IOrganization
    {
        public const string PATH = "/api/v2/organizations/";
        /// <summary>
        /// Retrieve an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Organization> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Organization>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Organizations.<br/>
        /// API Path: <c>/api/v2/organizations/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(PATH, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }
        /// <summary>
        /// List Organizations Administered by the User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/admin_of_organizations/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindAdministeredByUser(ulong userId,
                                                                                  HttpQuery? query = null)
        {
            var path = $"{User.PATH}/{userId}/admin_of_organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }
        /// <summary>
        /// List Organizations for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/organizations/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindFromUser(ulong userId,
                                                                        HttpQuery? query = null)
        {
            var path = $"{User.PATH}/{userId}/organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        [JsonConverter(typeof(Json.SummaryFieldsOrganizationConverter))]
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public int MaxHosts { get; } = maxHosts;
        public string? CustomVirtualenv { get; } = customVirtualenv;
        public int? DefaultEnvironment { get; } = defaultEnvironment;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(string? searchWords = null, ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", searchWords, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public ExecutionEnvironment[] GetExecutionEnvironments(string? searchWords = null,
                                                               string orderBy = "name",
                                                               ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ExecutionEnvironment>("execution_environments",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ExecutionEnvironment[] GetExecutionEnvironments(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ExecutionEnvironment>("execution_environments", query)];
        }

        /// <summary>
        /// Get the projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Project[] GetProjects(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Project>("projects", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Project[] GetProjects(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Project>("projects", query)];
        }

        /// <summary>
        /// Get the inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Inventory[] GetInventories(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Inventory>("inventories", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Inventory[] GetInventories(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Inventory>("inventories", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
