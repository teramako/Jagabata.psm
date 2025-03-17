using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IOAuth2AccessToken
    {
        /// <summary>
        /// Optional description of this access token.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Application ID
        /// </summary>
        ulong? Application { get; }
        /// <summary>
        /// Allowed scopes, further restricts users's permissions.
        /// Must be a simple space-sparated string with allowed scopes <c>['read', 'write']</c>.
        /// </summary>
        string Scope { get; }
    }

    public class OAuth2AccessToken(ulong id, ResourceType type, string url, RelatedDictionary related,
                                   SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                   string description, ulong user, string token, string? refreshToken,
                                   ulong? application, DateTime expires, string scope)
        : ResourceBase, IOAuth2AccessToken
    {
        public const string PATH = "/api/v2/tokens/";
        /// <summary>
        /// Retieve an Access Token.<br/>
        /// API Path: <c>/api/v2/tokens/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<OAuth2AccessToken> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<OAuth2AccessToken>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Assess Tokens.<br/>
        /// API Path: <c>/api/v2/tokens/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(PATH, query, getAll))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }
        }
        /// <summary>
        /// List Access Tokens for an Application.<br/>
        /// API Path: <c>/api/v2/applications/<paramref name="applicationId"/>/tokens/</c>
        /// </summary>
        /// <param name="applicationId">Application ID</param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindFromApplication(ulong applicationId,
                                                                                    NameValueCollection? query = null,
                                                                                    bool getAll = false)
        {
            var path = $"{Resources.Application.PATH}{applicationId}/tokens/";
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(path, query, getAll))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }
        }
        /// <summary>
        /// List Access Tokens for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/tokens/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindFromUser(ulong userId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{Resources.User.PATH}{userId}/tokens/";
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(path, query, getAll))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }

        }
        /// <summary>
        /// List Access Tokens for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/personal_tokens/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindPersonalTokensFromUser(ulong userId,
                                                                                           NameValueCollection? query = null,
                                                                                           bool getAll = false)
        {
            var path = $"{Resources.User.PATH}{userId}/personal_tokens/";
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(path, query, getAll))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }

        }
        /// <summary>
        /// List Access Tokens for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/authorized_tokens/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindAuthorizedTokensFromUser(ulong userId,
                                                                                             NameValueCollection? query = null,
                                                                                             bool getAll = false)
        {
            var path = $"{Resources.User.PATH}{userId}/authorized_tokens/";
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(path, query, getAll))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
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
        public string Description { get; } = description;
        public ulong User { get; } = user;
        public string Token { get; } = token;
        public string? RefreshToken { get; } = refreshToken;
        public ulong? Application { get; } = application;
        public DateTime Expires { get; } = expires;
        public string Scope { get; } = scope;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/tokens/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public IEnumerable<ActivityStream> GetRecentActivityStream(int pageSize = 20)
        {
            return GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize);
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, string.Empty, Description);
            if (Application is null && SummaryFields.TryGetValue<UserSummary>("User", out var user))
            {
                item.Metadata.Add("User", $"[{user.Type}:{user.Id} {user.Username}");
            }
            else if (SummaryFields.TryGetValue<ApplicationSummary>("Application", out var app))
            {
                item.Metadata.Add("Application", $"[{app.Type}:{app.Id} {app.Name}");
            }
            item.Metadata.Add("Scope", Scope);
            return item;
        }
    }
}
