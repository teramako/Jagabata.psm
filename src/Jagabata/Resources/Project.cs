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
        /// Retrieve a Project.<br/>
        /// API Path: <c>/api/v2/projects/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Project> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Project>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Projects.<br/>
        /// API Path: <c>/api/v2/projects/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<Project> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(PATH, query))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
        }
        /// <summary>
        /// List Projects for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/projects/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Project> FindFromOrganization(ulong organizationId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/projects/";
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(path, query))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
        }
        /// <summary>
        /// List Projects for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/projects/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Project> FindFromUser(ulong userId,
                                                                   HttpQuery? query = null)
        {
            var path = $"{User.PATH}{userId}/projects/";
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(path, query))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
        }
        /// <summary>
        /// List Projects for a Team.<br/>
        /// API Path: <c>/api/v2/teams/<paramref name="teamId"/>/projects/</c>
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Project> FindFromTeam(ulong teamId,
                                                                   HttpQuery? query = null)
        {
            var path = $"{Team.PATH}{teamId}/projects/";
            await foreach (var result in RestAPI.GetResultSetAsync<Project>(path, query))
            {
                foreach (var project in result.Contents.Results)
                {
                    yield return project;
                }
            }
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
        public ProjectUpdateJob[] GetRecentJobs(int count = 20)
        {
            var path = $"{PATH}{Id}/project_updates/?order_by=-id&page_size={count}";
            return [.. RestAPI.GetResultSet<ProjectUpdateJob>(path)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/projects/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public IEnumerable<ActivityStream> GetRecentActivityStream(ushort pageSize = 20)
        {
            return GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize);
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
