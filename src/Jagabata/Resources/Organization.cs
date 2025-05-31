using System.Runtime.CompilerServices;
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
        /// Get an Organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Organization ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Organization> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Organization>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        public static Organization Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Organizations
        /// <para>
        /// Implement API: <c>/api/v2/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindAsync(HttpQuery? query = null,
                                                                     [EnumeratorCancellation]
                                                                     CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(PATH, query, ct))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }

        /// <summary>
        /// Find Organizations associated with the User of <paramref name="userId"/>
        /// <para>
        /// Implement API: <c>/api/v2/users/{userId}/organizations/</c>
        /// and <c>/api/v2/users/{userId}/admin_of_organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="query"></param>
        /// <param name="admin">true; Organizations Administered by the User of <paramref name="userId"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Organization> FindAsync(ulong userId,
                                                                     HttpQuery? query = null,
                                                                     bool admin = false,
                                                                     [EnumeratorCancellation]
                                                                     CancellationToken ct = default)
        {
            var path = $"{User.PATH}{userId}/{(admin ? "admin_of_organizations" : "organizations")}/";
            await foreach (var result in RestAPI.GetResultSetAsync<Organization>(path, query, ct))
            {
                foreach (var org in result.Contents.Results)
                {
                    yield return org;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Organization[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Organizations by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Organization[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, bool, CancellationToken)"/>
        public static Organization[] Find(ulong userId, HttpQuery? query = null, bool admin = false)
        {
            return [.. FindAsync(userId, query, admin).ToBlockingEnumerable()];
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
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/organizations/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public ExecutionEnvironment[] FindExecutionEnvironments(string? searchWords = null,
                                                                string orderBy = "name",
                                                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<ExecutionEnvironment>("execution_environments",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find execution environments related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/execution_environments/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ExecutionEnvironment[] FindExecutionEnvironments(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ExecutionEnvironment>("execution_environments", query)];
        }

        /// <summary>
        /// Find projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Project[] FindProjects(string? searchWords = null,
                                      string orderBy = "name",
                                      ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Project>("projects",
                                                        searchWords,
                                                        orderBy,
                                                        pageSize)];
        }

        /// <summary>
        /// Find projects related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Project[] FindProjects(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Project>("projects", query)];
        }

        /// <summary>
        /// Find inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Inventory[] FindInventories(string? searchWords = null,
                                           string orderBy = "name",
                                           ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Inventory>("inventories",
                                                          searchWords,
                                                          orderBy,
                                                          pageSize)];
        }

        /// <summary>
        /// Find inventories related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/inventories/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Inventory[] FindInventories(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Inventory>("inventories", query)];
        }

        /// <summary>
        /// Find job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public JobTemplate[] FindJobTemplates(string? searchWords = null,
                                              string orderBy = "name",
                                              ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<JobTemplate>("job_templates",
                                                            searchWords,
                                                            orderBy,
                                                            pageSize)];
        }

        /// <summary>
        /// Find job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public JobTemplate[] FindJobTemplates(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<JobTemplate>("job_templates", query)];
        }

        /// <summary>
        /// Find workflow job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public WorkflowJobTemplate[] FindWorkflowJobTemplates(string? searchWords = null,
                                                              string orderBy = "name",
                                                              ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplate>("workflow_job_templates",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Find workflow job templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/workflow_job_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public WorkflowJobTemplate[] FindWorkflowJobTemplates(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<WorkflowJobTemplate>("workflow_job_templates", query)];
        }

        /// <summary>
        /// Find users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindUsers(string? searchWords = null,
                                string orderBy = "username",
                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<User>("users",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/users/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindUsers(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("users", query)];
        }

        /// <summary>
        /// Find admin users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/admins/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public User[] FindAdmins(string? searchWords = null,
                                 string orderBy = "username",
                                 ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<User>("admins",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find admin users related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/admins/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAdmins(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("admins", query)];
        }

        /// <summary>
        /// Find teams related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Team[] FindTeams(string? searchWords = null,
                                string orderBy = "name",
                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Team>("teams",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find teams related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] FindTeams(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Team>("teams", query)];
        }

        /// <summary>
        /// Find credentials related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] FindCredentials(string? searchWords = null,
                                            string orderBy = "name",
                                            ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials",
                                                           searchWords,
                                                           orderBy,
                                                           pageSize)];
        }

        /// <summary>
        /// Find credentials related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
        }

        /// <summary>
        /// Find applications related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Application[] FindApplications(string? searchWords = null,
                                              string orderBy = "name",
                                              ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Application>("applications",
                                                            searchWords,
                                                            orderBy,
                                                            pageSize)];
        }

        /// <summary>
        /// Find applications related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/applications/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Application[] FindApplications(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Application>("applications", query)];
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
        /// Find the access list related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/access_list/</c>
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
        /// Find the access list related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAccessList(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Find instance groups related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public InstanceGroup[] FindInstanceGroups(string? searchWords = null,
                                                  string orderBy = "name",
                                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups",
                                                              searchWords,
                                                              orderBy,
                                                              pageSize)];
        }

        /// <summary>
        /// Find instance groups related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/instance_groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public InstanceGroup[] FindInstanceGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<InstanceGroup>("instance_groups", query)];
        }

        /// <summary>
        /// Find notification templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplates(string? searchWords = null,
                                                                string orderBy = "name",
                                                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates related to this organization
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplates(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates", query)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(string? searchWords = null,
                                                                       string orderBy = "name",
                                                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        /// <summary>
        /// Find notification templates that have approvals notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnApprovals(string? searchWords = null,
                                                                           string orderBy = "name",
                                                                           ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have approvals notification enabled for this organization.
        /// <para>
        /// Implement API: <c>/api/v2/organizations/{id}/notification_templates_approvals/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnApprovals(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_approvals", query)];
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
