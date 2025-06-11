using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [Flags]
    public enum ProjectOptions
    {
        None = 0,
        ScmClean = 1 << 0,
        ScmDeleteOnUpdate = 1 << 1,
        ScmTrackSubmodules = 1 << 2,
        ScmUpdateOnLaunch = 1 << 3,
        AllowOverride = 1 << 4
    }

    public interface IProject
    {
        /// <summary>
        /// Name of this project.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this project.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Local path (relative <c>PROJECT_ROOT</c>) containing playbooks and related files for this project.
        /// </summary>
        string LocalPath { get; }
        /// <summary>
        /// Specifies the source control system used to store the project.
        /// </summary>
        string ScmType { get; }
        /// <summary>
        /// The location where the project is stored.
        /// </summary>
        string ScmUrl { get; }
        /// <summary>
        /// Specific branch, tag or commit to checkout.
        /// </summary>
        string ScmBranch { get; }
        /// <summary>
        /// For git projects, an additional refspec to fetch.
        /// </summary>
        string ScmRefspec { get; }
        /// <summary>
        /// Discard any local changes before syncing the project.
        /// </summary>
        bool ScmClean { get; }
        /// <summary>
        /// Track submodules latest commits on defined branch.
        /// </summary>
        bool ScmTrackSubmodules { get; }
        /// <summary>
        /// Delete the project before syncing.
        /// </summary>
        bool ScmDeleteOnUpdate { get; }
        ulong? Credential { get; }
        /// <summary>
        /// The amount of time (in seconds) to run before the task is canceled.
        /// </summary>
        int Timeout { get; }
        /// <summary>
        /// The organization used to determine access to this template.
        /// </summary>
        ulong Organization { get; }
        /// <summary>
        /// Update the project when a job is launched that used the project.
        /// </summary>
        bool ScmUpdateOnLaunch { get; }
        /// <summary>
        /// The number of seconds after the last project update ran that
        /// a new project update will be launched as a job dependency.
        /// </summary>
        int ScmUpdateCacheTimeout { get; }
        /// <summary>
        /// Allow changing the SCM branch or revision in a job template that uses this project.
        /// </summary>
        bool AllowOverride { get; }
        /// <summary>
        /// The default execution environment for jobs run using this project.
        /// </summary>
        ulong? DefaultEnvironment { get; }
        /// <summary>
        /// An optional credential used for validating files in the project against unexpected changes.
        /// </summary>
        ulong? SignatureValidationCredential { get; }
    }

    public class Project(ulong id, ResourceType type, string url, RelatedDictionary related,
                         SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                         string description, string localPath, string scmType, string scmUrl, string scmBranch,
                         string scmRefspec, bool scmClean, bool scmTrackSubmodules, bool scmDeleteOnUpdate,
                         ulong? credential, int timeout, string scmRevision, DateTime? lastJobRun, bool lastJobFailed,
                         DateTime? nextJobRun, JobTemplateStatus status, ulong organization, bool scmUpdateOnLaunch,
                         int scmUpdateCacheTimeout, bool allowOverride, string? customVirtualenv,
                         ulong? defaultEnvironment, ulong? signatureValidationCredential, bool lastUpdateFailed,
                         DateTime? lastUpdated)
        : UnifiedJobTemplate, IProject
    {
        public new const string PATH = "/api/v2/projects/";

        /// <summary>
        /// Get a Project
        /// <para>
        /// Implement API: <c>/api/v2/projects/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<Project> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Project>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new Project Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Projects
        /// <para>
        /// Implement API: <c>/api/v2/projects/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<Project> FindAsync(HttpQuery? query = null,
                                                                    [EnumeratorCancellation]
                                                                    CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(PATH, query, ct))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
        }

        /// <summary>
        /// Find Projects associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/{Type}/{Id}/projects/</c>
        /// <para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Organization</item>
        ///     <item>User</item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Project> FindAsync(IResource resource,
                                                                HttpQuery? query = null,
                                                                [EnumeratorCancellation]
                                                                CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Organization => $"{Resources.Organization.PATH}{resource.Id}/projects/",
                ResourceType.User => $"{User.PATH}{resource.Id}/projects/",
                ResourceType.Team => $"{Team.PATH}{resource.Id}/projects/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(path, query, ct))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new Project[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Labels by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new Project[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Project[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Projects associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static Project[] Find(IResource resource,
                                     string? searchWords = null,
                                     string orderBy = "name",
                                     ushort pageSize = 20,
                                     uint startPage = 1)
        {
            return Find(resource, new QueryBuilder().SetSearchWords(searchWords)
                                                    .SetOrderBy(orderBy)
                                                    .SetPageSize(pageSize)
                                                    .SetStartPage(startPage)
                                                    .Build());
        }

        /// <summary>
        /// Get inventory files and directories.
        /// </summary>
        /// <returns>Array of inventory files and directories available with in the project, not comprehensive</returns>
        public static async Task<string[]> GetInventoryFiles(ulong projectId)
        {
            var apiResult = await RestAPI.GetAsync<string[]>($"{PATH}{projectId}/inventories/");
            return apiResult.Contents;
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public override string Name { get; } = name;
        public override string Description { get; } = description;
        public string LocalPath { get; } = localPath;
        public string ScmType { get; } = scmType;
        public string ScmUrl { get; } = scmUrl;
        public string ScmBranch { get; } = scmBranch;
        public string ScmRefspec { get; } = scmRefspec;
        public bool ScmClean { get; } = scmClean;
        public bool ScmTrackSubmodules { get; } = scmTrackSubmodules;
        public bool ScmDeleteOnUpdate { get; } = scmDeleteOnUpdate;
        public ulong? Credential { get; } = credential;
        public int Timeout { get; } = timeout;
        public string ScmRevision { get; } = scmRevision;
        public override DateTime? LastJobRun { get; } = lastJobRun;
        public override bool LastJobFailed { get; } = lastJobFailed;
        public override DateTime? NextJobRun { get; } = nextJobRun;
        public override JobTemplateStatus Status { get; } = status;
        public ulong Organization { get; } = organization;
        public bool ScmUpdateOnLaunch { get; } = scmUpdateOnLaunch;
        public int ScmUpdateCacheTimeout { get; } = scmUpdateCacheTimeout;
        public bool AllowOverride { get; } = allowOverride;
        public string? CustomVirtualenv { get; } = customVirtualenv;
        public ulong? DefaultEnvironment { get; } = defaultEnvironment;
        public ulong? SignatureValidationCredential { get; } = signatureValidationCredential;
        public bool LastUpdateFailed { get; } = lastUpdateFailed;
        public DateTime? LastUpdated { get; } = lastUpdated;

        [JsonIgnore]
        public ProjectOptions Options => (ScmClean ? ProjectOptions.ScmClean : 0) |
                       (ScmDeleteOnUpdate ? ProjectOptions.ScmDeleteOnUpdate : 0) |
                       (ScmTrackSubmodules ? ProjectOptions.ScmTrackSubmodules : 0) |
                       (ScmUpdateOnLaunch ? ProjectOptions.ScmUpdateOnLaunch : 0) |
                       (AllowOverride ? ProjectOptions.AllowOverride : 0);

        /// <summary>
        /// Get inventory files and directories available within this project, not comprehensive.
        /// </summary>
        /// <returns>Array of inventory files and directories</returns>
        public string[] GetInventoryFiles()
        {
            return RestAPI.Get<string[]>($"{PATH}{Id}/inventories/");
        }

        /// <summary>
        /// Get playbooks available within this project.
        /// </summary>
        /// <returns>Array of playbooks</returns>
        public string[] GetPlaybooks()
        {
            return RestAPI.Get<string[]>($"{PATH}{Id}/playbooks/");
        }

        /// <summary>
        /// Get the most recently executed jobs.
        /// Implement API: <c>/api/v2/projects/{id}/project_updates/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public ProjectUpdateJob[] GetRecentJobs(ushort count = 20)
        {
            return [.. FindResultsByRelatedKey<ProjectUpdateJob>("project_updates", null, "-id", count)];
        }

        /// <summary>
        /// Find the  activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/projects/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Find teams related to this project
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/teams/</c>
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
        /// Find teams related to this project
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/teams/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Team[] FindTeams(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Team>("teams", query)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_started/</c>
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
        /// Find notification templates that have start notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_success/</c>
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
        /// Find notification templates that have success notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_error/</c>
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
        /// Find notification templates that have error notification enabled for this project.
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        /// <summary>
        /// Find the access list related to this project
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/access_list/</c>
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
        /// Find the access list related to this project
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/access_list/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public User[] FindAccessList(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<User>("access_list", query)];
        }

        /// <summary>
        /// Get the object roles related to this project
        /// </summary>
        /// <remarks>
        /// This is almost same as:
        /// <code>thisObject.SummaryFields["ObjectRoles"]</code>
        /// </remarks>
        public ObjectRoleSummary[] GetObjectRoles()
        {
            return SummaryFields.TryGetValue<Dictionary<string, ObjectRoleSummary>>("ObjectRoles", out var dict)
                ? [.. dict.Values]
                : [];
        }

        /// <summary>
        /// Get the organization related this project
        /// </summary>
        public Organization? GetOrganization()
        {
            return Related.TryGetPath("organization", out var path)
                ? RestAPI.Get<Organization>(path)
                : null;
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["ScmType"] = string.IsNullOrEmpty(ScmType) ? "Local" : ScmType,
                    ["Status"] = $"{Status}",
                }
            };
        }
    }
}
