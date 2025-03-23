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
        /// Retrieve a Credential Input Source.<br/>
        /// API Path: <c>/api/v2/credential_input_sources/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<CredentialInputSource> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<CredentialInputSource>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Credential Input Sources.<br/>
        /// API Path: <c>api/v2/credential_input_sources/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialInputSource> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(PATH, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credential Input Sources for a Credential.<br/>
        /// API Path: <c>/api/v2/credentials/<paramref name="credentialId"/>/input_sources/</c>
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialInputSource> FindFromCredential(ulong credentialId,
                                                                                       HttpQuery? query = null)
        {
            var path = $"{Credential.PATH}{credentialId}/input_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
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
