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
        /// Retrieve a Credential.<br/>
        /// API Path: <c>/api/v2/credentials/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Credential> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Credential>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Credentials.<br/>
        /// API Path: <c>api/v2/credentials/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(PATH, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/credentials/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromOrganization(ulong organizationId,
                                                                              HttpQuery? query = null)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Galaxy Credentials for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/galaxy_credentials/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindGalaxyFromOrganization(ulong organizationId,
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
        /// <summary>
        /// List Credentials for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/credentials/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromUser(ulong userId,
                                                                      HttpQuery? query = null)
        {
            var path = $"{User.PATH}{userId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Team.<br/>
        /// API Path: <c>/api/v2/teams/<paramref name="teamId"/>/credentials/</c>
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromTeam(ulong teamId,
                                                                      HttpQuery? query = null)
        {
            var path = $"{Team.PATH}{teamId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Credential Type.<br/>
        /// API Path: <c>/api/v2/credential_type/<paramref name="credentialTypeId"/>/credentials/</c>
        /// </summary>
        /// <param name="credentialTypeId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromCredentialType(ulong credentialTypeId,
                                                                                HttpQuery? query = null)
        {
            var path = $"{Resources.CredentialType.PATH}{credentialTypeId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Inventory Source.<br/>
        /// API Path: <c>/api/v2/inventory_sources/<paramref name="inventorySourceId"/>/credentials/</c>
        /// </summary>
        /// <param name="inventorySourceId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromInventorySource(ulong inventorySourceId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Inventory Update.<br/>
        /// API Path: <c>/api/v2/inventory_updates/<paramref name="inventoryUpdateJobId"/>/credentials/</c>
        /// </summary>
        /// <param name="inventoryUpdateJobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromInventoryUpdateJob(ulong inventoryUpdateJobId,
                                                                                    HttpQuery? query = null)
        {
            var path = $"{InventoryUpdateJobBase.PATH}{inventoryUpdateJobId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Job Template.<br/>
        /// API Path: <c>/api/v2/job_templates/<paramref name="jobTemplateId"/>/credentials/</c>
        /// </summary>
        /// <param name="jobTemplateId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromJobTemplate(ulong jobTemplateId,
                                                                             HttpQuery? query = null)
        {
            var path = $"{JobTemplate.PATH}{jobTemplateId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Job.<br/>
        /// API Path: <c>/api/v2/jobs/<paramref name="jobId"/>/credentials/</c>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromJobTemplateJob(ulong jobId,
                                                                                HttpQuery? query = null)
        {
            var path = $"{JobTemplateJobBase.PATH}{jobId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Schedule.<br/>
        /// API Path: <c>/api/v2/schedule/<paramref name="scheduleId"/>/credentials/</c>
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromSchedule(ulong scheduleId,
                                                                          HttpQuery? query = null)
        {
            var path = $"{Schedule.PATH}{scheduleId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Workflow Job Template Node.<br/>
        /// API Path: <c>/api/v2/workflow_job_template_nodes/<paramref name="wjtnId"/>/credentials/</c>
        /// </summary>
        /// <param name="wjtnId">ID of Workflow Job Template Node</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromWorkflowJobTemplateNode(ulong wjtnId,
                                                                                         HttpQuery? query = null)
        {
            var path = $"{WorkflowJobTemplateNode.PATH}{wjtnId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }
        /// <summary>
        /// List Credentials for a Workflow Job Node.<br/>
        /// API Path: <c>/api/v2/workflow_job_nodes/<paramref name="wjnId"/>/credentials/</c>
        /// </summary>
        /// <param name="wjnId">ID of Workflow Job Node</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromWorkflowJobNode(ulong wjnId,
                                                                                 HttpQuery? query = null)
        {
            var path = $"{WorkflowJobNode.PATH}{wjnId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query))
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
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong? Organization { get; } = organization;
        public ulong CredentialType { get; } = credentialType;
        public bool Managed { get; } = managed;

        public Dictionary<string, object?> Inputs { get; } = inputs;
        public string Kind { get; } = kind;
        public bool Cloud { get; } = cloud;
        public bool Kubernetes { get; } = kubernetes;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/credentials/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public IEnumerable<ActivityStream> GetRecentActivityStream(ushort pageSize = 20)
        {
            return GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize);
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
