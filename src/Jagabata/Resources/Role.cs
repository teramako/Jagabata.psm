namespace Jagabata.Resources
{
    public class Role(ulong id, ResourceType type, string url, RelatedDictionary related, Role.Summary summaryFields,
                      string name, string description)
        : IResource, ICacheableResource, IHasCacheableItems
    {
        public const string PATH = "/api/v2/roles/";
        /// <summary>
        /// Retrieve a Role.<br/>
        /// API Path: <c>/api/v2/roles/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Role> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Role>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Roles.<br/>
        /// API Path: <c>/api/v2/roles/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Role> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Role>(PATH, query))
            {
                foreach (var role in result.Contents.Results)
                {
                    yield return role;
                }
            }
        }
        public record Summary(string? ResourceName,
                              ResourceType? ResourceType,
                              string? ResourceTypeDisplayName,
                              ulong? ResourceId);


        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;

        public string Name { get; } = name;
        public string Description { get; } = description;

        /// <summary>
        /// Get the users related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] GetUsers(string? searchWords = null, string orderBy = "username", ushort pageSize = 20)
        {
            return GetUsers(new QueryBuilder().SetSearchWords(searchWords)
                                              .SetOrderBy(orderBy)
                                              .SetPageSize(pageSize)
                                              .Build());
        }

        /// <summary>
        /// Get the users related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] GetUsers(HttpQuery query)
        {
            return Related.TryGetPath("users", out var path)
                ? [.. RestAPI.GetResultSet<User>(path, query)
                             .SelectMany(static apiResult => apiResult.Contents.Results)]
                : [];
        }

        /// <summary>
        /// Get the teams related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] GetTeams(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return GetTeams(new QueryBuilder().SetSearchWords(searchWords)
                                              .SetOrderBy(orderBy)
                                              .SetPageSize(pageSize)
                                              .Build());
        }

        /// <summary>
        /// Get the teams related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] GetTeams(HttpQuery query)
        {
            return Related.TryGetPath("teams", out var path)
                ? [.. RestAPI.GetResultSet<Team>(path, query)
                             .SelectMany(static apiResult => apiResult.Contents.Results)]
                : [];
        }

        CacheItem ICacheableResource.GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, Description);
            if (SummaryFields.ResourceId is not null)
            {
                item.Metadata.Add("TargetObject", $"[{SummaryFields.ResourceType}:{SummaryFields.ResourceId}] {SummaryFields.ResourceName}");
            }
            return item;
        }

        IEnumerable<CacheItem> IHasCacheableItems.GetCacheableItems()
        {
            if (SummaryFields.ResourceId is not null
                && SummaryFields.ResourceType is not null
                && SummaryFields.ResourceName is not null)
            {
                yield return new CacheItem((ResourceType)SummaryFields.ResourceType,
                                           (ulong)SummaryFields.ResourceId,
                                           SummaryFields.ResourceName,
                                           string.Empty,
                                           CacheType.Summary);
            }
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
