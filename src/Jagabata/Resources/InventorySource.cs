using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<InventorySourceSource>))]
    public enum InventorySourceSource
    {
        /// <summary>
        /// File, Directory or Script
        /// </summary>
        File,
        /// <summary>
        /// Template additional groups and hostvars at runtime.
        /// </summary>
        Constructed,
        /// <summary>
        /// Sourced from a Project
        /// </summary>
        Scm,
        /// <summary>
        /// Amazon EC2
        /// </summary>
        EC2,
        /// <summary>
        /// Google Compute Engine
        /// </summary>
        GCE,
        /// <summary>
        /// Microsoft Azure Resource Manager
        /// </summary>
        AzureRM,
        /// <summary>
        /// VMware vCenter
        /// </summary>
        VMware,
        /// <summary>
        /// Red Hat Satellite 6
        /// </summary>
        Satellite6,
        /// <summary>
        /// OpenStack
        /// </summary>
        OpenStack,
        /// <summary>
        /// Red Hat Virtualization
        /// </summary>
        RHV,
        /// <summary>
        /// Red Hat Ansible Automation Platform
        /// </summary>
        Controller,
        /// <summary>
        /// Red Hat Insights
        /// </summary>
        Insights,
    }

    [Flags]
    public enum InventorySourceOptions
    {
        None = 0,
        Overwrite = 1 << 0,
        OverwriteVars = 1 << 1,
        UpdateOnLaunch = 1 << 2,
    }

    public interface IInventorySource
    {
        /// <summary>
        /// Name of the inventory source.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of the inventory source.
        /// </summary>
        string Description { get; }
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
        /// <summary>
        /// The container image to be used for execution.
        /// </summary>
        ulong? ExecutionEnvironment { get; }
        ulong Inventory { get; }
        bool UpdateOnLaunch { get; }
        int UpdateCacheTimeout { get; }
        /// <summary>
        /// Project containing inventory file used as source.
        /// </summary>
        ulong? SourceProject { get; }
    }

    public class InventorySource(ulong id, ResourceType type, string url, RelatedDictionary related,
                                 SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                 string name, string description, InventorySourceSource source, string sourcePath,
                                 string sourceVars, string scmBranch, ulong? credential, string enabledVar,
                                 string enabledValue, string hostFilter, bool overwrite, bool overwriteVars,
                                 string? customVirtualenv, int timeout, int verbosity, string limit,
                                 DateTime? lastJobRun, bool lastJobFailed, DateTime? nextJobRun,
                                 JobTemplateStatus status, ulong? executionEnvironment, ulong inventory,
                                 bool updateOnLaunch, int updateCacheTimeout, ulong? sourceProject,
                                 bool lastUpdateFailed, DateTime? lastUpdated)
        : UnifiedJobTemplate, IInventorySource
    {
        public new const string PATH = "/api/v2/inventory_sources/";

        /// <summary>
        /// Retrieve an Inventory Source.<br/>
        /// API Path: <c>/api/v2/inventory_sources/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static new async Task<InventorySource> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<InventorySource>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Inventory Sources.<br/>
        /// API Path: <c>/api/v2/inventory_sources/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<InventorySource> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(PATH, query))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }
        /// <summary>
        /// List Inventory Sources for a Project.<br/>
        /// API Path: <c>/api/v2/projects/<paramref name="projectId"/>/scm_inventory_sources/</c>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventorySource> FindFromProject(ulong projectId,
                                                                              HttpQuery? query = null)
        {
            var path = $"{Project.PATH}{projectId}/scm_inventory_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(path, query))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }
        /// <summary>
        /// List Inventory Sources for an Inventory.<br/>
        /// API Path: <c>/api/v2/inventories/<paramref name="inventoryId"/>/inventory_sources/</c>
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventorySource> FindFromInventory(ulong inventoryId,
                                                                                HttpQuery? query = null)
        {
            var path = $"{Resources.Inventory.PATH}{inventoryId}/inventory_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(path, query))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }
        /// <summary>
        /// List Inventory Sources for an Group.<br/>
        /// API Path: <c>/api/v2/groups/<paramref name="groupId"/>/inventory_sources/</c>
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventorySource> FindFromGroup(ulong groupId,
                                                                            HttpQuery? query = null)
        {
            var path = $"{Group.PATH}{groupId}/inventory_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(path, query))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }
        /// <summary>
        /// List Inventory Sources for an Host.<br/>
        /// API Path: <c>/api/v2/hosts/<paramref name="hostId"/>/inventory_sources/</c>
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventorySource> FindFromHost(ulong hostId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Host.PATH}{hostId}/inventory_sources/";
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(path, query))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
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
        public override DateTime? LastJobRun { get; } = lastJobRun;
        public override bool LastJobFailed { get; } = lastJobFailed;
        public override DateTime? NextJobRun { get; } = nextJobRun;
        public override JobTemplateStatus Status { get; } = status;
        public ulong? ExecutionEnvironment { get; } = executionEnvironment;
        public ulong Inventory { get; } = inventory;
        public bool UpdateOnLaunch { get; } = updateOnLaunch;
        public int UpdateCacheTimeout { get; } = updateCacheTimeout;
        public ulong? SourceProject { get; } = sourceProject;
        public bool LastUpdateFailed { get; } = lastUpdateFailed;
        public DateTime? LastUpdated { get; } = lastUpdated;

        [JsonIgnore]
        public InventorySourceOptions Options => (Overwrite ? InventorySourceOptions.Overwrite : 0)
                                                 | (OverwriteVars ? InventorySourceOptions.OverwriteVars : 0)
                                                 | (UpdateOnLaunch ? InventorySourceOptions.UpdateOnLaunch : 0);

        /// <summary>
        /// Get the most recently executed jobs.
        /// Implement API: <c>/api/v2/inventory_sources/{id}/inventory_updates/</c>
        /// </summary>
        /// <param name="count">Number of jobs to retrieve</param>
        public InventoryUpdateJob[] GetRecentJobs(int count = 20)
        {
            return [.. RestAPI.GetResultSet<InventoryUpdateJob>($"{PATH}{Id}/inventory_updates/",
                                                                new HttpQuery($"order_by=-id&page_size={count}"))
                              .SelectMany(static apiResult => apiResult.Contents.Results)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(string? searchWords = null, ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", searchWords, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        /// <summary>
        /// Get the execution environment related to this inventory source
        /// </summary>
        public ExecutionEnvironment? GetExecutionEnvironment()
        {
            return Related.TryGetPath("execution_environment", out var path)
                ? RestAPI.Get<ExecutionEnvironment>(path)
                : null;
        }

        /// <summary>
        /// Get the hosts related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Host[] GetHosts(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Host>("hosts", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the hosts related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Host[] GetHosts(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Host>("hosts", query)];
        }

        /// <summary>
        /// Get the groups related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Group[] GetGroups(string? searchWords = null, string orderBy = "name", ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<Group>("groups", searchWords, orderBy, pageSize)];
        }

        /// <summary>
        /// Get the groups related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Group[] GetGroups(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<Group>("groups", query)];
        }

        /// <summary>
        /// Get the notification templates that have start notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnStarted(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have start notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Get the notification templates that have success notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have success notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Get the notification templates that have error notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnError(string? searchWords = null,
                                                                        string orderBy = "name",
                                                                        ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                    searchWords,
                                                                    orderBy,
                                                                    pageSize)];
        }

        /// <summary>
        /// Get the notification templates that have error notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] GetNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Status"] = $"{Status}",
                }
            };
            if (LastUpdated is not null)
            {
                item.Metadata.Add("LastUpdated", $"{LastUpdated}");
            }
            if (SummaryFields.TryGetValue<ProjectSummary>("SourceProject", out var project))
            {
                item.Metadata.Add("SourceProject", $"SourceProject=[{project.Type}:{project.Id}] {project.Name}");
            }
            if (SummaryFields.TryGetValue<InventorySummary>("Inventory", out var inventory))
            {
                item.Metadata.Add("Inventory", $"Inventory=[{inventory.Type}:{inventory.Id}]{inventory.Name}");
            }
            return item;
        }
    }
}
