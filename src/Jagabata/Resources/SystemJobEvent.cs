using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public class SystemJobEvent(ulong id, ResourceType type, string url, RelatedDictionary related,
                                SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                JobEventEvent @event, int counter, string eventDisplay, Dictionary<string, object?> eventData,
                                bool failed, bool changed, string uuid, string stdout, int startLine, int endLine,
                                JobVerbosity verbosity, ulong systemJob)
        : JobEventBase
    {
        /// <summary>
        /// Find System Job Events for a System Job
        /// <para>
        /// Implement API: <c>/api/v2/system_jobs/<paramref name="id"/>/events/</c>
        /// </para>
        /// </summary>
        /// <param name="id">System Job ID</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<SystemJobEvent> FindAsync(ulong id,
                                                                       HttpQuery? query = null,
                                                                       [EnumeratorCancellation]
                                                                       CancellationToken ct = default)
        {
            var path = $"{SystemJobBase.PATH}{id}/events/";
            await foreach (var result in RestAPI.GetResultSetAsync<SystemJobEvent>(path, query, ct))
            {
                foreach (var jobEvent in result.Contents.Results)
                {
                    yield return jobEvent;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        public static SystemJobEvent[] Find(ulong id, HttpQuery? query = null)
        {
            return [.. FindAsync(id, query).ToBlockingEnumerable()];
        }

        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        /// <inheritdoc cref="FindAsync(ulong, HttpQuery?, CancellationToken)"/>
        public static SystemJobEvent[] Find(ulong id,
                                            string? searchWords = null,
                                            string orderBy = "counter",
                                            ushort pageSize = 20,
                                            uint startPage = 1)
        {
            return Find(id, new QueryBuilder().SetSearchWords(searchWords)
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
        public override JobEventEvent Event { get; } = @event;
        public override int Counter { get; } = counter;
        public override string EventDisplay { get; } = eventDisplay;
        public override Dictionary<string, object?> EventData { get; } = eventData;
        public override bool Failed { get; } = failed;
        public override bool Changed { get; } = changed;
        public override string UUID { get; } = uuid;
        public override string Stdout { get; } = stdout;
        public override int StartLine { get; } = startLine;
        public override int EndLine { get; } = endLine;
        public override JobVerbosity Verbosity { get; } = verbosity;
        public ulong SystemJob { get; } = systemJob;
    }
}
