using System.Text;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// For SummaryFields property
    /// </summary>
    [JsonConverter(typeof(Json.SummaryFieldsConverter))]
    public class SummaryFieldsDictionary : Dictionary<string, object>
    {
    }

    public abstract record SummaryBase
    {
        public sealed override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            if (PrintMembers(sb)) sb.Append(' ');
            sb.Append('}');
            return sb.ToString();
        }
    }
    public abstract record ResourceSummary(ulong Id, ResourceType Type) : SummaryBase, IResource;

    [JsonConverter(typeof(Json.CapabilityConverter))]
    [Flags]
    public enum Capability
    {
        None = 0,
        Edit = 1 << 0,
        Delete = 1 << 1,
        Start = 1 << 2,
        Schedule = 1 << 3,
        Copy = 1 << 4,
        Use = 1 << 5,
        AdHoc = 1 << 6,
    }

    // Application in Token
    public record ApplicationSummary(ulong Id, string Name)
        : ResourceSummary(Id, ResourceType.OAuth2Application);

    // List<Group> in Host
    public record GroupSummary(ulong Id, string Name)
        : ResourceSummary(Id, ResourceType.Group);

    // List<Label> in Inventory, Job, JobTemplate, WorkflowJob, WorkflowJobTemplate
    public record LabelSummary(ulong Id, string Name)
        : ResourceSummary(Id, ResourceType.Label);

    // Host in AdHocCommandJobEvent, JobEvent, JobHostSummary
    public record HostSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.Host);

    // Organization in Application, Credential, ExecutionEnvironment, Inventory, InventorySource, InventoryUpdate,
    //                 JobTemplate, Job, Label, NotificationTemplate, Project, ProjectUpdate, Team, WorkflowJobtemplate
    public record OrganizationSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.Organization);

    // CredentialType in Credential
    public record CredentialTypeSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.CredentialType);

    // ObjectRoles in Credential, InstanceGroup, Inventory, JobTemplate, Project, Team, WorkflowJobTemplate
    public record ObjectRoleSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.Role);

    // JobTemplate in Job
    public record JobTemplateSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.JobTemplate);

    // NotificationTemplate in Notification
    public record NotificationTemplateSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.NotificationTemplate);

    // WorkflowJobTemplate in WorkflowApproval, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplateNode
    public record WorkflowJobTemplateSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.WorkflowJobTemplate);

    // WorkflowJob in WorkflowApproval, WorkflowJobNode
    public record WorkflowJobSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.WorkflowJob);

    // Actor in ActivityStream
    // CreatedBy in AdHocCommand, Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory,
    //              InventorySource, InventoryUpdate, JobTemplate, Job, Label, NotificationTemplate, Organization,
    //              Project, Schedule, Team, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // ModifiedBy in Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory InventorySource,
    //               JobTemplate, Label, NotificationTemplate, Organization, Project, Schedule, Team,
    //               WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // User in Token
    // ApprovedOrDeniedBy in WorkflowApproval
    public record UserSummary(ulong Id, string Username, string FirstName, string LastName)
        : ResourceSummary(Id, ResourceType.User);

    // ObjectRoles in Organization
    public record OrganizationObjectRoleSummary(ulong Id, string Name, string Description, bool UserOnly = false)
        : ResourceSummary(Id, ResourceType.Role);

    // ExecutionEnvironment in AdHocCommand, InventorySource, InventoryUpdate, JobTemplate, Job, SystemJob
    // DefaultEnvironment in Organization, Project, ProjectUpdate
    // ResolvedEnvironment in SystemJobTemplate, WorkflowApprovalTemplate
    public record EnvironmentSummary(ulong Id, string Name, string Description, string Image)
        : ResourceSummary(Id, ResourceType.ExecutionEnvironment);

    // RelatedFieldCounts in Organization
    public record RelatedFieldCountsSummary(int Inventories, int Teams, int Users, int JobTemplates,
                                            int Admins, int Projects)
        : SummaryBase;

    // List<Token> in Application
    public record TokenSummary(ulong Id, string Token, string Scope)
        : ResourceSummary(Id, ResourceType.OAuth2AccessToken);

    public record ListSummary<T>(int Count, T[] Results)
        : SummaryBase;

    // Credential in AdHocCommand, InventorySource, InventoryUpdate, Project, ProjectUpdate
    // SourceCredential in CredentialInputSource
    // TargetCredential in CredentialInputSource
    public record CredentialSummary(ulong Id, string Name, string Description, string Kind,
                                    bool Cloud = false, bool Kubernetes = false, ulong? CredentialTypeId = null)
        : ResourceSummary(Id, ResourceType.Credential);

    // Credentials in JobTemplate, Job
    public record JobTemplateCredentialSummary(ulong Id, string Name, string Description, string Kind, bool Cloud)
        : ResourceSummary(Id, ResourceType.Credential);

    // LastJob in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public record LastJobSummary(ulong Id, string Name, string Description, DateTime? Finished,
                                 JobStatus Status, bool Failed)
        : SummaryBase;

    // RecentJobs in Host
    public record HostRecentJobSummary(ulong Id, string Name, ResourceType Type, JobStatus Status, DateTime? Finished)
        : ResourceSummary(Id, Type);

    // RecentJobs in JobTemplate, WorkflowJobTemplate
    public record RecentJobSummary(ulong Id, JobStatus Status, DateTime? Finished, DateTime? CanceledOn, ResourceType Type)
        : ResourceSummary(Id, Type);

    // Job in WorkflowJobNode
    public record JobSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed,
                             double Elapsed, ResourceType Type)
        : ResourceSummary(Id, Type);

    // LastJob in Host
    public record HostLastJobSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed,
                                     double Elapsed, ulong JobTemplateId, string JobTemplateName)
        : ResourceSummary(Id, ResourceType.Job);

    // Job in JobEvent, JobHostSummary
    public record JobExSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed,
                               double Elapsed, ResourceType Type, ulong JobTemplateId, string JobTemplateName)
        : ResourceSummary(Id, Type);

    // SourceWorkflowJob in Job, WorkflowApproval
    public record SourceWorkflowJobSummary(ulong Id, string Name, string Description, JobStatus Status,
                                           bool Failed, double Elapsed)
        : ResourceSummary(Id, ResourceType.WorkflowJob);

    // AncestorJob in Job
    public record AncestorJobSummary(ulong Id, string Name, ResourceType Type, string Url)
        : ResourceSummary(Id, Type);

    // LastJobHostSummary in Host
    public record LastJobHostSummary(ulong Id, bool Failed)
        : ResourceSummary(Id, ResourceType.JobHostSummary);

    // LastUpdate in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public record LastUpdateSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed)
        : SummaryBase;

    // Inventory in AdHocCommand, Group, Host, InventorySource, InventoryUpdate, JobTemplate, Job, Schedule, WorkflowJobTemplate
    public record InventorySummary(ulong Id, string Name, string Description, bool HasActiveFailures, int TotalHosts,
                                   int HostsWithActiveFailures, int TotalGroups, bool HasInventorySources,
                                   int TotalInventorySources, int InventorySourcesWithFailures, ulong OrganizationId, string Kind)
        : ResourceSummary(Id, ResourceType.Inventory);

    // InventorySource in InventoryUpdate
    public record InventorySourceSummary(ulong Id, string Name, string Source, DateTime LastUpdated, JobStatus Status)
        : ResourceSummary(Id, ResourceType.InventorySource);

    // SourceProject in InventorySource, InventoryUpdate
    // Project in JobTemplate, Job, ProjectUpdate
    public record ProjectSummary(ulong Id, string Name, string Description, JobTemplateStatus Status,
                                 string ScmType, bool AllowOverride)
        : ResourceSummary(Id, ResourceType.Project);

    // ProjectUpdate in ProjectUpdateJobEvent
    public record ProjectUpdateSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed)
        : ResourceSummary(Id, ResourceType.ProjectUpdate);

    // UnifiedJobTemplate in InventoryUpdate, Job, ProjectUpdate, Schedule, SystemJob, WorkflowApproval, WorkflowJob,
    //                       WorkflowJobNode, WorkflowJobTemplateNode
    public record UnifiedJobTemplateSummary(ulong Id, string Name, string Description, ResourceType UnifiedJobType)
        : SummaryBase
    {
        public ResourceType Type => UnifiedJobType switch
        {
            ResourceType.Job => ResourceType.JobTemplate,
            ResourceType.ProjectUpdate => ResourceType.Project,
            ResourceType.InventoryUpdate => ResourceType.InventorySource,
            ResourceType.WorkflowJob => ResourceType.WorkflowJobTemplate,
            ResourceType.SystemJob => ResourceType.SystemJobTemplate,
            _ => ResourceType.None
        };
    }

    // InstanceGroup in AdHocCommand, InventoryUpdate, Job, ProjectUpdate, SystemJob
    public record InstanceGroupSummary(ulong Id, string Name, bool IsContainerGroup)
        : ResourceSummary(Id, ResourceType.InstanceGroup);

    // Owners in Credential
    public record OwnerSummary(ulong Id, ResourceType Type, string Name, string Description, string Url)
        : ResourceSummary(Id, Type);

    // Schedule in InventoryUpdate, Job, ProjectUpdate, SystemJob, WorkflowJob
    public record ScheduleSummary(ulong Id, string Name, string Description, DateTime NextRun)
        : ResourceSummary(Id, ResourceType.Schedule);

    // RecentNotification in NotificationTemplate
    public record RecentNotificationSummary(ulong Id, JobStatus Status, DateTime Created, string Error)
        : ResourceSummary(Id, ResourceType.Notification);

    // WorkflowApprovalTemplate in WorkflowApproval
    public record WorkflowApprovalTemplateSummary(ulong Id, string Name, string Description, int Timeout)
        : ResourceSummary(Id, ResourceType.WorkflowApprovalTemplate);

    public record AdHocCommandSummary(ulong Id, string Name, JobStatus Status, string Limit)
        : ResourceSummary(Id, ResourceType.AdHocCommand);

    public record InstanceSummary(ulong Id, string Hostname)
        : ResourceSummary(Id, ResourceType.Instance);

    public record NotificationSummary(ulong Id, JobStatus Status, string NotificationType, ulong NotificationTemplateId)
        : ResourceSummary(Id, ResourceType.Notification);

    public record RoleSummary(ulong Id, string RoleField)
        : ResourceSummary(Id, ResourceType.Role);

    public record SettingSummary(string Name, string Category)
        : SummaryBase;

    public record SurveySummary(string Title, string Description)
        : SummaryBase;

    public record TeamSummary(ulong Id, string Name, string Description)
        : ResourceSummary(Id, ResourceType.Team);

    public record WorkflowJobTemplateNodeSummary(ulong Id, ulong UnifiedJobTemplateId)
        : ResourceSummary(Id, ResourceType.WorkflowJobTemplateNode);
}
