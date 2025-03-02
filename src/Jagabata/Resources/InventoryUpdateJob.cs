using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IInventoryUpdateJob : IUnifiedJob
    {
        string Description { get; }
        ulong UnifiedJobTemplate { get; }
        string ControllerNode { get; }
        InventorySourceSource Source { get; }
        string SourcePath { get; }
        /// <summary>
        /// Inventory source variables in YAML or JSON format.
        /// </summary>
        string SourceVars { get; }
        /// <summary>
        /// Inventory source SCM branch.
        /// Project default used if blank. Only allowed if project <c>allow_override</c> field is set to <c>true</c>.
        /// </summary>
        string ScmBranch { get; }
        /// <summary>
        /// Cloud credential to use for inventory updates.
        /// </summary>
        ulong? Credential { get; }
        /// <summary>
        /// Retrieve the enabled state from the given dict of host variables.
        /// The enabled variable may be specified as <c>"foo.bar"</c>, in which case the lookup will traverse into nested dicts,
        /// equivalent to: <c>from_dict.get("foo", {}).get("bar", default)</c>
        /// </summary>
        string EnabledVar { get; }
        /// <summary>
        /// Only used when <c>enabled_var</c> is set.
        /// Value when the host is considered enabled.
        /// For example if
        /// <c>enabled_var</c> = <c>"status.power_state" </c> and
        /// <c>enabled_value</c> =  <c>"powered_on"</c>
        /// with host variables:
        /// <code>
        /// {
        ///     "status": {
        ///         "power_state": "powered_on",
        ///         "created": "2018-02-01T08:00:00.000000Z:00",
        ///         "healthy": true
        ///     },
        ///     "name": "foobar",
        ///     "ip_address": "192.168.2.1"
        /// }
        /// </code>
        /// The host would be marked enabled.
        /// If <c>power_state</c> where any value other then <c>powered_on</c> then the host would be disabled when imprted.
        /// If the key is not found then the host will be enabled.
        /// </summary>
        string EnabledValue { get; }
        /// <summary>
        /// This field is deprecated and will be removed in a future release.
        /// Regex where only matching hosts will be imported.
        /// </summary>
        string HostFilter { get; }
        /// <summary>
        /// Overwrite local groups and hosts from remote inventory source.
        /// </summary>
        bool Overwrite { get; }
        /// <summary>
        /// Overwrite local variables from remote inventory source.
        /// </summary>
        bool OverwriteVars { get; }
        string? CustomVirtualenv { get; }
        /// <summary>
        /// The amount of time (in seconds) to run before the task is canceled.
        /// </summary>
        int Timeout { get; }
        /// <summary>
        /// <list type="bullet">
        /// <item><term>0</term><description>WARNING</description></item>
        /// <item><term>1</term><description>INFO (default)</description></item>
        /// <item><term>2</term><description>DEBUG</description></item>
        /// </list>
        /// </summary>
        int Verbosity { get; }
        /// <summary>
        /// Enter host, group or pettern match.
        /// </summary>
        string Limit { get; }
        ulong Inventory { get; }
        ulong InventorySource { get; }
        bool LicenseError { get; }
        bool OrgHostLimitError { get; }
        /// <summary>
        /// Inventory files from thie Project Update were used for the inventory update.
        /// </summary>
        ulong? SourceProjectUpdate { get; }
        /// <summary>
        /// The Instance group the job was run under.
        /// </summary>
        ulong? InstanceGroup { get; }
        string ScmRevision { get; }
    }

    public abstract class InventoryUpdateJobBase : UnifiedJob, IInventoryUpdateJob, ICacheableResource
    {
        public new const string PATH = "/api/v2/inventory_updates/";

        public abstract string Description { get; }
        public abstract ulong UnifiedJobTemplate { get; }
        public abstract ulong? ExecutionEnvironment { get; }
        public abstract string ExecutionNode { get; }
        public abstract string ControllerNode { get; }
        public abstract LaunchedBy LaunchedBy { get; }
        public abstract InventorySourceSource Source { get; }
        public abstract string SourcePath { get; }
        public abstract string SourceVars { get; }
        public abstract string ScmBranch { get; }
        public abstract ulong? Credential { get; }
        public abstract string EnabledVar { get; }
        public abstract string EnabledValue { get; }
        public abstract string HostFilter { get; }
        public abstract bool Overwrite { get; }
        public abstract bool OverwriteVars { get; }
        public abstract string? CustomVirtualenv { get; }
        public abstract int Timeout { get; }
        public abstract int Verbosity { get; }
        public abstract string Limit { get; }
        public abstract ulong Inventory { get; }
        public abstract ulong InventorySource { get; }
        public abstract bool LicenseError { get; }
        public abstract bool OrgHostLimitError { get; }
        public abstract ulong? SourceProjectUpdate { get; }
        public abstract ulong? InstanceGroup { get; }
        public abstract string ScmRevision { get; }

        public CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Source"] = $"{Source}",
                    ["Status"] = $"{Status}",
                    ["Finished"] = $"{Finished}",
                    ["Elapsed"] = $"{Elapsed}"
                }
            };
        }
    }

    public sealed class InventoryUpdateJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                                           SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                           string name, string description, ulong unifiedJobTemplate,
                                           JobLaunchType launchType, JobStatus status, ulong? executionEnvironment,
                                           string controllerNode, bool failed, DateTime? started, DateTime? finished,
                                           DateTime? canceledOn, double elapsed, string jobExplanation,
                                           string executionNode, LaunchedBy launchedBy, string? workUnitId,
                                           InventorySourceSource source, string sourcePath, string sourceVars,
                                           string scmBranch, ulong? credential, string enabledVar, string enabledValue,
                                           string hostFilter, bool overwrite, bool overwriteVars,
                                           string? customVirtualenv, int timeout, int verbosity, string limit,
                                           ulong inventory, ulong inventorySource, bool licenseError,
                                           bool orgHostLimitError, ulong? sourceProjectUpdate, ulong? instanceGroup,
                                           string scmRevision)
        : InventoryUpdateJobBase
    {
        /// <summary>
        /// Retrieve an Inventory Update.<br/>
        /// API Path: <c>/api/v2/inventory_updates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<Detail> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Detail>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Inventory Updates.<br/>
        /// API Path: <c>/api/v2/inventory_updates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<InventoryUpdateJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<InventoryUpdateJob>(PATH, query, getAll))
            {
                foreach (var inventoryUpdateJob in result.Contents.Results)
                {
                    yield return inventoryUpdateJob;
                }
            }
        }
        /// <summary>
        /// List Inventory Updadates for a Project Update.<br/>
        /// API Path: <c>/api/v2/project_update/<paramref name="projectUpdateId"/>/scm_inventory_updates/</c>
        /// </summary>
        /// <param name="projectUpdateId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventoryUpdateJob> FindFromProjectUpdate(ulong projectUpdateId,
                                                                                       NameValueCollection? query = null,
                                                                                       bool getAll = false)
        {
            var path = $"{ProjectUpdateJobBase.PATH}{projectUpdateId}/scm_inventory_updates/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventoryUpdateJob>(path, query, getAll))
            {
                foreach (var inventoryUpdateJob in result.Contents.Results)
                {
                    yield return inventoryUpdateJob;
                }
            }
        }
        /// <summary>
        /// List Inventory Updadates for an Inventory Source.<br/>
        /// API Path: <c>/api/v2/nventory_sources/<paramref name="inventorySourceId"/>/inventory_updates/</c>
        /// </summary>
        /// <param name="inventorySourceId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventoryUpdateJob> FindFromInventorySource(ulong inventorySourceId,
                                                                                         NameValueCollection? query = null,
                                                                                         bool getAll = false)
        {
            var path = $"{Resources.InventorySource.PATH}{inventorySourceId}/inventory_updates/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventoryUpdateJob>(path, query, getAll))
            {
                foreach (var inventoryUpdateJob in result.Contents.Results)
                {
                    yield return inventoryUpdateJob;
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
        public override string ControllerNode { get; } = controllerNode;
        public override LaunchedBy LaunchedBy { get; } = launchedBy;
        public override string? WorkUnitId { get; } = workUnitId;
        public override InventorySourceSource Source { get; } = source;
        public override string SourcePath { get; } = sourcePath;
        public override string SourceVars { get; } = sourceVars;
        public override string ScmBranch { get; } = scmBranch;
        public override ulong? Credential { get; } = credential;
        public override string EnabledVar { get; } = enabledVar;
        public override string EnabledValue { get; } = enabledValue;
        public override string HostFilter { get; } = hostFilter;
        public override bool Overwrite { get; } = overwrite;
        public override bool OverwriteVars { get; } = overwriteVars;
        public override string? CustomVirtualenv { get; } = customVirtualenv;
        public override int Timeout { get; } = timeout;
        public override int Verbosity { get; } = verbosity;
        public override string Limit { get; } = limit;
        public override ulong Inventory { get; } = inventory;
        public override ulong InventorySource { get; } = inventorySource;
        public override bool LicenseError { get; } = licenseError;
        public override bool OrgHostLimitError { get; } = orgHostLimitError;
        public override ulong? SourceProjectUpdate { get; } = sourceProjectUpdate;
        public override ulong? InstanceGroup { get; } = instanceGroup;
        public override string ScmRevision { get; } = scmRevision;

        public sealed class Detail(ulong id, ResourceType type, string url, RelatedDictionary related,
                                   SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                   string name, string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                   JobStatus status, ulong? executionEnvironment, string controllerNode, bool failed,
                                   DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                                   string jobArgs, string jobCwd, Dictionary<string, string> jobEnv,
                                   string jobExplanation, string executionNode, LaunchedBy launchedBy,
                                   string resultTraceback, bool eventProcessingFinished, string? workUnitId,
                                   InventorySourceSource source, string sourcePath, string sourceVars, string scmBranch,
                                   ulong? credential, string enabledVar, string enabledValue, string hostFilter,
                                   bool overwrite, bool overwriteVars, string? customVirtualenv, int timeout,
                                   int verbosity, string limit, ulong inventory, ulong inventorySource,
                                   bool licenseError, bool orgHostLimitError, ulong? sourceProjectUpdate,
                                   ulong? instanceGroup, string scmRevision, ulong? sourceProject)
            : InventoryUpdateJobBase, IJobDetail
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
            public override string ControllerNode { get; } = controllerNode;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public override LaunchedBy LaunchedBy { get; } = launchedBy;
            public override string? WorkUnitId { get; } = workUnitId;
            public override InventorySourceSource Source { get; } = source;
            public override string SourcePath { get; } = sourcePath;
            public override string SourceVars { get; } = sourceVars;
            public override string ScmBranch { get; } = scmBranch;
            public override ulong? Credential { get; } = credential;
            public override string EnabledVar { get; } = enabledVar;
            public override string EnabledValue { get; } = enabledValue;
            public override string HostFilter { get; } = hostFilter;
            public override bool Overwrite { get; } = overwrite;
            public override bool OverwriteVars { get; } = overwriteVars;
            public override string? CustomVirtualenv { get; } = customVirtualenv;
            public override int Timeout { get; } = timeout;
            public override int Verbosity { get; } = verbosity;
            public override string Limit { get; } = limit;
            public override ulong Inventory { get; } = inventory;
            public override ulong InventorySource { get; } = inventorySource;
            public override bool LicenseError { get; } = licenseError;
            public override bool OrgHostLimitError { get; } = orgHostLimitError;
            public override ulong? SourceProjectUpdate { get; } = sourceProjectUpdate;
            public override ulong? InstanceGroup { get; } = instanceGroup;
            public override string ScmRevision { get; } = scmRevision;
            public ulong? SourceProject { get; } = sourceProject;
        }
    }

    public record CanUpdateInventorySource(ulong? InventorySource, bool CanUpdate);
}
