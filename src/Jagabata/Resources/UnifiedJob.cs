using System.Collections.Specialized;
using System.Web;

namespace Jagabata.Resources
{
    public interface IUnifiedJobSummary : IResource
    {
        string Url { get; }
        string Name { get; }
        JobStatus Status { get; }
        double Elapsed { get; }
        bool Failed { get; }
    }

    public interface IUnifiedJob : IUnifiedJobSummary
    {
        DateTime Created { get; }
        DateTime? Modified { get; }
        JobLaunchType LaunchType { get; }
        DateTime? Started { get; }
        DateTime? Finished { get; }
        DateTime? CanceledOn { get; }
        string JobExplanation { get; }
        string? WorkUnitId { get; }
    }

    public abstract class UnifiedJob : SummaryFieldsContainer, IUnifiedJob
    {
        public const string PATH = "/api/v2/unified_jobs/";

        public abstract DateTime Created { get; }
        public abstract DateTime? Modified { get; }
        public abstract string Name { get; }
        public abstract JobLaunchType LaunchType { get; }
        public abstract JobStatus Status { get; }
        public abstract bool Failed { get; }
        public abstract DateTime? Started { get; }
        public abstract DateTime? Finished { get; }
        public abstract DateTime? CanceledOn { get; }
        public abstract double Elapsed { get; }
        public abstract string JobExplanation { get; }
        public abstract string? WorkUnitId { get; }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }

        /// <summary>
        /// Retrieve a job.
        /// The job is one of:
        /// <list type="bullet">
        /// <item><term><see cref="JobTemplateJob"/></term><description>Type: <c>job</c></description></item>
        /// <item><term><see cref="WorkflowJob"/></term><description>Type: <c>workflow_job</c></description></item>
        /// <item><term><see cref="ProjectUpdateJob"/></term><description>Type: <c>project_update</c></description></item>
        /// <item><term><see cref="InventoryUpdateJob"/></term><description>Type: <c>inventory_update</c></description></item>
        /// <item><term><see cref="SystemJob"/></term><description>Type: <c>sytem_job</c></description></item>
        /// </list>
        /// </summary>
        /// <param name="id">Unified Job ID</param>
        /// <returns></returns>
        public static async Task<IUnifiedJob> Get(ulong id)
        {
            var query = HttpUtility.ParseQueryString($"id={id}&page_size=1");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return apiResult.Contents.Results.OfType<IUnifiedJob>().Single();
        }
        public static async Task<IUnifiedJob[]> Get(params ulong[] idList)
        {
            if (idList.Length > 200)
            {
                throw new ArgumentException($"too many items: {nameof(idList)} Length must be less than or equal to 200.");
            }
            var query = HttpUtility.ParseQueryString($"id__in={string.Join(',', idList)}&page_size={idList.Length}");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return [.. apiResult.Contents.Results.OfType<IUnifiedJob>()];
        }
        /// <summary>
        /// List Unified Jobs.<br/>
        /// API Path: <c>/api/v2/unified_jobs/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<IUnifiedJob> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync(PATH, query, getAll))
            {
                foreach (var obj in result.Contents.Results)
                {
                    if (obj is IUnifiedJob job)
                    {
                        yield return job;
                    }
                }
            }
        }
    }
}

