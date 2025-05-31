using System.Runtime.CompilerServices;

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
        /// Get a job of <paramref name="id"/>.
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/?id=<paramref name="id"/>/</c>
        /// </para>
        /// <para>
        /// The job is one of:
        /// <list type="bullet">
        /// <item><c>job</c></item>
        /// <item><c>workflow_job</c></item>
        /// <item><c>project_update</c></item>
        /// <item><c>inventory_update</c></item>
        /// <item><c>sytem_job</c></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="id">Unified Job ID</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<IUnifiedJob> GetAsync(ulong id, CancellationToken ct = default)
        {
            var query = new HttpQuery($"id={id}&page_size=1");
            var apiResult = await RestAPI.GetAsync<ResultSet>($"{PATH}?{query}", cancellationToken: ct);
            return apiResult.Contents.Results.OfType<IUnifiedJob>().Single();
        }

        /// <summary>
        /// Get jobs by id list
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/?id__in=<paramref name="idList"/>/</c>
        /// </para>
        /// <para>
        /// The job is one of:
        /// <list type="bullet">
        /// <item><c>job</c></item>
        /// <item><c>workflow_job</c></item>
        /// <item><c>project_update</c></item>
        /// <item><c>inventory_update</c></item>
        /// <item><c>sytem_job</c></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="idList"></param>
        public static async IAsyncEnumerable<IUnifiedJob> GetAsync(params ulong[] idList)
        {
            foreach (var query in new QueryBuilder().SetOrderBy("id")
                                                    .BuildWithIdList([.. idList.Order()]))
            {
                await foreach (var job in FindAsync(query))
                {
                    yield return job;
                }
            }
        }

        /// <summary>
        /// Get a job
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/?id=<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IUnifiedJob Get(ulong id)
        {
            var task = GetAsync(id);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Get jobs by id list
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/?id__in=<paramref name="idList"/>/</c>
        /// </para>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static IUnifiedJob[] Get(params ulong[] idList)
        {
            return [.. new QueryBuilder().SetOrderBy("id")
                                         .BuildWithIdList([.. idList.Order()])
                                         .SelectMany(static query => FindAsync(query).ToBlockingEnumerable())];
        }

        /// <summary>
        /// List Unified Jobs.<br/>
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<IUnifiedJob> FindAsync(HttpQuery? query = null,
                                                                    [EnumeratorCancellation]
                                                                    CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync(PATH, query, ct))
            {
                foreach (var job in result.Contents.Results.OfType<IUnifiedJob>())
                {
                    yield return job;
                }
            }
        }

        /// <summary>
        /// Find jobs
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        public static IUnifiedJob[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find jobs by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/unified_jobs/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static IUnifiedJob[] Find(string? searchWords = null,
                                            string orderBy = "-timestamp",
                                            ushort pageSize = 20,
                                            uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
                                          .SetOrderBy(orderBy)
                                          .SetPageSize(pageSize)
                                          .SetStartPage(startPage)
                                          .Build());
        }
    }
}

