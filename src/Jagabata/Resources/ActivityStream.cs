using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
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

    public class ActivityStream(ulong id,
                                ResourceType type,
                                string url,
                                RelatedDictionary related,
                                SummaryFieldsDictionary summaryFields,
                                DateTime timestamp,
                                ActivityStreamOperation operation,
                                Dictionary<string, object?> changes,
                                ResourceType object1,
                                ResourceType object2,
                                string objectAssociation,
                                string actionNode,
                                ResourceType objectType)
        : ResourceBase
    {
        public const string PATH = "/api/v2/activity_stream/";

        /// <summary>
        /// Get an ActivityStream.
        /// <para>
        /// Implement API: <c>/api/v2/activity_stream/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">ActivityStream ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<ActivityStream> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<ActivityStream>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static ActivityStream Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find ActivityStream
        /// <para>
        /// Implement API: <c>/api/v2/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindAsync(HttpQuery? query = null,
                                                                       [EnumeratorCancellation]
                                                                       CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(PATH, query, ct))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }

        /// <summary>
        /// Find ActivityStream associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/activity_stream/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Application (OAuth2Application)</item>
        ///     <item>Token (OAuth2AccessToken)</item>
        ///     <item>Organization</item>
        ///     <item>User</item>
        ///     <item>Project</item>
        ///     <item>Team</item>
        ///     <item>Credential</item>
        ///     <item>CredentialType</item>
        ///     <item>Inventory</item>
        ///     <item>InventorySource</item>
        ///     <item>Group</item>
        ///     <item>Host</item>
        ///     <item>JobTemplate</item>
        ///     <item>Job</item>
        ///     <item>AdHocCommand</item>
        ///     <item>WorkflowJobTemplate</item>
        ///     <item>WorkflowJob</item>
        ///     <item>ExecutionEnvironment</item>
        /// </list>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindAsync(IResource resource,
                                                                       HttpQuery? query = null,
                                                                       [EnumeratorCancellation] CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.OAuth2Application => $"{Application.PATH}{resource.Id}/activity_stream/",
                ResourceType.OAuth2AccessToken => $"{OAuth2AccessToken.PATH}{resource.Id}/activity_stream/",
                ResourceType.Organization => $"{Organization.PATH}{resource.Id}/activity_stream/",
                ResourceType.User => $"{User.PATH}{resource.Id}/activity_stream/",
                ResourceType.Project => $"{Project.PATH}{resource.Id}/activity_stream/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/activity_stream/",
                ResourceType.Credential => $"{Credential.PATH}{resource.Id}/activity_stream/",
                ResourceType.CredentialType => $"{CredentialType.PATH}{resource.Id}/activity_stream/",
                ResourceType.Inventory => $"{Inventory.PATH}{resource.Id}/activity_stream/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{resource.Id}/activity_stream/",
                ResourceType.Group => $"{Group.PATH}{resource.Id}/activity_stream/",
                ResourceType.Host => $"{Host.PATH}{resource.Id}/activity_stream/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/activity_stream/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{resource.Id}/activity_stream/",
                ResourceType.AdHocCommand => $"{AdHocCommandBase.PATH}{resource.Id}/activity_stream/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{resource.Id}/activity_stream/",
                ResourceType.WorkflowJob => $"{WorkflowJobBase.PATH}{resource.Id}/activity_stream/",
                ResourceType.ExecutionEnvironment => $"{ExecutionEnvironment.PATH}{resource.Id}/activity_stream/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var apiResult in RestAPI.GetResultSetAsync<ActivityStream>(path, query, ct))
            {
                foreach (var activity in apiResult.Contents.Results)
                {
                    yield return activity;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static ActivityStream[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find ActivityStream by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static ActivityStream[] Find(string? searchWords = null,
                                            string orderBy = "-timestamp",
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
        public static ActivityStream[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find ActivityStream associated with <paramref name="resource"/> by basic parammeters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static ActivityStream[] Find(IResource resource,
                                            string? searchWords = null,
                                            string orderBy = "-timestamp",
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

        public DateTime Timestamp { get; } = timestamp;
        public ActivityStreamOperation Operation { get; } = operation;
        public Dictionary<string, object?> Changes { get; } = changes;
        public ResourceType Object1 { get; } = object1;
        public ResourceType Object2 { get; } = object2;
        public string ObjectAssociation { get; } = objectAssociation;
        public string ActionNode { get; } = actionNode;
        public ResourceType ObjectType { get; } = objectType;

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, $"{Operation}", $"{Timestamp}")
            {
                Metadata = {
                    ["Object1"] = $"{Object1}",
                    ["Object2"] = $"{Object2}",
                    ["ObjectAssociation"] = ObjectAssociation,
                }
            };
            if (SummaryFields.TryGetValue<UserSummary>("Actor", out var actor))
            {
                item.Metadata.Add("Actor", $"[{actor.Type}:{actor.Id}] {actor.Username}");
            }
            return item;
        }
    }
}
