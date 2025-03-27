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
        /// Retieve an Application.<br/>
        /// API Path: <c>/api/v2/applications/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Application> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Application>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Applications.<br/>
        /// API Path: <c>/api/v2/applications/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Application> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Application>(PATH, query))
            {
                foreach (var app in result.Contents.Results)
                {
                    yield return app;
                }
            }
        }
        /// <summary>
        /// List Applications for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/applications/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Application> FindFromOrganization(ulong organizationId,
                                                                               HttpQuery? query = null)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/applications/";
            await foreach (var result in RestAPI.GetResultSetAsync<Application>(path, query))
            {
                foreach (var app in result.Contents.Results)
                {
                    yield return app;
                }
            }
        }
        /// <summary>
        /// List Applications for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/applications/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Application> FindFromUser(ulong userId,
                                                                       HttpQuery? query = null)
        {
            var path = $"{User.PATH}{userId}/applications/";
            await foreach (var result in RestAPI.GetResultSetAsync<Application>(path, query))
            {
                foreach (var app in result.Contents.Results)
                {
                    yield return app;
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
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/applications/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/applications/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }
    }
}
