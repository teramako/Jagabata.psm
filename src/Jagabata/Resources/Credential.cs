using System.Collections.Specialized;

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


    public class Credential(ulong id,
                            ResourceType type,
                            string url,
                            RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields,
                            DateTime created,
                            DateTime? modified,
                            string name,
                            string description,
                            ulong? organization,
                            ulong credentialType,
                            bool managed,
                            Dictionary<string, object?> inputs,
                            string kind,
                            bool cloud,
                            bool kubernetes)
        : SummaryFieldsContainer, ICredential, IResource, ICacheableResource
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(PATH, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromOrganization(ulong organizationId,
                                                                              NameValueCollection? query = null,
                                                                              bool getAll = false)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindGalaxyFromOrganization(ulong organizationId,
                                                                                    NameValueCollection? query = null,
                                                                                    bool getAll = false)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/galaxy_credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromUser(ulong userId,
                                                                      NameValueCollection? query = null,
                                                                      bool getAll = false)
        {
            var path = $"{User.PATH}{userId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromTeam(ulong teamId,
                                                                      NameValueCollection? query = null,
                                                                      bool getAll = false)
        {
            var path = $"{Team.PATH}{teamId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromCredentialType(ulong credentialTypeId,
                                                                                NameValueCollection? query = null,
                                                                                bool getAll = false)
        {
            var path = $"{Resources.CredentialType.PATH}{credentialTypeId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromInventorySource(ulong inventorySourceId,
                                                                                 NameValueCollection? query = null,
                                                                                 bool getAll = false)
        {
            var path = $"{InventorySource.PATH}{inventorySourceId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromInventoryUpdateJob(ulong inventoryUpdateJobId,
                                                                                    NameValueCollection? query = null,
                                                                                    bool getAll = false)
        {
            var path = $"{InventoryUpdateJob.PATH}{inventoryUpdateJobId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromJobTemplate(ulong jobTemplateId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{JobTemplate.PATH}{jobTemplateId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromJobTemplateJob(ulong jobId,
                                                                                NameValueCollection? query = null,
                                                                                bool getAll = false)
        {
            var path = $"{JobTemplateJob.PATH}{jobId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromSchedule(ulong scheduleId,
                                                                          NameValueCollection? query = null,
                                                                          bool getAll = false)
        {
            var path = $"{Schedule.PATH}{scheduleId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromWorkflowJobTemplateNode(ulong wjtnId,
                                                                                         NameValueCollection? query = null,
                                                                                         bool getAll = false)
        {
            var path = $"{WorkflowJobTemplateNode.PATH}{wjtnId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
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
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Credential> FindFromWorkflowJobNode(ulong wjnId,
                                                                                 NameValueCollection? query = null,
                                                                                 bool getAll = false)
        {
            var path = $"{WorkflowJobNode.PATH}{wjnId}/credentials/";
            await foreach (var result in RestAPI.GetResultSetAsync<Credential>(path, query, getAll))
            {
                foreach (var credential in result.Contents.Results)
                {
                    yield return credential;
                }
            }
        }

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
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

        public CacheItem GetCacheItem()
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
