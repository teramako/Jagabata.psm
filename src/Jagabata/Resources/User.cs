using System.Collections.Specialized;

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

    public class User(ulong id,
                      ResourceType type,
                      string url,
                      RelatedDictionary related,
                      SummaryFieldsDictionary summaryFields,
                      DateTime created,
                      DateTime? modified,
                      string username,
                      string firstName,
                      string lastName,
                      string email,
                      bool isSuperuser,
                      bool isSystemAuditor,
                      string password,
                      string ldapDn,
                      DateTime? lastLogin,
                      string externalAccount,
                      string[] auth)
        : IUser, IResource, ICacheableResource
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<User>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromOrganization(ulong organizationId,
                                                                        NameValueCollection? query = null,
                                                                        bool getAll = false)
        {
            var path = $"{Organization.PATH}{organizationId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromTeam(ulong teamId,
                                                                NameValueCollection? query = null,
                                                                bool getAll = false)
        {
            var path = $"{Team.PATH}{teamId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindOwnerFromCredential(ulong credentialId,
                                                                           NameValueCollection? query = null,
                                                                           bool getAll = false)
        {
            var path = $"{Credential.PATH}{credentialId}/owner_users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<User> FindFromRole(ulong roleId,
                                                                NameValueCollection? query = null,
                                                                bool getAll = false)
        {
            var path = $"{Role.PATH}{roleId}/users/";
            await foreach (var result in RestAPI.GetResultSetAsync<User>(path, query, getAll))
            {
                foreach (var user in result.Contents.Results)
                {
                    yield return user;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

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

        public CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Username, string.Empty);
            if (!string.IsNullOrEmpty(Email))
            {
                item.Metadata.Add("Email", Email);
            }
            return item;
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
