using System.Collections.Specialized;
using System.Text;

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

    public class InventoryUpdateJob(ulong id, ResourceType type, string url, RelatedDictionary related,
                                    SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                    string name, string description, ulong unifiedJobTemplate, JobLaunchType launchType,
                                    JobStatus status, ulong? executionEnvironment, string controllerNode, bool failed,
                                    DateTime? started, DateTime? finished, DateTime? canceledOn, double elapsed,
                                    string jobExplanation, string executionNode, LaunchedBy launchedBy,
                                    string? workUnitId, InventorySourceSource source, string sourcePath,
                                    string sourceVars, string scmBranch, ulong? credential, string enabledVar,
                                    string enabledValue, string hostFilter, bool overwrite, bool overwriteVars,
                                    string? customVirtualenv, int timeout, int verbosity, string limit, ulong inventory,
                                    ulong inventorySource, bool licenseError, bool orgHostLimitError,
                                    ulong? sourceProjectUpdate, ulong? instanceGroup, string scmRevision)
        : UnifiedJob(id, type, url, created, modified, name, launchType, status, executionEnvironment, failed,
                     started, finished, canceledOn, elapsed, jobExplanation, launchedBy, workUnitId),
          IInventoryUpdateJob, IResource, ICacheableResource
    {
        public new const string PATH = "/api/v2/inventory_updates/";

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
            var path = $"{ProjectUpdateJob.PATH}{projectUpdateId}/scm_inventory_updates/";
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

        public class Detail(ulong id, ResourceType type, string url, RelatedDictionary related, SummaryFieldsDictionary summaryFields,
                            DateTime created, DateTime? modified, string name, string description,
                            ulong unifiedJobTemplate, JobLaunchType launchType, JobStatus status,
                            ulong? executionEnvironment, string controllerNode, bool failed, DateTime? started,
                            DateTime? finished, DateTime? canceledOn, double elapsed, string jobArgs, string jobCwd,
                            Dictionary<string, string> jobEnv, string jobExplanation, string executionNode,
                            LaunchedBy launchedBy, string resultTraceback, bool eventProcessingFinished,
                            string? workUnitId, InventorySourceSource source, string sourcePath, string sourceVars,
                            string scmBranch, ulong? credential, string enabledVar, string enabledValue,
                            string hostFilter, bool overwrite, bool overwriteVars, string? customVirtualenv, int timeout,
                            int verbosity, string limit, ulong inventory, ulong inventorySource, bool licenseError,
                            bool orgHostLimitError, ulong? sourceProjectUpdate, ulong? instanceGroup, string scmRevision,
                            ulong? sourceProject)
            : InventoryUpdateJob(id, type, url, related, summaryFields, created, modified, name, description, unifiedJobTemplate,
                                 launchType, status, executionEnvironment, controllerNode, failed, started, finished, canceledOn,
                                 elapsed, jobExplanation, executionNode, launchedBy, workUnitId, source, sourcePath, sourceVars,
                                 scmBranch, credential, enabledVar, enabledValue, hostFilter, overwrite, overwriteVars,
                                 customVirtualenv, timeout, verbosity, limit, inventory, inventorySource, licenseError,
                                 orgHostLimitError, sourceProjectUpdate, instanceGroup, scmRevision),
              IInventoryUpdateJob, IJobDetail
        {
            public string JobArgs { get; } = jobArgs;
            public string JobCwd { get; } = jobCwd;
            public Dictionary<string, string> JobEnv { get; } = jobEnv;
            public string ResultTraceback { get; } = resultTraceback;
            public bool EventProcessingFinished { get; } = eventProcessingFinished;
            public ulong? SourceProject { get; } = sourceProject;
        }

        public RelatedDictionary Related { get; } = related;
        public SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

        public string Description { get; } = description;
        public ulong UnifiedJobTemplate { get; } = unifiedJobTemplate;
        public string ControllerNode { get; } = controllerNode;
        public string ExecutionNode { get; } = executionNode;

        public InventorySourceSource Source { get; } = source;
        public string SourcePath { get; } = sourcePath;
        public string SourceVars { get; } = sourceVars;
        public string ScmBranch { get; } = scmBranch;
        public ulong? Credential { get; } = credential;
        public string EnabledVar { get; } = enabledVar;
        public string EnabledValue { get; } = enabledValue;
        public string HostFilter { get; } = hostFilter;
        public bool Overwrite { get; } = overwrite;
        public bool OverwriteVars { get; } = overwriteVars;
        public string? CustomVirtualenv { get; } = customVirtualenv;
        public int Timeout { get; } = timeout;
        public int Verbosity { get; } = verbosity;
        public string Limit { get; } = limit;
        public ulong Inventory { get; } = inventory;
        public ulong InventorySource { get; } = inventorySource;
        public bool LicenseError { get; } = licenseError;
        public bool OrgHostLimitError { get; } = orgHostLimitError;
        public ulong? SourceProjectUpdate { get; } = sourceProjectUpdate;
        public ulong? InstanceGroup { get; } = instanceGroup;
        public string ScmRevision { get; } = scmRevision;

        public string GetDescription()
        {
            var sb = new StringBuilder($"[{Source}] {Name}");
            if (!string.IsNullOrEmpty(Description))
            {
                sb.Append($" ({Description})");
            }
            sb.Append($" Status={Status}");
            if (Finished is not null)
            {
                sb.Append($" Finished={Finished}");
            }
            return sb.ToString();
        }
    }

    public record CanUpdateInventorySource(ulong? InventorySource, bool CanUpdate);
}
