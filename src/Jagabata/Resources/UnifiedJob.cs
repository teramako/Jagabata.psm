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

    public abstract class UnifiedJob : ResourceBase, IUnifiedJob
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

        /// <summary>
        /// Get log of this job
        /// </summary>
        /// <param name="vt100Color">Get log as ANSI color format, otherwise as plain text format</param>
        public virtual string GetJobLog(bool vt100Color = true)
        {
            if (Related.TryGetPath("stdout", out var path))
            {
                var query = vt100Color ? "format=ansi" : "format=txt";
                return RestAPI.Get<string>($"{path}?{query}", AcceptType.Text);
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the Template resource related this job.
        /// </summary>
        /// <typeparam name="TResource">Resource class</typeparam>
        protected TResource? GetTemplate<TResource>() where TResource : class
        {
            return Related.TryGetPath("unified_job_template", out var path)
                ? RestAPI.Get<TResource>(path)
                : null;
        }

        /// <summary>
        /// Get job events for this job
        /// </summary>
        /// <param name="relatedKey">key for job events URL path in <see cref="ResourceBase.Related"/> dictionary</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">thrown when this resource has no <paramref name="relatedKey"/> data</exception>
        protected IEnumerable<TResource> GetEvents<TResource>(string relatedKey = "events")
            where TResource : JobEventBase
        {
            return Related.TryGetPath(relatedKey, out var path)
                ? RestAPI.GetResultSet<TResource>(path,
                                                  new HttpQuery("order_by=counter&page_size=200", QueryCount.Infinity))
                         .SelectMany(static apiResult => apiResult.Contents.Results)
                : [];
        }

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
            var query = new HttpQuery($"id={id}&page_size=1");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return apiResult.Contents.Results.OfType<IUnifiedJob>().Single();
        }
        public static async Task<IUnifiedJob[]> Get(params ulong[] idList)
        {
            if (idList.Length > 200)
            {
                throw new ArgumentException($"too many items: {nameof(idList)} Length must be less than or equal to 200.");
            }
            var query = new HttpQuery($"id__in={string.Join(',', idList)}&page_size={idList.Length}");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}");
            return [.. apiResult.Contents.Results.OfType<IUnifiedJob>()];
        }
        /// <summary>
        /// List Unified Jobs.<br/>
        /// API Path: <c>/api/v2/unified_jobs/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<IUnifiedJob> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync(PATH, query))
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

