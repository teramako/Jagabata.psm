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
        /// Retrieve information about the current User.<br/>
        /// API Path: <c>/api/v2/me/</c>
        /// </summary>
        /// <returns></returns>
        public static async Task<User> GetMe()
        {
            var apiResult = await RestAPI.GetAsync<ResultSet<User>>("/api/v2/me/");
            return apiResult.Contents.Results.Single();
        }
        /// <summary>
        /// Retrieve a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<User> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<User>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Users.<br/>
        /// API Path: <c>/api/v2/users/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<User>(PATH, query))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }
        /// <summary>
        /// List Users for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/users/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromOrganization(ulong organizationId,
                                                                        HttpQuery? query = null)
        {
            var path = $"{Organization.PATH}{organizationId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }
        /// <summary>
        /// List Users for a Team.<br/>
        /// API Path: <c>/api/v2/teams/<paramref name="teamId"/>/users/</c>
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromTeam(ulong teamId,
                                                                HttpQuery? query = null)
        {
            var path = $"{Team.PATH}{teamId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }
        /// <summary>
        /// List Users for a Credential.<br/>
        /// API Path: <c>/api/v2/credentials/<paramref name="credentialId"/>/owner_users/</c>
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindOwnerFromCredential(ulong credentialId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Credential.PATH}{credentialId}/owner_users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }
        /// <summary>
        /// List Users for a Role.<br/>
        /// API Path: <c>/api/v2/roles/<paramref name="roleId"/>/users/</c>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromRole(ulong roleId,
                                                                HttpQuery? query = null)
        {
            var path = $"{Role.PATH}{roleId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
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
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/users/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the teams related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] GetTeams(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Team>("teams", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the teams related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] GetTeams(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Team>("teams", query)];
        }

        /// <summary>
        /// Get the organizations related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Organization[] GetOrganizations(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Organization>("organizations", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the organizations related to this user
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Organization[] GetOrganizations(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Organization>("organizations", query)];
        }

        /// <summary>
        /// Get the organizations for which this user is an administrator.
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/admin_of_organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Organization[] GetAdminOfOrganizations(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Organization>("admin_of_organizations", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the organizations for which this user is an administrator.
        /// <para>
        /// Implement API: <c>/api/v2/users/{id}/admin_of_organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Organization[] GetAdminOfOrganizations(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Organization>("admin_of_organizations", query)];
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
