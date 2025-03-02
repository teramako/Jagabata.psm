using System.Collections.Specialized;
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

    public class Organization(ulong id,
                              ResourceType type,
                              string url,
                              RelatedDictionary related,
                              SummaryFieldsDictionary summaryFields,
                              DateTime created,
                              DateTime? modified,
                              string name,
                              string description,
                              int maxHosts,
                              string? customVirtualenv,
                              int? defaultEnvironment)
        : SummaryFieldsContainer, IOrganization, IResource, ICacheableResource
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindAdministeredByUser(ulong userId,
                                                                                    NameValueCollection? query = null,
                                                                                    bool getAll = false)
        {
            var path = $"{User.PATH}/{userId}/admin_of_organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindFromUser(ulong userId,
                                                                        NameValueCollection? query = null,
                                                                        bool getAll = false)
        {
            var path = $"{User.PATH}/{userId}/organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query, getAll))
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

        public CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
