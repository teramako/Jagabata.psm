using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface ISystemJob : IUnifiedJob
    {
        string Description { get; }
        ulong UnifiedJobTemplate { get; }
        string ExecutionNode { get; }

        ulong SystemJobTemplate { get; }
        string JobType { get; }
        string ExtraVars { get; }
        string ResultStdout { get; }

        /// <summary>
        /// Deseriaze string <see cref="ExtraVars">ExtraVars</see>(JSON or YAML) to Dictionary
        /// </summary>
        /// <returns>result of deserialized <see cref="ExtraVars"/> to Dictionary</returns>
        Dictionary<string, object?> GetExtraVars();
    }

    public abstract class SystemJobBase : UnifiedJob, ISystemJob
    {
        public new const string PATH = "/api/v2/system_jobs/";

        public abstract string Description { get; }
        public abstract ulong UnifiedJobTemplate { get; }
        public abstract ulong? ExecutionEnvironment { get; }
        public abstract string ExecutionNode { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract ulong SystemJobTemplate { get; }
        public abstract string JobType { get; }
        public abstract string ExtraVars { get; }
        public abstract string ResultStdout { get; }

        public SystemJobTemplate? GetTemplate()
        {
            return GetTemplate<SystemJobTemplate>();
        }

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                    ["Finished"] = $"{Finished}",
                    ["Elapsed"] = $"{Elapsed}"
                }
            };
        }
    }

    public class SystemJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                           SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                           string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                           ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                           DateTime? canceledOn, double elapsed, string jobExplanation, string executionNode,
                           LaunchedBy launchedBy, string? workUnitId, ulong systemJobTemplate, string jobType,
                           string extraVars, string resultStdout)
        : SystemJobBase
    {
        /// <summary>
        /// Retrieve a System Job Template.<br/>
        /// API Path: <c>/api/v2/system_job_templates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<Detail> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List System Job Templates.<br/>
        /// API Path: <c>/api/v2/system_job_templates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<SystemJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJob>(PATH, query, getAll))
            {
                foreach (var systemJob in result.Contents.Results)
                {
                    yield return systemJob;
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
        public override ulong SystemJobTemplate { get; } = systemJobTemplate;
        public override string JobType { get; } = jobType;
        public override string ExtraVars { get; } = extraVars;
        public override string ResultStdout { get; } = resultStdout;

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            string description, ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                            ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                            DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string jobExplanation, string executionNode,
                            string resultTraceback, bool eventProcessingFinished, LaunchedBy launchedBy,
                            string? workUnitId, ulong systemJobTemplate, string jobType, string extraVars,
                            string resultStdout)
            : SystemJobBase, IJobDetail
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
            public override ulong SystemJobTemplate { get; } = systemJobTemplate;
            public override string JobType { get; } = jobType;
            public override string ExtraVars { get; } = extraVars;
            public override string ResultStdout { get; } = resultStdout;
        }
    }
}

