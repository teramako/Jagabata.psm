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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> Find(HttpQuery? query)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(PATH, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromApplication(ulong applicationId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{Application.PATH}{applicationId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromToken(ulong tokenId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{OAuth2AccessToken.PATH}{tokenId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromOrganization(ulong organizationId,
                                                                                  HttpQuery? query = null)
        {
            var path = $"{Organization.PATH}{organizationId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromUser(ulong userId,
                                                                          HttpQuery? query = null)
        {
            var path = $"{User.PATH}{userId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromProject(ulong projectId,
                                                                             HttpQuery? query = null)
        {
            var path = $"{Project.PATH}{projectId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromTeam(ulong teamId,
                                                                          HttpQuery? query = null)
        {
            var path = $"{Team.PATH}{teamId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromCredential(ulong credentialId,
                                                                                HttpQuery? query = null)
        {
            var path = $"{Credential.PATH}{credentialId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromCredentialType(ulong credentialTypeId,
                                                                                    HttpQuery? query = null)
        {
            var path = $"{CredentialType.PATH}{credentialTypeId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
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
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromInventory(ulong inventoryId,
                                                                               HttpQuery? query = null)
        {
            var path = $"{Inventory.PATH}{inventoryId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Inventory Source.<br/>
        /// API Path: <c>/api/v2/inventory_sources/<paramref name="inventorySourceId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="inventorySourceId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromInventorySource(ulong inventorySourceId,
                                                                                     HttpQuery? query = null)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromGroup(ulong groupId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromHost(ulong hostId,
                                                                          HttpQuery? query = null)
        {
            var path = $"{Host.PATH}{hostId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Job Template.<br/>
        /// API Path: <c>/api/v2/job_templates/<paramref name="jobTemplateId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="jobTemplateId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromJobTemplate(ulong jobTemplateId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{JobTemplate.PATH}{jobTemplateId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="jobId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromJob(ulong jobId,
                                                                         HttpQuery? query = null)
        {
            var path = $"{JobTemplateJobBase.PATH}{jobId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Ad Hoc Command.<br/>
        /// API Path: <c>/api/v2/ad_hoc_commands/<paramref name="cmdId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromAdHocCommand(ulong cmdId,
                                                                                  HttpQuery? query = null)
        {
            var path = $"{AdHocCommandBase.PATH}{cmdId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Workflow Job Template.<br/>
        /// API Path: <c>/api/v2/workflow_job_templates/<paramref name="wjtId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="wjtId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromWorkflowJobTemplate(ulong wjtId,
                                                                                         HttpQuery? query = null)
        {
            var path = $"{WorkflowJobTemplate.PATH}{wjtId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for a Workflow Job.<br/>
        /// API Path: <c>/api/v2/workflow_jobs/<paramref name="jobId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromWorkflowJob(ulong jobId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{WorkflowJobBase.PATH}{jobId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
        }
        /// <summary>
        /// List Activity Stream for an Execution Environment.<br/>
        /// API Path: <c>/api/v2/execution_environments/<paramref name="exeEnvId"/>/activity_stream/</c>
        /// </summary>
        /// <param name="exeEnvId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ActivityStream> FindFromExecutionEnvironment(ulong exeEnvId,
                                                                                          HttpQuery? query = null)
        {
            var path = $"{ExecutionEnvironment.PATH}{exeEnvId}/activity_stream/";
            await foreach (var result in RestAPI.GetResultSetAsync<ActivityStream>(path, query))
            {
                foreach (var activity in result.Contents.Results)
                {
                    yield return activity;
                }
            }
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
