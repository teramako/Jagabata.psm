using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface ICredential
    {
        /// <summary>
        /// Name of this credential.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this credential.
        /// </summary>
        string Description { get; }
        ulong? Organization { get; }
        /// <summary>
        /// Specify the type of credential you want to create.
        /// Refer to the documentaion for detail on each type.
        /// </summary>
        ulong CredentialType { get; }
        /// <summary>
        /// Enter inputs using either JSON or YAML syntax.
        /// Refer to the documentaion for example syntax.
        /// </summary>
        Dictionary<string, object?> Inputs { get; }
    }


    public class Credential(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, ulong? organization, ulong credentialType, bool managed,
                            Dictionary<string, object?> inputs, string kind, bool cloud, bool kubernetes)
        : ResourceBase, ICredential
    {
        public const string PATH = "/api/v2/credentials/";

        /// <summary>
        /// Get a Credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/<paramref name="id"/>/</c>
        /// </apra>
        /// </summary>
        /// <param name="id">Credential ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Credential> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Credential>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Credential Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Credentials
        /// <para>
        /// Implement API: <c>api/v2/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindAsync(HttpQuery? query = null,
                                                                   [EnumeratorCancellation]
                                                                   CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(PATH, query, ct))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        /// <summary>
        /// Find Credentials associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Organization</item>
        ///     <item>User</item>
        ///     <item>Team</item>
        ///     <item>CredentialType</item>
        ///     <item>InventorySource</item>
        ///     <item>InventoryUpdate</item>
        ///     <item>JobTemplate</item>
        ///     <item>Job</item>
        ///     <item>Schedule</item>
        ///     <item>WorkflowJobTemplateNode</item>
        ///     <item>WorkflowJobNode</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindAsync(IResource resource,
                                                                   HttpQuery? query = null,
                                                                   [EnumeratorCancellation]
                                                                   CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Organization => $"{Resources.Organization.PATH}{resource.Id}/credentials/",
                ResourceType.User => $"{User.PATH}{resource.Id}/credentials/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/credentials/",
                ResourceType.CredentialType => $"{Resources.CredentialType.PATH}{resource.Id}/credentials/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{resource.Id}/credentials/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJobBase.PATH}{resource.Id}/credentials/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/credentials/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{resource.Id}/credentials/",
                ResourceType.Schedule => $"{Schedule.PATH}{resource.Id}/credentials/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{resource.Id}/credentials/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{resource.Id}/credentials/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, ct))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// Find Galaxy Credentials for an Organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/<paramref name="organizationId"/>/galaxy_credentials/</c>
        /// <para>
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindGalaxyAsync(ulong organizationId,
                                                                         HttpQuery? query = null)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/galaxy_credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Credential[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Credentials by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Credential[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Credential[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Credentials associated with <paramref name="resource"/> by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Credential[] Find(IResource resource,
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

        /// <inheritdoc cref="FindGalaxyAsync(ulong, HttpQuery?)"/>
        public static Credential[] FindGalaxy(ulong organizationId, HttpQuery? query = null)
        {
            return [.. FindGalaxyAsync(organizationId, query).ToBlockingEnumerable()];
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
        public ulong? Organization { get; } = organization;
        public ulong CredentialType { get; } = credentialType;
        public bool Managed { get; } = managed;

        public Dictionary<string, object?> Inputs { get; } = inputs;
        public string Kind { get; } = kind;
        public bool Cloud { get; } = cloud;
        public bool Kubernetes { get; } = kubernetes;

        [JsonIgnore]
        public OwnerSummary[] Owners =>
            SummaryFields.TryGetValue<OwnerSummary[]>("Owners", out var owners) ? owners : [];

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/credentials/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find the access list related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/access_list/</c>
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
        /// Find the access list related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAccessList(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Get the object roles related to this credential
        /// </summary>
        /// <remarks>
        /// This is almost same as:
        /// <code>thisObject.SummaryFields["ObjectRoles"]</code>
        /// </remarks>
        public ObjectRoleSummary[] GetObjectRoles()
        {
            return SummaryFields.TryGetValue<Dictionary<string, ObjectRoleSummary>>("ObjectRoles", out var dict)
                ? [.. dict.Values]
                : [];
        }

        /// <summary>
        /// Find the owner users related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/owner_users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindOwnerUsers(string? searchWords = null,
                                     string orderBy = "username",
                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<User>("owner_users",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find the owner users related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/owner_users/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindOwnerUsers(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("owner_users", query)];
        }

        /// <summary>
        /// Find the owner teams related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/owner_teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] FindOwnerTeams(string? searchWords = null,
                                     string orderBy = "name",
                                     ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Team>("owner_teams",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find the owner teams related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/owner_teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] FindOwnerTeams(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Team>("owner_teams", query)];
        }

        /// <summary>
        /// Find input sources related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public CredentialInputSource[] FindInputSources(string? searchWords = null,
                                                        string orderBy = "id",
                                                        ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<CredentialInputSource>("input_sources",
                                                                      searchWords,
                                                                      orderBy,
                                                                      pageSize)];
        }

        /// <summary>
        /// Find input sources related to this credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public CredentialInputSource[] FindInputSources(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<CredentialInputSource>("input_sources", query)];
        }

        /// <summary>
        /// Get the credential type related this credential
        /// </summary>
        public CredentialType? GetCredentialType()
        {
            return Related.TryGetPath("credential_type", out var path)
                ? RestAPI.Get<CredentialType>(path)
                : null;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Kind) ? $"{Type}:{Id}:{Name}" : $"{Type}:{Id}:{Kind}:{Name}";
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Kind"] = Kind,
                }
            };
            if (SummaryFields.TryGetValue<CredentialTypeSummary>("CredentialType", out var ct))
            {
                item.Metadata.Add("CredentialType", $"[{ct.Type}:{ct.Id}] {ct.Name}");
            }
            return item;
        }
    }
}
