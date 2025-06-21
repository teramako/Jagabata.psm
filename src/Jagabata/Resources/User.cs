using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface IUser
    {
        string Username { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        bool IsSuperuser { get; }
        bool IsSystemAuditor { get; }
        string Password { get; }
    }

    public class User(ulong id, ResourceType type, string url, RelatedDictionary related,
                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string username,
                      string firstName, string lastName, string email, bool isSuperuser, bool isSystemAuditor,
                      string password, string ldapDn, DateTime? lastLogin, string externalAccount, string[] auth)
        : ResourceBase, IUser
    {
        public const string PATH = "/api/v2/users/";

        /// <summary>
        /// Get a User
        /// <para>
        /// Implement API: <c>/api/v2/users/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<User> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<User>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static User Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get information about the current User
        /// <para>
        /// Implement API: <c>/api/v2/me/</c>
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<User> GetMeAsync(CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<ResultSet<User>>("/api/v2/me/", cancellationToken: ct);
            return apiResult.Contents.Results.Single();
        }

        /// <inheritdoc cref="GetMeAsync(CancellationToken)"/>
        public static User GetMe()
        {
            return GetMeAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Users
        /// <para>
        /// Implement API: <c>/api/v2/users/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindAsync(HttpQuery? query = null,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<User>(PATH, query, ct))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }

        /// <summary>
        /// Find Users associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/{Type}/{Id}/users/</c>
        /// <para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Inventory</item>
        ///     <item>JobTemplate</item>
        ///     <item>Job</item>
        ///     <item>Schedule</item>
        ///     <item>WorkflowJobTemplate</item>
        ///     <item>WorkflowJob</item>
        ///     <item>WorkflowJobTemplateNode</item>
        ///     <item>WorkflowJobNode</item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindAsync(IResource resource,
                                                             HttpQuery? query = null,
                                                             [EnumeratorCancellation]
                                                             CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{resource.Id}/users/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/users/",
                ResourceType.Credential => $"{Credential.PATH}{resource.Id}/owner_users/",
                ResourceType.Role => $"{Role.PATH}{resource.Id}/users/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query, ct))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static User[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Users by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static User[] Find(string? searchWords = null,
                                  string orderBy = "username",
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
        public static User[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Users associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static User[] Find(IResource resource,
                                  string? searchWords = null,
                                  string orderBy = "username",
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

        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Username { get; } = username;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public string Email { get; } = email;
        public bool IsSuperuser { get; } = isSuperuser;
        public bool IsSystemAuditor { get; } = isSystemAuditor;
        public string Password { get; } = password;
        public string LdapDn { get; } = ldapDn;
        public DateTime? LastLogin { get; } = lastLogin;
        public string? ExternalAccount { get; } = externalAccount;
        public string[] Auth { get; } = auth;

        public UserData ToData()
        {
            return new UserData()
            {
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                IsSuperuser = IsSuperuser,
                IsSystemAuditor = IsSystemAuditor,
                Password = Password,
            };
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/users/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find teams related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] FindTeams(string? searchWords = null,
                                string orderBy = "name",
                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Team>("teams",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find teams related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] FindTeams(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Team>("teams", query)];
        }

        /// <summary>
        /// Find organizations related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Organization[] FindOrganizations(string? searchWords = null,
                                                string orderBy = "name",
                                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Organization>("organizations",
                                                             searchWords,
                                                             orderBy,
                                                             pageSize)];
        }

        /// <summary>
        /// Find organizations related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Organization[] FindOrganizations(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Organization>("organizations", query)];
        }

        /// <summary>
        /// Find organizations for which this user is an administrator.
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/admin_of_organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Organization[] FindAdminOfOrganizations(string? searchWords = null,
                                                       string orderBy = "name",
                                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Organization>("admin_of_organizations",
                                                             searchWords,
                                                             orderBy,
                                                             pageSize)];
        }

        /// <summary>
        /// Find organizations for which this user is an administrator.
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/admin_of_organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Organization[] FindAdminOfOrganizations(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Organization>("admin_of_organizations", query)];
        }

        /// <summary>
        /// Find projects related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Project[] FindProjects(string? searchWords = null,
                                      string orderBy = "name",
                                      ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Project>("projects",
                                                        searchWords,
                                                        orderBy,
                                                        pageSize)];
        }

        /// <summary>
        /// Find projects related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Project[] FindProjects(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Project>("projects", query)];
        }

        /// <summary>
        /// Find credentials related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] FindCredentials(string? searchWords = null,
                                            string orderBy = "name",
                                            ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials",
                                                           searchWords,
                                                           orderBy,
                                                           pageSize)];
        }

        /// <summary>
        /// Find credentials related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Find roles related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/roles/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Role[] FindRoles(string? searchWords = null,
                                string orderBy = "id",
                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Role>("roles",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find roles related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/roles/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Role[] FindRoles(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Role>("roles", query)];
        }

        /// <summary>
        /// Find the access list related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindAccessList(string? searchWords = null,
                                     string orderBy = "username",
                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<User>("access_list",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find the access list related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAccessList(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Find tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public OAuth2AccessToken[] FindTokens(string? searchWords = null,
                                              string orderBy = "id",
                                              ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("tokens",
                                                                  searchWords,
                                                                  orderBy,
                                                                  pageSize)];
        }

        /// <summary>
        /// Find tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public OAuth2AccessToken[] FindTokens(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("tokens", query)];
        }

        /// <summary>
        /// Find authorized tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/authorized_tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public OAuth2AccessToken[] FindAuthorizedTokens(string? searchWords = null,
                                                        string orderBy = "id",
                                                        ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("authorized_tokens",
                                                                  searchWords,
                                                                  orderBy,
                                                                  pageSize)];
        }

        /// <summary>
        /// Find authorized tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/authorized_tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public OAuth2AccessToken[] FindAuthorizedTokens(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("authorized_tokens", query)];
        }

        /// <summary>
        /// Find personal tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/personal_tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public OAuth2AccessToken[] FindPersonalTokens(string? searchWords = null,
                                                      string orderBy = "id",
                                                      ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("personal_tokens",
                                                                  searchWords,
                                                                  orderBy,
                                                                  pageSize)];
        }

        /// <summary>
        /// Find personal tokens related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/personal_tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public OAuth2AccessToken[] FindPersonalTokens(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("personal_tokens", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Username, string.Empty);
            if (!string.IsNullOrEmpty(Email))
            {
                item.Metadata.Add("Email", Email);
            }
            return item;
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Username}";
        }
    }

    public struct UserData
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsSuperuser { get; set; }
        public bool? IsSystemAuditor { get; set; }
        public string? Password { get; set; }
    }
}
