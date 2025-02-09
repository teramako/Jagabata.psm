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

    public class AdHocCommand(ulong id, ResourceType type, string url, RelatedDictionary related,
                              SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                              JobLaunchType launchType, JobStatus status, ulong? executionEnvironment, bool failed,
                              DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                              string jobExplanation, LaunchedBy launchedBy, string? workUnitId, string executionNode,
                              string controllerNode, JobType jobType, ulong inventory, string limit, ulong credential,
                              string moduleName, string moduleArgs, byte forks, JobVerbosity verbosity, string extraVars,
                              bool becomeEnabled, bool diffMode)
        : UnifiedJob(id, type, url, created, modified, name, launchType, status, executionEnvironment, failed,
                     started, finished, canceledOn, elapsed, jobExplanation, launchedBy, workUnitId),
          IAdHocCommand, IResource, ICacheableResource
    {
        public new const string PATH = "/api/v2/ad_hoc_commands/";
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

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                            SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                            JobLaunchType launchType, JobStatus status, ulong? executionEnvironment, bool failed,
                            DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                            string jobExplanation, LaunchedBy launchedBy, string? workUnitId, string executionNode,
                            string controllerNode, JobType jobType, ulong inventory, string limit, ulong credential,
                            string moduleName, string moduleArgs, byte forks, JobVerbosity verbosity, string extraVars,
                            bool becomeEnabled, bool diffMode, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string resultTraceback, bool eventProcessingFinished,
                            Dictionary<string, int> hostStatusCounts)
            : AdHocCommand(id, type, url, related, summaryFields, created, modified, name, launchType, status,
                           executionEnvironment, failed, started, finished, canceledOn, elapsed, jobExplanation,
                           launchedBy, workUnitId, executionNode, controllerNode, jobType, inventory, limit, credential,
                           moduleName, moduleArgs, forks, verbosity, extraVars, becomeEnabled, diffMode),
              IAdHocCommand, IJobDetail, IResource
        {
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public Dictionary<string, int> HostStatusCounts { get; } = hostStatusCounts;
        }
        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public string ExecutionNode { get; } = executionNode;
        public string ControllerNode { get; } = controllerNode;
        public JobType JobType { get; } = jobType;
        public ulong Inventory { get; } = inventory;
        public string Limit { get; } = limit;
        public ulong Credential { get; } = credential;
        public string ModuleName { get; } = moduleName;
        public string ModuleArgs { get; } = moduleArgs;
        public byte Forks { get; } = forks;
        public JobVerbosity Verbosity { get; } = verbosity;
        public string ExtraVars { get; } = extraVars;
        public bool BecomeEnabled { get; } = becomeEnabled;
        public bool DiffMode { get; } = diffMode;

        public Dictionary<string, object?> GetExtraVars()
        {
            return Yaml.DeserializeToDict(ExtraVars);
        }

        public CacheItem GetCacheItem()
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
    }
}
