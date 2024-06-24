using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace AWX.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<ActivityStreamOperation>))]
    public enum ActivityStreamOperation
    {
        Create,
        Update,
        Delete,
        Associate,
        Disassociate,
    }

    [ResourceType(ResourceType.ActivityStream)]
    public class ActivityStream(ulong id,
                          ResourceType type,
                          string url,
                          RelatedDictionary related,
                          ActivityStream.Summary summaryFields,
                          DateTime timestamp,
                          ActivityStreamOperation operation,
                          Dictionary<string, object?> changes,
                          string object1,
                          string object2,
                          string objectAssociation,
                          string actionNode,
                          string objectType)
        : IResource<ActivityStream.Summary>
    {
        public const string PATH = "/api/v2/activity_stream/";

        /// <summary>
        /// Retrieve an Activity Stream.<br/>
        /// API Path: <c>/api/v2/activity_stream/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ActivityStream> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<ActivityStream>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Activity Sterams.<br/>
        /// API Path: <c>/api/v2/activity_stream/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach(var result in RestAPI.GetResultSetAsync<ActivityStream>(PATH, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Application.<br/>
        /// API Path: <c>/api/v2/applications/<paramref name="applicationId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromApplication(ulong applicationId,
                                                                                 NameValueCollection? query = null,
                                                                                 bool getAll = false)
        {
            var path = $"{Application.PATH}{applicationId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Access Token.<br/>
        /// API Path: <c>/api/v2/tokens/<paramref name="tokenId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromToken(ulong tokenId,
                                                                           NameValueCollection? query = null,
                                                                           bool getAll = false)
        {
            var path = $"{OAuth2AccessToken.PATH}{tokenId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromOrganization(ulong organizationId,
                                                                                  NameValueCollection? query = null,
                                                                                  bool getAll = false)
        {
            var path = $"{Organization.PATH}{organizationId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromUser(ulong userId,
                                                                          NameValueCollection? query = null,
                                                                          bool getAll = false)
        {
            var path = $"{User.PATH}{userId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Project.<br/>
        /// API Path: <c>/api/v2/projects/<paramref name="projectId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromProject(ulong projectId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{Project.PATH}{projectId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Team.<br/>
        /// API Path: <c>/api/v2/teams/<paramref name="teamId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromTeam(ulong teamId,
                                                                          NameValueCollection? query = null,
                                                                          bool getAll = false)
        {
            var path = $"{Team.PATH}{teamId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Credential.<br/>
        /// API Path: <c>/api/v2/credentials/<paramref name="credentialId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromCredential(ulong credentialId,
                                                                                NameValueCollection? query = null,
                                                                                bool getAll = false)
        {
            var path = $"{Credential.PATH}{credentialId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a CredentialType.<br/>
        /// API Path: <c>/api/v2/credential_types/<paramref name="credentialTypeId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="credentialTypeId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromCredentialType(ulong credentialTypeId,
                                                                                NameValueCollection? query = null,
                                                                                bool getAll = false)
        {
            var path = $"{CredentialType.PATH}{credentialTypeId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromInventory(ulong inventoryId,
                                                                               NameValueCollection? query = null,
                                                                               bool getAll = false)
        {
            var path = $"{Inventory.PATH}{inventoryId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query, getAll))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        public class Summary(UserSummary? actor)
        {
            public UserSummary? Actor { get; set; } = actor;
            [JsonExtensionData]
            public Dictionary<string, object>? ExtensionData { get; set; }
        }

        public ulong Id { get; } = id;

        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;

        [JsonPropertyOrder(10)]
        public DateTime Timestamp { get; } = timestamp;
        [JsonPropertyOrder(11)]
        public ActivityStreamOperation Operation { get; } = operation;
        [JsonPropertyOrder(12)]
        public Dictionary<string, object?> Changes { get; } = changes;
        [JsonPropertyOrder(13)]
        public string Object1 { get; } = object1;
        [JsonPropertyOrder(14)]
        public string Object2 { get; } = object2;
        [JsonPropertyOrder(15)]
        [JsonPropertyName("object_association")]
        public string ObjectAssociation { get; } = objectAssociation;
        [JsonPropertyOrder(16)]
        [JsonPropertyName("action_node")]
        public string ActionNode { get; } = actionNode;
        [JsonPropertyOrder(17)]
        [JsonPropertyName("object_type")]
        public string ObjectType { get; } = objectType;
    }
}
