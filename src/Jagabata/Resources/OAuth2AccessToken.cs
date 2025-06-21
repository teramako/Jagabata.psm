using System.Runtime.CompilerServices;

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
        /// Get an Access Token
        /// <para>
        /// Implement API: <c>/api/v2/tokens/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<OAuth2AccessToken> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<OAuth2AccessToken>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static OAuth2AccessToken Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Assess Tokens
        /// <para>
        /// Implement API: <c>/api/v2/tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindAsync(HttpQuery? query = null,
                                                                          [EnumeratorCancellation]
                                                                          CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(PATH, query, ct))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Find Access Tokens associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/{Type}/{Id}/tokens/</c>
        /// <para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Application</item>
        ///     <item>User</item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<OAuth2AccessToken> FindAsync(IResource resource,
                                                                          HttpQuery? query = null,
                                                                          [EnumeratorCancellation]
                                                                          CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.OAuth2Application => $"{Resources.Application.PATH}{resource.Id}/tokens/",
                ResourceType.User => $"{Resources.User.PATH}{resource.Id}/tokens/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<OAuth2AccessToken>(path, query, ct))
            {
                foreach (var token in result.Contents.Results)
                {
                    yield return token;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static OAuth2AccessToken[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Access Tokens by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static OAuth2AccessToken[] Find(string? searchWords = null,
                                               string orderBy = "-id",
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
        public static OAuth2AccessToken[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Access Tokens associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static OAuth2AccessToken[] Find(IResource resource,
                                               string? searchWords = null,
                                               string orderBy = "-id",
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
        public string Description { get; } = description;
        public ulong User { get; } = user;
        public string Token { get; } = token;
        public string? RefreshToken { get; } = refreshToken;
        public ulong? Application { get; } = application;
        public DateTime Expires { get; } = expires;
        public string Scope { get; } = scope;

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/tokens/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/tokens/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
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
