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

    public abstract class UnifiedJobTemplate : ResourceBase, IUnifiedJobTemplate
    {
        public const string PATH = "/api/v2/unified_job_templates/";

        public abstract DateTime Created { get; }
        public abstract DateTime? Modified { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract DateTime? LastJobRun { get; }
        public abstract bool LastJobFailed { get; }
        public abstract DateTime? NextJobRun { get; }
        public abstract JobTemplateStatus Status { get; }

        /// <summary>
        /// Get schedules for this resource.
        /// <para>Query:</para>
        /// <list type="bullet">
        ///     <item><c>order_by=next_run</c></item>
        ///     <item><c>enabled=true</c> (when not <paramref name="all"/>)</item>
        ///     <item><c>not__next_run__isnull=true</c> (when not <paramref name="all"/>)</item>
        ///     <item><c>page_size=<paramref name="count"/></c></item>
        /// </list>
        /// </summary>
        /// <param name="all">Include disabled or next_run is emptied schedules</param>
        /// <param name="count">Number of schedules to retrieve</param>
        public IEnumerable<Schedule> GetSchedules(bool all = false, int count = 20)
        {
            if (Related.TryGetPath("schedules", out var path))
            {
                var query = HttpUtility.ParseQueryString("");
                query.Add("order_by", "next_run");
                if (!all)
                {
                    query.Add("enabled", "true");
                    query.Add("not__next_run__isnull", "true");
                }
                query.Add("page_size", $"{count}");
                return RestAPI.GetResultSet<Schedule>(path, query, false);
            }
            return [];
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }

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
