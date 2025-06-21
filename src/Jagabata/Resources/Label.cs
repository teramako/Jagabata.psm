using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface ILabel
    {
        /// <summary>
        /// Name of the label.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Organization this label belongs to.
        /// </summary>
        ulong Organization { get; }
    }

    public class Label(ulong id, ResourceType type, string url, RelatedDictionary related,
                       SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                       ulong organization)
        : ResourceBase, ILabel
    {
        public const string PATH = "/api/v2/labels/";

        /// <summary>
        /// Get a Label
        /// <para>
        /// Implement API: <c>/api/v2/labels/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Label ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Label> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Label>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Label Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Labels
        /// <para>
        /// Implement API: <c>/api/v2/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Label> FindAsync(HttpQuery? query = null,
                                                              [EnumeratorCancellation]
                                                              CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Label>(PATH, query, ct))
            {
                foreach (var label in result.Contents.Results)
                {
                    yield return label;
                }
            }
        }

        /// <summary>
        /// Find Labels associated with <paramref name="resource"/>
        /// </summary>
        /// <remarks>
        /// Implement API: <c>/api/v2/{Type}/{Id}/labels/</c>
        /// <para>
        /// Available types of <paramref name="resource"/>:
        /// </para>
        /// <list type="bullet">
        ///     <item>Inventory</item>
        ///     <item>JobTemplate</item>
        ///     <item>Job</item>
        ///     <item>Schedule</item>
        ///     <item>WorkflowJobTemplate</item>
        ///     <item>WorkflowJob</item>
        ///     <item>WorkflowJobTemplateNode</item>
        ///     <item>WorkflowJobNode</item>
        /// </list>
        /// </remarks>
        /// <param name="resource">Resource object associated with</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Label> FindAsync(IResource resource,
                                                              HttpQuery? query = null,
                                                              [EnumeratorCancellation]
                                                              CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{resource.Id}/labels/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{resource.Id}/labels/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{resource.Id}/labels/",
                ResourceType.Schedule => $"{Schedule.PATH}{resource.Id}/labels/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{resource.Id}/labels/",
                ResourceType.WorkflowJob => $"{WorkflowJobBase.PATH}{resource.Id}/labels/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{resource.Id}/labels/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{resource.Id}/labels/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Label>(path, query, ct))
            {
                foreach (var label in result.Contents.Results)
                {
                    yield return label;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Label[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Labels by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/labels/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Label[] Find(string? searchWords = null,
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

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Label[] Find(IResource resource, HttpQuery query)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Labels associated with <paramref name="resource"/> by basic parameters.
        /// </summary>
        /// <inheritdoc cref="Find(string?, string, ushort, uint)"/>
        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Label[] Find(IResource resource,
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

        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public ulong Organization { get; } = organization;

        /// <summary>
        /// Get the organization related to this label
        /// </summary>
        public Organization? GetOrganization()
        {
            return Related.TryGetPath("organization", out var path)
                ? RestAPI.Get<Organization>(path)
                : null;
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, Name, string.Empty);
            if (SummaryFields.TryGetValue<OrganizationSummary>("Organization", out var org))
            {
                item.Metadata.Add("Organization", $"[{org.Type}:{org.Id}] {org.Name}");
            }
            return item;
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
