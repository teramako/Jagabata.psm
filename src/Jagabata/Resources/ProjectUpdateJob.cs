using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IProjectUpdateJob : IUnifiedJob
    {
        string Description { get; }
        ulong UnifiedJobTemplate { get; }
        string ExecutionNode { get; }

        string LocalPath { get; }
        string ScmType { get; }
        string ScmUrl { get; }
        string ScmBranch { get; }
        string ScmRefspec { get; }
        bool ScmClean { get; }
        bool ScmTrackSubmodules { get; }
        bool ScmDeleteOnUpdate { get; }
        ulong? Credential { get; }
        int Timeout { get; }
        string ScmRevision { get; }
        ulong Project { get; }
        JobType JobType { get; }
        string JobTags { get; }
    }

    public abstract class ProjectUpdateJobBase : UnifiedJob, IProjectUpdateJob, ICacheableResource
    {
        public new const string PATH = "/api/v2/project_updates/";

        public abstract string Description { get; }
        public abstract ulong UnifiedJobTemplate { get; }
        public abstract ulong? ExecutionEnvironment { get; }
        public abstract string ExecutionNode { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract string LocalPath { get; }
        public abstract string ScmType { get; }
        public abstract string ScmUrl { get; }
        public abstract string ScmBranch { get; }
        public abstract string ScmRefspec { get; }
        public abstract bool ScmClean { get; }
        public abstract bool ScmTrackSubmodules { get; }
        public abstract bool ScmDeleteOnUpdate { get; }
        public abstract ulong? Credential { get; }
        public abstract int Timeout { get; }
        public abstract string ScmRevision { get; }
        public abstract ulong Project { get; }
        public abstract JobType JobType { get; }
        public abstract string JobTags { get; }

        public CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                    ["Finished"] = $"{Finished}",
                    ["Elapsed"] = $"{Elapsed}",
                }
            };
        }
    }

    public class ProjectUpdateJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                                  SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                  string name, string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                                  DateTime? finished, DateTime? canceledOn, double elapsed, string jobExplanation,
                                  string executionNode, LaunchedBy launchedBy, string workUnitId, string localPath,
                                  string scmType, string scmUrl, string scmBranch, string scmRefspec, bool scmClean,
                                  bool scmTrackSubmodules, bool scmDeleteOnUpdate, ulong? credential, int timeout,
                                  string scmRevision, ulong project, JobType jobType, string jobTags)
        : ProjectUpdateJobBase
    {
        /// <summary>
        /// Retrieve a Project Update.<br/>
        /// API Path: <c>/api/v2/project_updates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<Detail> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Project Updages.<br/>
        /// API Path: <c>/api/v2/project_updates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<ProjectUpdateJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ProjectUpdateJob>(PATH, query, getAll))
            {
                foreach (var projectUpdateJob in result.Contents.Results)
                {
                    yield return projectUpdateJob;
                }
            }
        }
        /// <summary>
        /// List Project Updates for a Project.<br/>
        /// API Path: <c>/api/v2/projects/<paramref name="projectId"/>/project_updates/</c>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ProjectUpdateJob> FindFromProject(ulong projectId,
                                                                               NameValueCollection? query = null,
                                                                               bool getAll = false)
        {
            var path = $"{Resources.Project.PATH}{projectId}/project_updates/";
            await foreach (var result in RestAPI.GetResultSetAsync<ProjectUpdateJob>(path, query, getAll))
            {
                foreach (var projectUpdateJob in result.Contents.Results)
                {
                    yield return projectUpdateJob;
                }
            }
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
        public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public override JobLaunchType LaunchType { get; } = launchType;
        public override JobStatus Status { get; } = status;
        public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public override bool Failed { get; } = failed;
        public override DateTime? Started { get; } = started;
        public override DateTime? Finished { get; } = finished;
        public override DateTime? CanceledOn { get; } = canceledOn;
        public override double Elapsed { get; } = elapsed;
        public override string JobExplanation { get; } = jobExplanation;
        public override string ExecutionNode { get; } = executionNode;
        public override LaunchedBy LaunchedBy { get; } = launchedBy;
        public override string? WorkUnitId { get; } = workUnitId;
        public override string LocalPath { get; } = localPath;
        public override string ScmType { get; } = scmType;
        public override string ScmUrl { get; } = scmUrl;
        public override string ScmBranch { get; } = scmBranch;
        public override string ScmRefspec { get; } = scmRefspec;
        public override bool ScmClean { get; } = scmClean;
        public override bool ScmTrackSubmodules { get; } = scmTrackSubmodules;
        public override bool ScmDeleteOnUpdate { get; } = scmDeleteOnUpdate;
        public override ulong? Credential { get; } = credential;
        public override int Timeout { get; } = timeout;
        public override string ScmRevision { get; } = scmRevision;
        public override ulong Project { get; } = project;
        public override JobType JobType { get; } = jobType;
        public override string JobTags { get; } = jobTags;

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, string localPath, string scmType, string scmUrl, string scmBranch,
                            string scmRefspec, bool scmClean, bool scmTrackSubmodules, bool scmDeleteOnUpdate,
                            ulong? credential, int timeout, string scmRevision, ulong unifiedJobTemplate,
                            JobLaunchType launchType, JobStatus status, ulong? executionEnvironment, bool failed,
                            DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs,
                            string jobCwd, Dictionary<string, string> jobEnv, string jobExplanation,
                            string executionNode, string resultTraceback, bool eventProcessingFinished,
                            LaunchedBy launchedBy, string workUnitId, ulong project, JobType jobType, string jobTags,
                            Dictionary<string, int> hostStatusCounts, Dictionary<string, int> playbookCounts)
            : ProjectUpdateJobBase, IJobDetail
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
            public override string Description { get; } = description;
            public override ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
            public override JobLaunchType LaunchType { get; } = launchType;
            public override JobStatus Status { get; } = status;
            public override ulong? ExecutionEnvironment { get; } = executionEnvironment;
            public override bool Failed { get; } = failed;
            public override DateTime? Started { get; } = started;
            public override DateTime? Finished { get; } = finished;
            public override DateTime? CanceledOn { get; } = canceledOn;
            public override double Elapsed { get; } = elapsed;
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public override string JobExplanation { get; } = jobExplanation;
            public override string ExecutionNode { get; } = executionNode;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override string LocalPath { get; } = localPath;
            public override string ScmType { get; } = scmType;
            public override string ScmUrl { get; } = scmUrl;
            public override string ScmBranch { get; } = scmBranch;
            public override string ScmRefspec { get; } = scmRefspec;
            public override bool ScmClean { get; } = scmClean;
            public override bool ScmTrackSubmodules { get; } = scmTrackSubmodules;
            public override bool ScmDeleteOnUpdate { get; } = scmDeleteOnUpdate;
            public override ulong? Credential { get; } = credential;
            public override int Timeout { get; } = timeout;
            public override string ScmRevision { get; } = scmRevision;
            public override ulong Project { get; } = project;
            public override JobType JobType { get; } = jobType;
            public override string JobTags { get; } = jobTags;
            public Dictionary<string, int> HostStatusCounts { get; } = hostStatusCounts;
            public Dictionary<string, int> PlaybookCounts { get; } = playbookCounts;
        }
    }

    public record CanUpdateProject(ulong? Project, bool CanUpdate);
}
