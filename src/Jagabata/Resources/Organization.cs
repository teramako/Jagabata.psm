using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    public interface IOrganization
    {
        string Name { get; }
        string Description { get; }
        int MaxHosts { get; }
        int? DefaultEnvironment { get; }
    }

    public class Organization(ulong id, ResourceType type, string url, RelatedDictionary related,
                              SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                              string description, int maxHosts, string? customVirtualenv, int? defaultEnvironment)
        : ResourceBase, IOrganization
    {
        public const string PATH = "/api/v2/organizations/";
        /// <summary>
        /// Retrieve an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Organization> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Organization>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Organizations.<br/>
        /// API Path: <c>/api/v2/organizations/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(PATH, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }
        /// <summary>
        /// List Organizations Administered by the User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/admin_of_organizations/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindAdministeredByUser(ulong userId,
                                                                                  HttpQuery? query = null)
        {
            var path = $"{User.PATH}/{userId}/admin_of_organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }
        /// <summary>
        /// List Organizations for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/organizations/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindFromUser(ulong userId,
                                                                        HttpQuery? query = null)
        {
            var path = $"{User.PATH}/{userId}/organizations/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        [JsonConverter(typeof(Json.SummaryFieldsOrganizationConverter))]
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public int MaxHosts { get; } = maxHosts;
        public string? CustomVirtualenv { get; } = customVirtualenv;
        public int? DefaultEnvironment { get; } = defaultEnvironment;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(string? searchWords = null, ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", searchWords, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public ExecutionEnvironment[] GetExecutionEnvironments(string? searchWords = null,
                                                               string orderBy = "name",
                                                               ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ExecutionEnvironment>("execution_environments",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ExecutionEnvironment[] GetExecutionEnvironments(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ExecutionEnvironment>("execution_environments", query)];
        }

        /// <summary>
        /// Get the projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Project[] GetProjects(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Project>("projects", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Project[] GetProjects(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Project>("projects", query)];
        }

        /// <summary>
        /// Get the inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Inventory[] GetInventories(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Inventory>("inventories", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Inventory[] GetInventories(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Inventory>("inventories", query)];
        }

        /// <summary>
        /// Get the job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public JobTemplate[] GetJobTemplates(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<JobTemplate>("job_templates", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public JobTemplate[] GetJobTemplates(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<JobTemplate>("job_templates", query)];
        }

        /// <summary>
        /// Get the workflow job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public WorkflowJobTemplate[] GetWorkflowJobTemplates(string? searchWords = null,
                                                             string orderBy = "name",
                                                             ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplate>("workflow_job_templates",
                                                                   searchWords,
                                                                   orderBy,
                                                                   pageSize)];
        }

        /// <summary>
        /// Get the workflow job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public WorkflowJobTemplate[] GetWorkflowJobTemplates(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<WorkflowJobTemplate>("workflow_job_templates", query)];
        }

        /// <summary>
        /// Get the users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] GetUsers(string? searchWords = null, string orderBy = "username", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<User>("users", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] GetUsers(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<User>("users", query)];
        }

        /// <summary>
        /// Get the admin users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/admins/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] GetAdmins(string? searchWords = null, string orderBy = "username", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<User>("admins", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the admin users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/admins/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] GetAdmins(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<User>("admins", query)];
        }

        /// <summary>
        /// Get the teams related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] GetTeams(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Team>("teams", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the teams related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] GetTeams(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Team>("teams", query)];
        }

        /// <summary>
        /// Get the credentials related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] GetCredentials(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Credential>("credentials", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the credentials related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] GetCredentials(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Get the applications related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Application[] GetApplications(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Application>("applications", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the applications related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Application[] GetApplications(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Application>("applications", query)];
        }

        /// <summary>
        /// Get the object roles related to this organization
        /// </summary>
        /// <remarks>
        /// This is almost same as:
        /// <code>thisObject.SummaryFields["ObjectRoles"]</code>
        /// </remarks>
        public OrganizationObjectRoleSummary[] GetObjectRoles()
        {
            return SummaryFields.TryGetValue<Dictionary<string, OrganizationObjectRoleSummary>>("ObjectRoles", out var dict)
                ? [.. dict.Values]
                : [];
        }

        /// <summary>
        /// Get the access list related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] GetAccessList(string? searchWords = null, string orderBy = "username", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<User>("access_list", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the access list related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] GetAccessList(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Get the instance groups related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public InstanceGroup[] GetInstanceGroups(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<InstanceGroup>("instance_groups", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the instance groups related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public InstanceGroup[] GetInstanceGroups(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<InstanceGroup>("instance_groups", query)];
        }

        /// <summary>
        /// Get the notification templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplates(string? searchWords = null,
                                                               string orderBy = "name",
                                                               ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplates(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates", query)];
        }

        /// <summary>
        /// Get the notification templates that have start notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnStarted(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have start notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Get the notification templates that have success notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have success notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Get the notification templates that have error notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnError(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have error notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        /// <summary>
        /// Get the notification templates that have approvals notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnApprovals(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have approvals notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnApprovals(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
