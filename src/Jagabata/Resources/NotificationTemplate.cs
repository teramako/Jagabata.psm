using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<NotificationType>))]
    public enum NotificationType
    {
        Email,
        Grafana,
        IRC,
        Mattemost,
        Pagerduty,
        RoketChat,
        Slack,
        Twillo,
        Webhook
    }

    public interface INotificationTemplate
    {
        /// <summary>
        /// Name of this notification template.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this notification template.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Organization ID
        /// </summary>
        ulong Organization { get; }
        NotificationType NotificationType { get; }
        Dictionary<string, object?> NotificationConfiguration { get; }
        Messages? Messages { get; }
    }

    public class NotificationTemplate(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      string name, string description, ulong organization,
                                      NotificationType notificationType,
                                      Dictionary<string, object?> notificationConfiguration, Messages? messages)
        : ResourceBase, INotificationTemplate
    {
        public const string PATH = "/api/v2/notification_templates/";
        /// <summary>
        /// Retrieve a Notification Template.<br/>
        /// API Path: <c>/api/v2/notification_templates/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<NotificationTemplate> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<NotificationTemplate>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Notification Templates.<br/>
        /// API Path: <c>/api/v2/notification_templates/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<NotificationTemplate> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<NotificationTemplate>(PATH, query))
            {
                foreach (var notificationTemplate in result.Contents.Results)
                {
                    yield return notificationTemplate;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;

        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong Organization { get; } = organization;
        public NotificationType NotificationType { get; } = notificationType;
        public Dictionary<string, object?> NotificationConfiguration { get; } = notificationConfiguration;
        public Messages? Messages { get; } = messages;

        /// <summary>
        /// Find notifications related to this notification template
        /// <para>
        /// Implement API: <c>/api/v2/notification_templates/{id}/notifications/</c>
        /// </para>
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="orderBy">Sort keys (<c>','</c> separated values)</param>
        /// <param name="pageSize">Max number to retrieve</param>.
        public Notification[] FindNotifications(string? searchWords = null,
                                                string orderBy = "-id",
                                                ushort pageSize = 20)
        {
            return [.. FindResultsByRelatedKey<Notification>("notifications",
                                                             searchWords,
                                                             orderBy,
                                                             pageSize)];
        }

        /// <summary>
        /// Find notifications related to this notification template
        /// <para>
        /// Implement API: <c>/api/v2/notification_templates/{id}/notifications/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public Notification[] FindNotifications(HttpQuery query)
        {
            return [.. FindResultsByRelatedKey<Notification>("notifications", query)];
        }

        /// <summary>
        /// Get the organization related this notification template
        /// </summary>
        public Organization? GetOrganization()
        {
            return Related.TryGetPath("organization", out var path)
                ? RestAPI.Get<Organization>(path)
                : null;
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Type"] = $"{NotificationType}"
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }

    public record Messages(
        NMessage Error,
        NMessage Started,
        NMessage Success,
        ApprovalMessages WorkflowApproval
    );

    public record ApprovalMessages(
        NMessage Denied,
        NMessage Running,
        NMessage Approved,
        NMessage TimedOut
    );

    public record NMessage(
        string Body,
        string? Message
    );
}
