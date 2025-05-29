using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Get log of this job.
        /// This is same as <see cref="ResultStdout"/> Property
        /// </summary>
        /// <param name="vt100Color">This parameter is ignored</param>
        public override string GetJobLog(bool vt100Color = false)
        {
            return ResultStdout;
        }

        /// <summary>
        /// Get system job events for this job
        /// </summary>
        public SystemJobEvent[] GetEvents()
        {
            return [.. GetEvents<SystemJobEvent>()];
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
        /// Get a System Job
        /// <para>
        /// Implement API: <c>/api/v2/system_jobs/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">System Job ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async Task<Detail> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new Detail Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find System Jobs
        /// <para>
        /// Implement API: <c>/api/v2/system_jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<SystemJob> FindAsync(HttpQuery? query = null,
                                                                      [EnumeratorCancellation]
                                                                      CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJob>(PATH, query, ct))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }

        /// <summary>
        /// Find System Jobs for a System Job Template.<br/>
        /// <para>
        /// Implement API: <c>/api/v2/system_job_templates/<paramref name="id"/>/jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="id">System Job Template ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<SystemJob> FindAsync(ulong id,
                                                                  HttpQuery? query = null,
                                                                  [EnumeratorCancellation]
                                                                  CancellationToken ct = default)
        {
            var path = $"{Resources.SystemJobTemplate.PATH}{id}/jobs/";
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJob>(path, query, ct))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }

        /// <summary>
        /// Find System Jobs by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/system_jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new SystemJob[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new SystemJob[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        public static SystemJob[] Find(ulong id, HttpQuery? query = null)
        {
            return [.. FindAsync(id, query).ToBlockingEnumerable()];
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

