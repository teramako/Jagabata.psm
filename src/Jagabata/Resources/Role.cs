using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public class Role(ulong id, ResourceType type, string url, RelatedDictionary related, Role.Summary summaryFields,
                      string name, string description)
        : IResource, ICacheableResource, IHasCacheableItems
    {
        public const string PATH = "/api/v2/roles/";

        /// <summary>
        /// Get a Role
        /// <para>
        /// Implement API: <c>/api/v2/roles/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Role> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Role>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Role Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Roles
        /// <para>
        /// Implement API: <c>/api/v2/roles/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Role> FindAsync(HttpQuery? query = null,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Role>(PATH, query, ct))
            {
                foreach (var role in result.Contents.Results)
                {
                    yield return role;
                }
            }
        }

        /// <summary>
        /// Find Roles associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/roles/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>User</item>
        ///     <item>Team</item>
        /// </list>
        /// </summary>
        /// <param name="resource">Resource object associated with the User or Team</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Role> FindAsync(IResource resource,
                                                             HttpQuery? query = null,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.User => $"{User.PATH}{resource.Id}/roles/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/roles/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Role>(path, query, ct))
            {
                foreach (var role in result.Contents.Results)
                {
                    yield return role;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Role[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Role[] Find(IResource resource,
                                  string? searchWords = null,
                                  string orderBy = "id",
                                  ushort pageSize = 20,
                                  uint startPage = 1)
        {
            return Find(resource, new QueryBuilder().SetSearchWords(searchWords)
                                                    .SetOrderBy(orderBy)
                                                    .SetPageSize(pageSize)
                                                    .SetStartPage(startPage)
                                                    .Build());
        }

        /// <summary>
        /// Find Object Roles associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/object_roles/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>InstanceGroup</item>
        ///     <item>Organization</item>
        ///     <item>Project</item>
        ///     <item>Team</item>
        ///     <item>Credential</item>
        ///     <item>Inventory</item>
        ///     <item>JobTemplate</item>
        ///     <item>WorkflowJobTemplate</item>
        /// </list>
        /// </summary>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Role> FindObjectRolesAsync(IResource resource,
                                                                        HttpQuery? query = null,
                                                                        [EnumeratorCancellation]
                                                                        CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.InstanceGroup => $"{InstanceGroup.PATH}{resource.Id}/object_roles/",
                ResourceType.Organization => $"{Organization.PATH}{resource.Id}/object_roles/",
                ResourceType.Project => $"{Project.PATH}{resource.Id}/object_roles/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/object_roles/",
                ResourceType.Credential => $"{Credential.PATH}{resource.Id}/object_roles/",
                ResourceType.Inventory => $"{Inventory.PATH}{resource.Id}/object_roles/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/object_roles/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{resource.Id}/object_roles/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Role>(path, query, ct))
            {
                foreach (var role in result.Contents.Results)
                {
                    yield return role;
                }
            }
        }

        /// <inheritdoc cref="FindObjectRolesAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Role[] FindObjectRoles(IResource resource, HttpQuery query)
        {
            return [.. FindObjectRolesAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <inheritdoc cref="FindObjectRolesAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Role[] FindObjectRoles(IResource resource,
                                             string? searchWords = null,
                                             string orderBy = "id",
                                             ushort pageSize = 20,
                                             uint startPage = 1)
        {
            return FindObjectRoles(resource, new QueryBuilder().SetSearchWords(searchWords)
                                                               .SetOrderBy(orderBy)
                                                               .SetPageSize(pageSize)
                                                               .SetStartPage(startPage)
                                                               .Build());
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
        /// Find users related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindUsers(string? searchWords = null,
                                string orderBy = "username",
                                ushort pageSize = 20)
        {
            return FindUsers(new QueryBuilder().SetSearchWords(searchWords)
                                               .SetOrderBy(orderBy)
                                               .SetPageSize(pageSize)
                                               .Build());
        }

        /// <summary>
        /// Find users related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindUsers(HttpQuery query)
        {
            return Related.TryGetPath("users", out var path)
                ? [.. RestAPI.GetResultSet<User>(path, query)
                             .SelectMany(static apiResult => apiResult.Contents.Results)]
                : [];
        }

        /// <summary>
        /// Find teams related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] FindTeams(string? searchWords = null,
                                string orderBy = "name",
                                ushort pageSize = 20)
        {
            return FindTeams(new QueryBuilder().SetSearchWords(searchWords)
                                               .SetOrderBy(orderBy)
                                               .SetPageSize(pageSize)
                                               .Build());
        }

        /// <summary>
        /// Find teams related to this role
        /// <para>
        /// Implement API: <c>/api/v2/roles/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] FindTeams(HttpQuery query)
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
