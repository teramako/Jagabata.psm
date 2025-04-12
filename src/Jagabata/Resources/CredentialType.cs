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
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/credential_types/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/credential_types/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find credentials related to this credential type
        /// <para>
        /// Implement API: <c>/api/v2/credential_types/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Name(s) of sort key</param>
        /// <param name="pageSize">Max number of credential_types to retrieve</param>
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
        /// Find credentials related to this credential type
        /// <para>
        /// Implement API: <c>/api/v2/credential_types/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
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
