using System.Collections.Specialized;

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
        public IEnumerable<AdHocCommandJobEvent> GetEvents()
        {
            return GetEvents<AdHocCommandJobEvent>();
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
        /// Retrieve an Ad Hoc Command.<br/>
        /// API Path: <c>/api/v2/ad_hoc_commands/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<Detail> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Ad Hoc Commands.<br/>
        /// API Path: <c>/api/v2/ad_hoc_commands/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<AdHocCommand> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommand>(PATH, query, getAll))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }
        /// <summary>
        /// List Ad Hoc Commands for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/ad_hoc_commands/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommand> FindFromInventory(ulong inventoryId,
                                                                             NameValueCollection? query = null,
                                                                             bool getAll = false)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/ad_hoc_commands/";
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommand>(path, query, getAll))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }
        /// <summary>
        /// List Ad Hoc Commands for a Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/ad_hoc_commands/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommand> FindFromGroup(ulong groupId,
                                                                         NameValueCollection? query = null,
                                                                         bool getAll = false)
        {
            var path = $"{Group.PATH}{groupId}/ad_hoc_commands/";
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommand>(path, query, getAll))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
                }
            }
        }
        /// <summary>
        /// List Ad Hoc Commands for a Host.
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/ad_hoc_commands/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<AdHocCommand> FindFromHost(ulong hostId,
                                                                        NameValueCollection? query = null,
                                                                        bool getAll = false)
        {
            var path = $"{Host.PATH}{hostId}/ad_hoc_commands/";
            await foreach (var result in RestAPI.GetResultSetAsync<AdHocCommand>(path, query, getAll))
            {
                foreach (var job in result.Contents.Results)
                {
                    yield return job;
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
