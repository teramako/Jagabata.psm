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

    public class ProjectUpdateJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                                  ProjectUpdateJob.Summary summaryFields, DateTime created, DateTime? modified,
                                  string name, string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                  JobStatus status, ulong? executionEnvironment, bool failed, DateTime? started,
                                  DateTime? finished, DateTime? canceledOn, double elapsed, string jobExplanation,
                                  string executionNode, LaunchedBy launchedBy, string workUnitId, string localPath,
                                  string scmType, string scmUrl, string scmBranch, string scmRefspec, bool scmClean,
                                  bool scmTrackSubmodules, bool scmDeleteOnUpdate, ulong? credential, int timeout,
                                  string scmRevision, ulong project, JobType jobType, string jobTags)
        : UnifiedJob(id, type, url, created, modified, name, launchType, status, executionEnvironment, failed,
                     started, finished, canceledOn, elapsed, jobExplanation, launchedBy, workUnitId),
          IProjectUpdateJob, IResource<ProjectUpdateJob.Summary>
    {
        public new const string PATH = "/api/v2/project_updates/";

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

        public record Summary(OrganizationSummary Organization,
                              EnvironmentSummary? DefaultEnvironment,
                              ProjectSummary Project,
                              CredentialSummary? Credential,
                              ScheduleSummary? Schedule,
                              UnifiedJobTemplateSummary UnifiedJobTemplate,
                              InstanceGroupSummary InstanceGroup,
                              Capability UserCapabilities);


        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            Summary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, string localPath, string scmType, string scmUrl, string scmBranch,
                            string scmRefspec, bool scmClean, bool scmTrackSubmodules, bool scmDeleteOnUpdate,
                            ulong? credential, int timeout, string scmRevision, ulong unifiedJobTemplate,
                            JobLaunchType launchType, JobStatus status, ulong? executionEnvironment, bool failed,
                            DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs,
                            string jobCwd, Dictionary<string, string> jobEnv, string jobExplanation,
                            string executionNode, string resultTraceback, bool eventProcessingFinished,
                            LaunchedBy launchedBy, string workUnitId, ulong project, JobType jobType, string jobTags,
                            Dictionary<string, int> hostStatusCounts, Dictionary<string, int> playbookCounts)
            : ProjectUpdateJob(id, type, url, related, summaryFields, created, modified, name, description,
                               unifiedJobTemplate, launchType, status, executionEnvironment, failed, started,
                               finished, canceledOn, elapsed, jobExplanation, executionNode, launchedBy, workUnitId,
                               localPath, scmType, scmUrl, scmBranch, scmRefspec, scmClean, scmTrackSubmodules,
                               scmDeleteOnUpdate, credential, timeout, scmRevision, project, jobType, jobTags),
              IProjectUpdateJob, IJobDetail
        {
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;

            public Dictionary<string, int> HostStatusCounts { get; } = hostStatusCounts;
            public Dictionary<string, int> PlaybookCounts { get; } = playbookCounts;
        }


        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;

        public string Description { get; } = description;
        public ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public string ExecutionNode { get; } = executionNode;

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
        public ulong Project { get; } = project;
        public JobType JobType { get; } = jobType;
        public string JobTags { get; } = jobTags;
    }

    public record CanUpdateProject(ulong? Project, bool CanUpdate);
}
