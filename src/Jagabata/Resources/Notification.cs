using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    public interface INotification
    {
        DateTime Created { get; }
        DateTime? Modified { get; }
        ulong NotificationTemplate { get; }
        string Error { get; }
        JobStatus Status { get; }
        int NotificationsSent { get; }
        NotificationType NotificationType { get; }
        string Recipients { get; }
        string Subject { get; }
        string? Body { get; }

    }


    public class Notification(ulong id, ResourceType type, string url, RelatedDictionary related,
                              SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                              ulong notificationTemplate, string error, JobStatus status, int notificationsSent,
                              NotificationType notificationType, string recipients, string subject, string? body)
        : ResourceBase, INotification
    {
        public const string PATH = "/api/v2/notifications/";

        /// <summary>
        /// Get a Notification
        /// <para>
        /// Implement API: <c>/api/v2/notifications/<paramref name="id"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="id">Notification ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<Notification> GetAsync(ulong id, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Notification>($"{PATH}{id}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(ulong, CancellationToken)"/>
        public static Notification Get(ulong id)
        {
            return GetAsync(id).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find Notifications
        /// <para>
        /// Implement API: <c>/api/v2/notifications/</c>
        /// </para>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Notification> FindAsync(HttpQuery? query = null,
                                                                     [EnumeratorCancellation]
                                                                     CancellationToken ct = default)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Notification>(PATH, query, ct))
            {
                foreach (var notification in result.Contents.Results)
                {
                    yield return notification;
                }
            }
        }

        /// <summary>
        /// Find Notifications associated with <paramref name="resource"/>
        /// <para>
        /// Implement API: <c>/api/v2/{Type}/{Id}/notifications/</c>
        /// </para>
        /// Available types of <paramref name="resource"/>:
        /// <list type="bullet">
        ///     <item>ProjectUpdate</item>
        ///     <item>InventoryUpdate</item>
        ///     <item>Job</item>
        ///     <item>AdHocCommand</item>
        ///     <item>SystemJob</item>
        ///     <item>NotificationTemplate</item>
        ///     <item>WorkflowJob</item>
        /// </list>
        /// </summary>
        /// <param name="resource">Resource object associated with this group</param>
        /// <param name="query"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Notification> FindAsync(IResource resource,
                                                                     HttpQuery? query = null,
                                                                     [EnumeratorCancellation]
                                                                     CancellationToken ct = default)
        {
            var path = resource.Type switch
            {
                ResourceType.ProjectUpdate => $"{ProjectUpdateJobBase.PATH}{resource.Id}/notifications/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJobBase.PATH}{resource.Id}/notifications/",
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{resource.Id}/notifications/",
                ResourceType.AdHocCommand => $"{AdHocCommandBase.PATH}{resource.Id}/notifications/",
                ResourceType.SystemJob => $"{SystemJobBase.PATH}{resource.Id}/notifications/",
                ResourceType.NotificationTemplate => $"{Resources.NotificationTemplate.PATH}{resource.Id}/notifications/",
                ResourceType.WorkflowJob => $"{WorkflowJobBase.PATH}{resource.Id}/notifications/",
                _ => throw new ArgumentException($"Not suppored type: {resource.Type}")
            };
            await foreach (var result in RestAPI.GetResultSetAsync<Notification>(path, query, ct))
            {
                foreach (var notification in result.Contents.Results)
                {
                    yield return notification;
                }
            }
        }

        /// <inheritdoc cref="FindAsync(HttpQuery?, CancellationToken)"/>
        public static Notification[] Find(HttpQuery query)
        {
            return [.. FindAsync(query).ToBlockingEnumerable()];
        }

        /// <inheritdoc cref="FindAsync(IResource, HttpQuery?, CancellationToken)"/>
        public static Notification[] Find(IResource resource, HttpQuery? query = null)
        {
            return [.. FindAsync(resource, query).ToBlockingEnumerable()];
        }

        /// <summary>
        /// Find Notifications by basic parameters.
        /// <para>
        /// Implement API: <c>/api/v2/notifications/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="startPage"></param>
        public static Notification[] Find(string? searchWords = null,
                                          string orderBy = "-id",
                                          ushort pageSize = 20,
                                          uint startPage = 1)
        {
            return Find(new QueryBuilder().SetSearchWords(searchWords)
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
        public ulong NotificationTemplate { get; } = notificationTemplate;
        public string Error { get; } = error;
        public JobStatus Status { get; } = status;
        public int NotificationsSent { get; } = notificationsSent;
        public NotificationType NotificationType { get; } = notificationType;
        public string Recipients { get; } = recipients;
        public string Subject { get; } = subject;
        public string? Body { get; } = body;

        /// <summary>
        /// Get the notification template related to this notification.
        /// </summary>
        public NotificationTemplate? GetTemplate()
        {
            return Related.TryGetPath("notification_template", out var path)
                ? RestAPI.Get<NotificationTemplate>(path)
                : null;
        }

        /// <summary>
        /// Get the job related to this notification.
        /// </summary>
        public IUnifiedJob? GetJob()
        {
            var query = new QueryBuilder().Add("notifications", $"{Id}").SetPageSize(1).Build();
            return RestAPI.GetResultSetAsync(UnifiedJob.PATH, query)
                          .ToBlockingEnumerable()
                          .SelectMany(static apiResult => apiResult.Contents.Results)
                          .OfType<IUnifiedJob>()
                          .FirstOrDefault();
        }

        protected override CacheItem GetCacheItem()
        {
            var item = new CacheItem(Type, Id, string.Empty, string.Empty)
            {
                Metadata = {
                    ["Type"] = $"{NotificationType}",
                    ["Status"] = $"{Status}",
                    ["Modified"] = $"{Modified}",
                    ["Subject"] = Subject,
                    ["Error"] = Error
                }
            };
            if (SummaryFields.TryGetValue<NotificationTemplateSummary>("NotificationTemplate", out var noti))
            {
                item.Name = noti.Name;
                item.Metadata.Add("Template", $"[{noti.Type}:{noti.Id}] {noti.Name}");
            }
            return item;
        }
    }
}
