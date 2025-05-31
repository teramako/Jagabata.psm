using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<ApplicationClientType>))]
    public enum ApplicationClientType
    {
        Confidential,
        Public
    }

    public interface IApplication
    {
        /// <summary>
        /// Name of this application.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this application.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Set to <c>Public</c> or <c>Confidential</c> depending on how secure the client device is.
        /// </summary>
        ApplicationClientType ClientType { get; }
        /// <summary>
        /// Allowed URIs list, space spareted.
        /// </summary>
        string RedirectUris { get; }
        /// <summary>
        /// The Grant type the user must use for acquire tokens for this application.
        /// </summary>
        string AuthorizationGrantType { get; }
        /// <summary>
        /// Set <c>True</c> to skip authorization step for completely trusted applications.
        /// </summary>
        bool SkipAuthorization { get; }
        /// <summary>
        /// Organization containing thie application.
        /// </summary>
        ulong Organization { get; }
    }

    public class Application(ulong id,
                             ResourceType type,
                             string url,
                             RelatedDictionary related,
                             SummaryFieldsDictionary summaryFields,
                             DateTime created,
                             DateTime? modified,
                             string name,
                             string description,
                             string clientId,
                             ApplicationClientType clientType,
                             string? clientSecret,
                             string redirectUris,
                             string authorizationGrantType,
                             bool skipAuthorization,
                             ulong organization)
        : ResourceBase, IApplication
    {
        public const string PATH = "/api/v2/applications/";

        /// <summary>
        /// Get an Application
        /// <para>
        /// Implement API: <c>/api/v2/applications/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<Application> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Application>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <summary>
        /// Find Applications
        /// <para>
        /// Implement API: <c>/api/v2/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Application> FindAsync(HttpQuery? query = null,
                                                                    [EnumeratorCancellation]
                                                                    CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Application>(PATH, query, ct))
            {
                foreach (var app in result.Contents.Results)
                {
                    yield return app;
                }
            }
        }

        /// <summary>
        /// Find Applications associated with <paramref name="resource"/>.
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/applications/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Organization</item>
        ///     <item>User</item>
        /// </list>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Application> FindAsync(IResource resource,
                                                                    HttpQuery? query = null,
                                                                    [EnumeratorCancellation]
                                                                    CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Organization => $"{Resources.Organization.PATH}{resource.Id}/applications/",
                ResourceType.User => $"{User.PATH}{resource.Id}/applications/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var apiResult in RestAPI.GetResultSetAsync<Application>(path, query, ct))
            {
                foreach (var activity in apiResult.Contents.Results)
                {
                    yield return activity;
                }
            }
        }

        /// <summary>
        /// Find Applications
        /// <para>
        /// Implement API: <c>/api/v2/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        public static Application[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Applications by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Application[] Find(string? searchWords = null,
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
        /// Find Applications associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/applications/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Organization</item>
        ///     <item>User</item>
        /// </list>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="query"></param>
        public static Application[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find applications associated with <paramref name="resource"/> by basic parammeters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Application[] Find(IResource resource,
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
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string ClientId { get; } = clientId;
        public ApplicationClientType ClientType { get; } = clientType;
        public string? ClientSecret { get; } = clientSecret;
        public string RedirectUris { get; } = redirectUris;
        public string AuthorizationGrantType { get; } = authorizationGrantType;
        public bool SkipAuthorization { get; } = skipAuthorization;
        public ulong Organization { get; } = organization;

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/applications/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/applications/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find tokens related to this application
        /// <para>
        /// Implement API: <c>/api/v2/applications/{id}/tokens/</c>
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
        /// Find tokens related to this application
        /// <para>
        /// Implement API: <c>/api/v2/applications/{id}/tokens/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public OAuth2AccessToken[] FindTokens(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<OAuth2AccessToken>("tokens", query)];
        }

        /// <summary>
        /// Get the organization related to this application
        /// </summary>
        public Organization? GetOrganization()
        {
            return Related.TryGetPath("organization", out var path)
                ? RestAPI.Get<Organization>(path)
                : null;
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
