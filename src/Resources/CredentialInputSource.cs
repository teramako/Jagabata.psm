using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface ICredentialInputSource
    {
        public string Description { get; }
        public string InputFieldName { get; }
        public Dictionary<string, object?> Metadata { get; }
        public ulong TargetCredential { get; }
        public ulong SourceCredential { get; }
    }

    public class CredentialInputSource(ulong id, ResourceType type, string url, RelatedDictionary related,
                                       CredentialInputSource.Summary summaryFields, DateTime created, DateTime? modified,
                                       string description, string inputFieldName, Dictionary<string, object?> metadata,
                                       ulong targetCredential, ulong sourceCredential)
        : ICredentialInputSource, IResource<CredentialInputSource.Summary>
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialInputSource> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialInputSource> FindFromCredential(ulong credentialId,
                                                                                       NameValueCollection? query = null,
                                                                                       bool getAll = false)
        {
            var path = $"{Credential.PATH}{credentialId}/input_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialInputSource>(path, query, getAll))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        public record Summary(CredentialSummary SourceCredential,
                              CredentialSummary TargetCredential,
                              UserSummary CreatedBy,
                              UserSummary? ModifiedBy,
                              Capability UserCapabilities);

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Description { get; } = description;
        public string InputFieldName { get; } = inputFieldName;
        public Dictionary<string, object?> Metadata { get; } = metadata;
        public ulong TargetCredential { get; } = targetCredential;
        public ulong SourceCredential { get; } = sourceCredential;
    }
}
