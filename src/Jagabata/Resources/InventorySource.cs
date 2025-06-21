using System.Runtime.CompilerServices;
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
        /// Get an Inventory Source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">InventorySource ID</param>
        /// <returns></returns>
        public static new async Task<InventorySource> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<InventorySource>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static new InventorySource Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Inventory Sources
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static new async IAsyncEnumerable<InventorySource> FindAsync(HttpQuery? query = null,
                                                                            [EnumeratorCancellation]
                                                                            CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(PATH, query, ct))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }

        /// <summary>
        /// Find Inventory Sources associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API:
        /// <list type="bullet">
        ///     <item><c>/api/v2/projects/{Id}/scm_inventory_sources/</c></item>
        ///     <item><c>/api/v2/{inventories | groups | hosts}/{Id}/inventory_sources/</c></item>
        /// </list>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>Project</item>
        ///     <item>Inventory</item>
        ///     <item>Group</item>
        ///     <item>Host</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<InventorySource> FindAsync(IResource resource,
                                                                        HttpQuery? query = null,
                                                                        [EnumeratorCancellation]
                                                                        CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Project => $"{Project.PATH}{resource.Id}/scm_inventory_sources/",
                ResourceType.Inventory => $"{Resources.Inventory.PATH}{resource.Id}/inventory_sources/",
                ResourceType.Group => $"{Group.PATH}{resource.Id}/inventory_sources/",
                ResourceType.Host => $"{Host.PATH}{resource.Id}/inventory_sources/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<InventorySource>(path, query, ct))
            {
                foreach (var inventorySource in result.Contents.Results)
                {
                    yield return inventorySource;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static new InventorySource[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static InventorySource[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Inventory Sources by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static new InventorySource[] Find(string? searchWords = null,
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

        /// <summary>
        /// Find Inventory Sources associated with <paramref name="resource"/> by basic parameters
        /// </summary>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        public static InventorySource[] Find(IResource resource,
                                             string? searchWords = null,
                                             string orderBy = "name",
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
        public InventoryUpdateJob[] GetRecentJobs(ushort count = 20)
        {
            return [.. FindResultsByRelatedKey<InventoryUpdateJob>("inventory_updates", null, "-id", count)];
        }

        /// <summary>
        /// Find the activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/activity_stream/</c>
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
        /// Implement API: <c>/api/v2/inventory_sources/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] FindActivityStream(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<ActivityStream>("activity_stream", query)];
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
        /// Find hosts related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Host[] FindHosts(string? searchWords = null,
                                string orderBy = "name",
                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Host>("hosts",
                                                     searchWords,
                                                     orderBy,
                                                     pageSize)];
        }

        /// <summary>
        /// Find hosts related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/hosts/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Host[] FindHosts(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Host>("hosts", query)];
        }

        /// <summary>
        /// Find groups related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Group[] FindGroups(string? searchWords = null,
                                  string orderBy = "name",
                                  ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Group>("groups",
                                                      searchWords,
                                                      orderBy,
                                                      pageSize)];
        }

        /// <summary>
        /// Find groups related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/groups/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Group[] FindGroups(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Group>("groups", query)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have start notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_started/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnStarted(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_started", query)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(string? searchWords = null,
                                                                         string orderBy = "name",
                                                                         ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have success notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_success/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnSuccess(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_success", query)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(string? searchWords = null,
                                                                       string orderBy = "name",
                                                                       ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error",
                                                                     searchWords,
                                                                     orderBy,
                                                                     pageSize)];
        }

        /// <summary>
        /// Find notification templates that have error notification enabled for this inventory source.
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/notification_templates_error/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public NotificationTemplate[] FindNotificationTemplatesOnError(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<NotificationTemplate>("notification_templates_error", query)];
        }

        /// <summary>
        /// Get the inventory related to this inventory source
        /// </summary>
        public Inventory? GetInventory()
        {
            return Related.TryGetPath("inventory", out var path)
                ? RestAPI.Get<Inventory>(path)
                : null;
        }

        /// <summary>
        /// Get the source project related to this inventory source
        /// </summary>
        public Project? GetSourceProject()
        {
            return Related.TryGetPath("source_project", out var path)
                ? RestAPI.Get<Project>(path)
                : null;
        }

        /// <summary>
        /// Find credentials related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Credential[] FindCredentials(string? searchWords = null,
                                            string orderBy = "name",
                                            ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials",
                                                           searchWords,
                                                           orderBy,
                                                           pageSize)];
        }

        /// <summary>
        /// Find credentials related to this inventory source
        /// <para>
        /// Implement API: <c>/api/v2/inventory_sources/{id}/credentials/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Credential[] FindCredentials(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Credential>("credentials", query)];
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
