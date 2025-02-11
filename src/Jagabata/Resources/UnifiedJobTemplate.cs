using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Web;

namespace Jagabata.Resources
{
    /// <summary>
    /// Template Status
    /// <list type="bullet">
    /// <item><term>new</term><description>New</description></item>
    /// <item><term>pending</term><description>Pending</description></item>
    /// <item><term>waiting</term><description>Waiting</description></item>
    /// <item><term>running</term><description>Running</description></item>
    /// <item><term>successful</term><description>Successful</description></item>
    /// <item><term>failed</term><description>Failed</description></item>
    /// <item><term>error</term><description>Error</description></item>
    /// <item><term>canceled</term><description>Canceled</description></item>
    /// <item><term>never updated</term><description>Never Updated</description></item>
    /// <item><term>ok</term><description>OK</description></item>
    /// <item><term>missing</term><description>Missing</description></item>
    /// <item><term>none</term><description>No Extrenal Source</description></item>
    /// <item><term>Updating</term><description>Updating</description></item>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<JobTemplateStatus>))]
    public enum JobTemplateStatus
    {
        New,
        Pending,
        Waiting,
        Running,
        Successful,
        Failed,
        Error,
        Canceled,
        NeverUpdated,
        OK,
        Missing,
        None,
        Updating
    }

    public interface IUnifiedJobTemplate
    {
        ulong Id { get; }
        ResourceType Type { get; }
        string Url { get; }
        /// <summary>
        /// Timestamp when this template was created.
        /// </summary>
        DateTime Created { get; }
        /// <summary>
        /// Timestamp when this template was last modified.
        /// </summary>
        DateTime? Modified { get; }
        /// <summary>
        /// Name of this template.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this template.
        /// </summary>
        string Description { get; }
        DateTime? LastJobRun { get; }
        bool LastJobFailed { get; }
        DateTime? NextJobRun { get; }
        JobTemplateStatus Status { get; }
    }

    public abstract class UnifiedJobTemplate(ulong id,
                                             ResourceType type,
                                             string url,
                                             DateTime created,
                                             DateTime? modified,
                                             string name,
                                             string description,
                                             DateTime? lastJobRun,
                                             bool lastJobFailed,
                                             DateTime? nextJobRun,
                                             JobTemplateStatus status)
        : SummaryFieldsContainer, IUnifiedJobTemplate
    {
        public const string PATH = "/api/v2/unified_job_templates/";

        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public DateTime? LastJobRun { get; } = lastJobRun;
        public bool LastJobFailed { get; } = lastJobFailed;
        public DateTime? NextJobRun { get; } = nextJobRun;
        public JobTemplateStatus Status { get; } = status;

        /// <summary>
        /// Retrieve a job template.
        ///
        /// The job template is one of:
        /// <list type="bullet">
        /// <item><term><see cref="JobTemplate"/></term><description>Type: <c>job_template</c></description></item>
        /// <item><term><see cref="WorkflowJobTemplate"/></term><description>Type: <c>workflow_job_template</c></description></item>
        /// <item><term><see cref="Project"/></term><description>Type: <c>project</c></description></item>
        /// <item><term><see cref="InventorySource"/></term><description>Type: <c>inventory_source</c></description></item>
        /// <item><term><see cref="SystemJobTemplate"/></term><description>Type: <c>sytem_job_template</c></description></item>
        /// </list>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<IUnifiedJobTemplate> Get(long id)
        {
            var query = HttpUtility.ParseQueryString($"id={id}&page_size=1");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return apiResult.Contents.Results.OfType<IUnifiedJobTemplate>().Single();
        }
        public static async Task<IUnifiedJobTemplate[]> Get(params ulong[] idList)
        {
            if (idList.Length > 200)
            {
                throw new ArgumentException($"too many items: {nameof(idList)} Length must be less than or equal to 200.");
            }
            var query = HttpUtility.ParseQueryString($"id__in={string.Join(',', idList)}&page_size={idList.Length}");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return [.. apiResult.Contents.Results.OfType<IUnifiedJobTemplate>()];
        }
        /// <summary>
        /// List Unified Job Templates.<br/>
        /// API Path: <c>/api/v2/unified_job_templates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<IUnifiedJobTemplate> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync(PATH, query, getAll))
            {
                foreach (var obj in result.Contents.Results)
                {
                    if (obj is IUnifiedJobTemplate jobTemplate)
                    {
                        yield return jobTemplate;
                    }
                }
            }
        }
    }
}
