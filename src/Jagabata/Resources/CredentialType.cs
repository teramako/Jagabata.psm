using System.Text.Json.Serialization;
using Jagabata.CredentialType;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<CredentialTypeKind>))]
    public enum CredentialTypeKind
    {
        /// <summary>
        /// Machine
        /// </summary>
        ssh,
        /// <summary>
        /// Vault
        /// </summary>
        vault,
        /// <summary>
        /// Network
        /// </summary>
        net,
        /// <summary>
        /// Source Control
        /// </summary>
        scm,
        /// <summary>
        /// Cloud
        /// </summary>
        cloud,
        /// <summary>
        /// Container Registry
        /// </summary>
        registry,
        /// <summary>
        /// Personal Access Token
        /// </summary>
        token,
        /// <summary>
        /// Insights
        /// </summary>
        insights,
        /// <summary>
        /// External
        /// </summary>
        external,
        /// <summary>
        /// Kubernetes
        /// </summary>
        kubernetes,
        /// <summary>
        /// Galaxy/Automation Hub
        /// </summary>
        galaxy,
        /// <summary>
        /// Cryptography
        /// </summary>
        cryptography
    }

    public interface ICredentialType
    {
        /// <summary>
        /// Name of this credential type.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this credential type.
        /// </summary>
        string Description { get; }
        CredentialTypeKind Kind { get; }
        FieldList Inputs { get; }
        Injectors Injectors { get; }
    }

    public class CredentialType(ulong id, ResourceType type, string url, RelatedDictionary related,
                                SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                                string description, CredentialTypeKind kind, string nameSpace, bool managed,
                                FieldList inputs, Injectors injectors)
        : ResourceBase, ICredentialType
    {
        public const string PATH = "/api/v2/credential_types/";

        /// <summary>
        /// Retrieve a Credential Type.<br/>
        /// API Path: <c>/api/v2/credential_types/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<CredentialType> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<CredentialType>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Credential Types.<br/>
        /// API Path: <c>/api/v2/credential_types/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialType> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialType>(PATH, query))
            {
                foreach (var credentialType in result.Contents.Results)
                {
                    yield return credentialType;
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
        public CredentialTypeKind Kind { get; } = kind;
        public string Namespace { get; } = nameSpace;
        public bool Managed { get; } = managed;
        public FieldList Inputs { get; } = inputs;
        public Injectors Injectors { get; } = injectors;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/credential_types/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/credential_types/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Kind"] = $"{Kind}",
                    ["Namespace"] = Namespace,
                    ["Managed"] = $"{Managed}"
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Kind}:{Name}";
        }
    }
}
