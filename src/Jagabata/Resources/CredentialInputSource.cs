using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface ICredentialInputSource
    {
        string Description { get; }
        string InputFieldName { get; }
        Dictionary<string, object?> Metadata { get; }
        ulong TargetCredential { get; }
        ulong SourceCredential { get; }
    }

    public class CredentialInputSource(ulong id, ResourceType type, string url, RelatedDictionary related,
                                       SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                       string description, string inputFieldName, Dictionary<string, object?> metadata,
                                       ulong targetCredential, ulong sourceCredential)
        : ResourceBase, ICredentialInputSource
    {
        public const string PATH = "/api/v2/credential_input_sources/";

        /// <summary>
        /// Get a Credential Input Source
        /// <para>
        /// Implement API: <c>/api/v2/credential_input_sources/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Credential Input Source ID</param>
        /// <param name="ct">Cancellation token</param>
        public static async Task<CredentialInputSource> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<CredentialInputSource>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static CredentialInputSource Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Credential Input Sources
        /// <para>
        /// Implement API: <c>api/v2/credential_input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        public static async IAsyncEnumerable<CredentialInputSource> FindAsync(HttpQuery? query = null,
                                                                              [EnumeratorCancellation]
                                                                              CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(PATH, query, ct))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        /// <summary>
        /// Find Credential Input Sources for a Credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/<paramref name="credentialId"/>/input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="credentialId">Credential ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        public static async IAsyncEnumerable<CredentialInputSource> FindAsync(ulong credentialId,
                                                                              HttpQuery? query = null,
                                                                              [EnumeratorCancellation]
                                                                              CancellationToken ct = default)
        {
            var path = $"{Credential.PATH}{credentialId}/input_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(path, query, ct))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static CredentialInputSource[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find CredentialInputSources by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/credential_input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static CredentialInputSource[] Find(string? searchWords = null,
                                                   string orderBy = "id",
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
        /// Find CredentianlInputSources for a Credential
        /// <para>
        /// Implement API: <c>/api/v2/credentials/<paramref name="credentialId"/>/input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="credentialId">Credential ID</param>
        /// <param name="query"></param>
        public static CredentialInputSource[] Find(ulong credentialId, HttpQuery query)
        {
            return [.. FindAsync(credentialId, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find CredentianlInputSources for a Credential by basic parameters
        /// <para>
        /// Implement API: <c>/api/v2/credentials/<paramref name="credentialId"/>/input_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="credentialId">Credential ID</param>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static CredentialInputSource[] Find(ulong credentialId,
                                                   string? searchWords = null,
                                                   string orderBy = "id",
                                                   ushort pageSize = 20,
                                                   uint startPage = 1)
        {
            return Find(credentialId, new QueryBuilder().SetSearchWords(searchWords)
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
        public string InputFieldName { get; } = inputFieldName;
        public Dictionary<string, object?> Metadata { get; } = metadata;
        public ulong TargetCredential { get; } = targetCredential;
        public ulong SourceCredential { get; } = sourceCredential;

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, Description)
            {
                Metadata = {
                    ["InputFieldName"] = InputFieldName,
                    ["TargetCredential"] = $"{TargetCredential}",
                    ["SourceCredential"] = $"{SourceCredential}"
                }
            };
        }
    }
}
