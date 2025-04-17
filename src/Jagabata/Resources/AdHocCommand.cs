using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface IAdHocCommand : IUnifiedJob
    {
        string ExecutionNode { get; }
        string ControllerNode { get; }

        JobType JobType { get; }
        ulong Inventory { get; }
        string Limit { get; }
        ulong Credential { get; }
        string ModuleName { get; }
        string ModuleArgs { get; }
        byte Forks { get; }
        JobVerbosity Verbosity { get; }
        string ExtraVars { get; }
        bool BecomeEnabled { get; }
        bool DiffMode { get; }

        /// <summary>
        /// Deseriaze string <see cref="ExtraVars">ExtraVars</see>(JSON or YAML) to Dictionary
        /// </summary>
        /// <returns>result of deserialized <see cref="ExtraVars"/> to Dictionary</returns>
        Dictionary<string, object?> GetExtraVars();
    }

    public abstract class AdHocCommandBase : UnifiedJob, IAdHocCommand
    {
        public new const string PATH = "/api/v2/ad_hoc_commands/";

        public abstract ulong? ExecutionEnvironment { get; }
        public abstract string ExecutionNode { get; }
        public abstract string ControllerNode { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract JobType JobType { get; }
        public abstract ulong Inventory { get; }
        public abstract string Limit { get; }
        public abstract ulong Credential { get; }
        public abstract string ModuleName { get; }
        public abstract string ModuleArgs { get; }
        public abstract byte Forks { get; }
        public abstract JobVerbosity Verbosity { get; }
        public abstract string ExtraVars { get; }
        public abstract bool BecomeEnabled { get; }
        public abstract bool DiffMode { get; }

        /// <summary>
        /// Get adhoc command events for this job
        /// </summary>
        public AdHocCommandJobEvent[] GetEvents()
        {
            return [.. GetEvents<AdHocCommandJobEvent>()];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/ad_hoc_commands/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, ModuleArgs)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                    ["Limit"] = Limit,
                    ["Finished"] = $"{Finished}",
                    ["Elapsed"] = $"{Elapsed}"
                }
            };
        }

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }
    }

    public sealed class AdHocCommand(ulong id, ResourceType type, string url, RelatedDictionary related,
                                     SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                     string name, JobLaunchType launchType, JobStatus status,
                                     ulong? executionEnvironment, bool failed, DateTime? started, DateTime? finished,
                                     DateTime? canceledOn, double elapsed, string jobExplanation, LaunchedBy launchedBy,
                                     string? workUnitId, string executionNode, string controllerNode, JobType jobType,
                                     ulong inventory, string limit, ulong credential, string moduleName,
                                     string moduleArgs, byte forks, JobVerbosity verbosity, string extraVars,
                                     bool becomeEnabled, bool diffMode)
        : AdHocCommandBase
    {
        /// <summary>
        /// Get an Ad Hoc Command
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static new async Task<Detail> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <summary>
        /// Get an AdhocCommand job
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new Detail Get(ulong id)
        {
            var task = GetAsync(id);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Find AdHocCommand jobs
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<AdHocCommand> FindAsync(HttpQuery? query = null,
                                                                         [EnumeratorCancellation]
                                                                         CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommand>(PATH, query, ct))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }

        /// <summary>
        /// Find AdHocCommand jobs associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/ad_hoc_commands/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Inventory</item>
        ///     <item>Group</item>
        ///     <item>Host</item>
        /// </list>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommand> FindAsync(IResource resource,
                                                                     HttpQuery? query = null,
                                                                     [EnumeratorCancellation]
                                                                     CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Inventory => $"{Resources.Inventory.PATH}{resource.Id}/ad_hoc_commands/",
                ResourceType.Group => $"{Group.PATH}{resource.Id}/ad_hoc_commands/",
                ResourceType.Host => $"{Host.PATH}{resource.Id}/ad_hoc_commands/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var apiResult in RestAPI.GetResultSetAsync<AdHocCommand>(path, query, ct))
            {
                foreach (var activity in apiResult.Contents.Results)
                {
                    yield return activity;
                }
            }
        }

        /// <summary>
        /// Find AdHocCommand jobs
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        public static new AdHocCommand[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find AdHocCommand jobs by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new AdHocCommand[] Find(string? searchWords = null,
                                              string orderBy = "-id",
                                              ushort pageSize = 20,
                                              uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }

        /// <summary>
        /// Find AdHocCommand jobs associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="query"></param>
        public static AdHocCommand[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find AdHocCommand jobs associated with <paramref name="resource"/> by basic parammeters
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/ad_hoc_commands/</c>
        /// </para>
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static AdHocCommand[] Find(IResource resource,
                                          string? searchWords = null,
                                          string orderBy = "-timestamp",
                                          ushort pageSize = 20,
                                          uint startPage = 1)
        {
            return Find(resource, new QueryBuilder().SetSearchWords(searchWords)
                                                    .SetOrderBy(orderBy)
                                                    .SetPageSize(pageSize)
                                                    .SetStartPage(startPage)
                                                    .Build());
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public override DateTime Created { get; } = created;
        public override DateTime? Modified { get; } = modified;
        public override string Name { get; } = name;
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
        public override string ControllerNode { get; } = controllerNode;
        public override LaunchedBy LaunchedBy { get; } = launchedBy;
        public override string? WorkUnitId { get; } = workUnitId;
        public override JobType JobType { get; } = jobType;
        public override ulong Inventory { get; } = inventory;
        public override string Limit { get; } = limit;
        public override ulong Credential { get; } = credential;
        public override string ModuleName { get; } = moduleName;
        public override string ModuleArgs { get; } = moduleArgs;
        public override byte Forks { get; } = forks;
        public override JobVerbosity Verbosity { get; } = verbosity;
        public override string ExtraVars { get; } = extraVars;
        public override bool BecomeEnabled { get; } = becomeEnabled;
        public override bool DiffMode { get; } = diffMode;

        public sealed class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                                   SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                   string name, JobLaunchType launchType, JobStatus status, ulong? executionEnvironment,
                                   bool failed, DateTime? started, DateTime? finished, DateTime? canceledOn,
                                   double elapsed, string jobExplanation, LaunchedBy launchedBy, string? workUnitId,
                                   string executionNode, string controllerNode, JobType jobType, ulong inventory,
                                   string limit, ulong credential, string moduleName, string moduleArgs, byte forks,
                                   JobVerbosity verbosity, string extraVars, bool becomeEnabled, bool diffMode,
                                   string jobArgs, string jobCwd, Dictionary<string, string> jobEnv,
                                   string resultTraceback, bool eventProcessingFinished,
                                   Dictionary<string, int> hostStatusCounts)
            : AdHocCommandBase, IJobDetail
        {
            public override ulong Id { get; } = id;
            public override ResourceType Type { get; } = type;
            public override string Url { get; } = url;
            public override RelatedDictionary Related { get; } = related;
            public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
            public override DateTime Created { get; } = created;
            public override DateTime? Modified { get; } = modified;
            public override string Name { get; } = name;
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
            public override string ControllerNode { get; } = controllerNode;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override JobType JobType { get; } = jobType;
            public override ulong Inventory { get; } = inventory;
            public override string Limit { get; } = limit;
            public override ulong Credential { get; } = credential;
            public override string ModuleName { get; } = moduleName;
            public override string ModuleArgs { get; } = moduleArgs;
            public override byte Forks { get; } = forks;
            public override JobVerbosity Verbosity { get; } = verbosity;
            public override string ExtraVars { get; } = extraVars;
            public override bool BecomeEnabled { get; } = becomeEnabled;
            public override bool DiffMode { get; } = diffMode;
            public Dictionary<string, int> HostStatusCounts { get; } = hostStatusCounts;
        }
    }
}
