using System.Collections.Specialized;

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
                              Notification.Summary summaryFields, DateTime created, DateTime? modified,
                              ulong notificationTemplate, string error, JobStatus status, int notificationsSent,
                              NotificationType notificationType, string recipients, string subject, string? body)
                : INotification, IResource<Notification.Summary>
    {
        public const string PATH = "/api/v2/notifications/";
        /// <summary>
        /// Retrieve a Notification.<br/>
        /// API Path: <c>/api/v2/notifications/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Notification> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Notification>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Notifications.<br/>
        /// API Path: <c>/api/v2/notifications/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Notification> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Notification>(PATH, query, getAll))
            {
                foreach (var notification in result.Contents.Results)
                {
                    yield return notification;
                }
            }
        }
        public record Summary(NotificationTemplateSummary NotificationTemplate);


        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;

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
    }
}
