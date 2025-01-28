using System.Diagnostics.CodeAnalysis;
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
        public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            if (TryGetValue(key, out var obj) && obj is T tValue)
            {
                value = tValue;
                return true;
            }
            value = default;
            return false;
        }
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

    public abstract record ResourceSummary(ulong Id, ResourceType Type)
        : SummaryBase, IResource, ICacheableResource
    {
        public abstract string GetDescription();
    }

    public abstract record NamedResourceSummary(ulong Id, ResourceType Type, string Name)
        : SummaryBase, IResource, ICacheableResource
    {
        public virtual string GetDescription()
        {
            return Name;
        }
    }

    public abstract record NameAndDescriptionResourceSummary(ulong Id, ResourceType Type,
                                                             string Name, string Description)
        : SummaryBase, IResource, ICacheableResource
    {
        public virtual string GetDescription()
        {
            return string.IsNullOrEmpty(Description) ? Name : $"{Name} ({Description})";
        }
    }

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
    public sealed record ApplicationSummary(ulong Id, string Name)
        : NamedResourceSummary(Id, ResourceType.OAuth2Application, Name);

    // List<Group> in Host
    public sealed record GroupSummary(ulong Id, string Name)
        : NamedResourceSummary(Id, ResourceType.Group, Name);

    // List<Label> in Inventory, Job, JobTemplate, WorkflowJob, WorkflowJobTemplate
    public sealed record LabelSummary(ulong Id, string Name)
        : NamedResourceSummary(Id, ResourceType.Label, Name);

    // Host in AdHocCommandJobEvent, JobEvent, JobHostSummary
    public sealed record HostSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Host, Name, Description);

    // Organization in Application, Credential, ExecutionEnvironment, Inventory, InventorySource, InventoryUpdate,
    //                 JobTemplate, Job, Label, NotificationTemplate, Project, ProjectUpdate, Team, WorkflowJobtemplate
    public sealed record OrganizationSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Organization, Name, Description);

    // CredentialType in Credential
    public sealed record CredentialTypeSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Credential, Name, Description);

    // ObjectRoles in Credential, InstanceGroup, Inventory, JobTemplate, Project, Team, WorkflowJobTemplate
    public sealed record ObjectRoleSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Role, Name, Description);

    // JobTemplate in Job
    public sealed record JobTemplateSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.JobTemplate, Name, Description);

    // NotificationTemplate in Notification
    public sealed record NotificationTemplateSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.NotificationTemplate, Name, Description);

    // WorkflowJobTemplate in WorkflowApproval, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplateNode
    public sealed record WorkflowJobTemplateSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.WorkflowJobTemplate, Name, Description);

    // WorkflowJob in WorkflowApproval, WorkflowJobNode
    public sealed record WorkflowJobSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.WorkflowJob, Name, Description);

    // Actor in ActivityStream
    // CreatedBy in AdHocCommand, Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory,
    //              InventorySource, InventoryUpdate, JobTemplate, Job, Label, NotificationTemplate, Organization,
    //              Project, Schedule, Team, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // ModifiedBy in Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory InventorySource,
    //               JobTemplate, Label, NotificationTemplate, Organization, Project, Schedule, Team,
    //               WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // User in Token
    // ApprovedOrDeniedBy in WorkflowApproval
    public sealed record UserSummary(ulong Id, string Username, string FirstName, string LastName)
        : ResourceSummary(Id, ResourceType.User)
    {
        public override string GetDescription()
        {
            return Username;
        }
    }

    // ObjectRoles in Organization
    public sealed record OrganizationObjectRoleSummary(ulong Id, string Name, string Description, bool UserOnly = false)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Role, Name, Description)
    {
        public override string GetDescription()
        {
            return UserOnly ? $"{base.GetDescription()} [UserOnly]" : base.GetDescription();
        }
    }

    // ExecutionEnvironment in AdHocCommand, InventorySource, InventoryUpdate, JobTemplate, Job, SystemJob
    // DefaultEnvironment in Organization, Project, ProjectUpdate
    // ResolvedEnvironment in SystemJobTemplate, WorkflowApprovalTemplate
    public sealed record EnvironmentSummary(ulong Id, string Name, string Description, string Image)
        : NameAndDescriptionResourceSummary(Id, ResourceType.ExecutionEnvironment, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Image={Image}";
        }
    };

    // RelatedFieldCounts in Organization
    public sealed record RelatedFieldCountsSummary(int Inventories, int Teams, int Users, int JobTemplates, int Admins, int Projects)
        : SummaryBase;

    // List<Token> in Application
    public sealed record TokenSummary(ulong Id, string Token, string Scope)
        : ResourceSummary(Id, ResourceType.OAuth2AccessToken)
    {
        public override string GetDescription()
        {
            return $"Scope={Scope}";
        }
    }

    public record ListSummary<T>(int Count, T[] Results)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            for (var i = 0; i < Count; i++)
            {
                var item = Results[i];
                if (item is not null)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(item.ToString());
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
    }

    // Credential in AdHocCommand, InventorySource, InventoryUpdate, Project, ProjectUpdate
    // SourceCredential in CredentialInputSource
    // TargetCredential in CredentialInputSource
    public sealed record CredentialSummary(ulong Id, string Name, string Description, string Kind,
                                    bool Cloud = false, bool Kubernetes = false, ulong? CredentialTypeId = null)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Credential, Name, Description)
    {
        public override string GetDescription()
        {
            return $"[{Kind}] {base.GetDescription()}";
        }
    };

    // Credentials in JobTemplate, Job
    public sealed record JobTemplateCredentialSummary(ulong Id, string Name, string Description, string Kind, bool Cloud)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Credential, Name, Description)
    {
        public override string GetDescription()
        {
            return $"[{Kind} {base.GetDescription()}";
        }
    };

    // LastJob in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public sealed record LastJobSummary(ulong Id, string Name, string Description, DateTime? Finished,
                                 JobStatus Status, bool Failed)
        : SummaryBase;

    // RecentJobs in Host
    public sealed record HostRecentJobSummary(ulong Id, string Name, ResourceType Type, JobStatus Status, DateTime? Finished)
        : NamedResourceSummary(Id, Type, Name)
    {
        public override string GetDescription()
        {
            return Finished is null
                   ? $"{base.GetDescription()} Status={Status}"
                   : $"{base.GetDescription()} Status={Status} Finished={Finished}";
        }
    }

    // RecentJobs in JobTemplate, WorkflowJobTemplate
    public sealed record RecentJobSummary(ulong Id, JobStatus Status, DateTime? Finished,
                                          DateTime? CanceledOn, ResourceType Type)
        : ResourceSummary(Id, Type), ICacheableResource
    {
        public override string GetDescription()
        {
            var sb = new StringBuilder($"Status={Status}");
            if (Finished is not null)
            {
                sb.Append($" Finished={Finished}");
            }
            if (CanceledOn is not null)
            {
                sb.Append($" CanceledOn={CanceledOn}");
            }
            return sb.ToString();
        }
    }

    // Job in ActivityStream
    public sealed record JobTemplateJobSummary(ulong Id, string Name, string Description,
                                               JobStatus Status, bool Failed, double Elapsed)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Job, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status} Elapsed={Elapsed}";
        }
    }

    // Job in WorkflowJobNode
    public sealed record WorkflowJobNodeJobSummary(ulong Id, string Name, string Description, JobStatus Status,
                                                   bool Failed, double Elapsed, ResourceType Type)
        : NameAndDescriptionResourceSummary(Id, Type, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status} Elapsed={Elapsed}";
        }
    }

    // LastJob in Host
    public sealed record HostLastJobSummary(ulong Id, string Name, string Description, JobStatus Status,
                                            bool Failed, double Elapsed, ulong JobTemplateId, string JobTemplateName)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Job, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status} Elapsed={Elapsed} Template=[{ResourceType.JobTemplate}:{JobTemplateId}]{JobTemplateName}";
        }
    }

    // Job in JobEvent, JobHostSummary
    public sealed record JobExSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed,
                                      double Elapsed, ResourceType Type, ulong JobTemplateId, string JobTemplateName)
        : NameAndDescriptionResourceSummary(Id, Type, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status} Elapsed={Elapsed} Template=[{ResourceType.JobTemplate}:{JobTemplateId}]{JobTemplateName}";
        }
    }

    // SourceWorkflowJob in Job, WorkflowApproval
    public sealed record SourceWorkflowJobSummary(ulong Id, string Name, string Description, JobStatus Status,
                                           bool Failed, double Elapsed)
        : NameAndDescriptionResourceSummary(Id, ResourceType.WorkflowJob, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status} Elapsed={Elapsed}";
        }
    }

    // AncestorJob in Job
    public sealed record AncestorJobSummary(ulong Id, string Name, ResourceType Type, string Url)
        : NamedResourceSummary(Id, Type, Name);

    // LastJobHostSummary in Host
    public sealed record LastJobHostSummary(ulong Id, bool Failed)
        : ResourceSummary(Id, ResourceType.JobHostSummary)
    {
        public override string GetDescription()
        {
            return $"Failed={Failed}";
        }
    }

    // LastUpdate in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public sealed record LastUpdateSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed)
        : SummaryBase;

    // Inventory in AdHocCommand, Group, Host, InventorySource, InventoryUpdate, JobTemplate, Job, Schedule, WorkflowJobTemplate
    public sealed record InventorySummary(ulong Id, string Name, string Description, bool HasActiveFailures, int TotalHosts,
                                   int HostsWithActiveFailures, int TotalGroups, bool HasInventorySources,
                                   int TotalInventorySources, int InventorySourcesWithFailures, ulong OrganizationId, string Kind)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Inventory, Name, Description)
    {
        public override string GetDescription()
        {
            return string.IsNullOrEmpty(Kind)
                   ? base.GetDescription()
                   : $"{base.GetDescription()} [{Kind}]";
        }
    };

    // InventorySource in InventoryUpdate
    public sealed record InventorySourceSummary(ulong Id, string Name, InventorySourceSource Source, DateTime LastUpdated, JobStatus Status)
        : NamedResourceSummary(Id, ResourceType.InventorySource, Name)
    {
        public override string GetDescription()
        {
            return $"[{Source}] {Name} Status={Status} LastUpdated={LastUpdated}";
        }
    };

    // SourceProject in InventorySource, InventoryUpdate
    // Project in JobTemplate, Job, ProjectUpdate
    public sealed record ProjectSummary(ulong Id, string Name, string Description, JobTemplateStatus Status,
                                 string ScmType, bool AllowOverride)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Project, Name, Description)
    {
        public override string GetDescription()
        {
            return string.IsNullOrEmpty(ScmType)
                ? $"[Local] {base.GetDescription()}"
                : $"[{ScmType}] {base.GetDescription()}";
        }
    }

    // ProjectUpdate in ProjectUpdateJobEvent
    public sealed record ProjectUpdateSummary(ulong Id, string Name, string Description, JobStatus Status, bool Failed)
        : NameAndDescriptionResourceSummary(Id, ResourceType.ProjectUpdate, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} Status={Status}";
        }
    }

    // UnifiedJobTemplate in InventoryUpdate, Job, ProjectUpdate, Schedule, SystemJob, WorkflowApproval, WorkflowJob,
    //                       WorkflowJobNode, WorkflowJobTemplateNode
    public sealed record UnifiedJobTemplateSummary(ulong Id, string Name, string Description, ResourceType UnifiedJobType)
        : NameAndDescriptionResourceSummary(Id, UnifiedJobType switch
        {
            ResourceType.Job => ResourceType.JobTemplate,
            ResourceType.ProjectUpdate => ResourceType.Project,
            ResourceType.InventoryUpdate => ResourceType.InventorySource,
            ResourceType.WorkflowJob => ResourceType.WorkflowJobTemplate,
            ResourceType.SystemJob => ResourceType.SystemJobTemplate,
            ResourceType.WorkflowApproval => ResourceType.WorkflowApprovalTemplate,
            _ => ResourceType.None
        }, Name, Description);

    // InstanceGroup in AdHocCommand, InventoryUpdate, Job, ProjectUpdate, SystemJob
    public sealed record InstanceGroupSummary(ulong Id, string Name, bool IsContainerGroup)
        : NamedResourceSummary(Id, ResourceType.InstanceGroup, Name)
    {
        public override string GetDescription()
        {
            return $"{Name} IsContainerGroup={IsContainerGroup}";
        }
    }

    // Owners in Credential
    public sealed record OwnerSummary(ulong Id, ResourceType Type, string Name, string Description, string Url)
        : NameAndDescriptionResourceSummary(Id, Type, Name, Description);

    // Schedule in InventoryUpdate, Job, ProjectUpdate, SystemJob, WorkflowJob
    public sealed record ScheduleSummary(ulong Id, string Name, string Description, DateTime NextRun)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Schedule, Name, Description)
    {
        public override string GetDescription()
        {
            return $"{base.GetDescription()} NextRun={NextRun}";
        }
    }

    // RecentNotification in NotificationTemplate
    public sealed record RecentNotificationSummary(ulong Id, JobStatus Status, DateTime Created, string Error)
        : ResourceSummary(Id, ResourceType.Notification), ICacheableResource
    {
        public override string GetDescription()
        {
            return string.IsNullOrEmpty(Error)
                   ? $"Status={Status} Created={Created}"
                   : $"Status={Status} Created={Created} Error={Error}";
        }
    }

    // WorkflowApprovalTemplate in WorkflowApproval
    public sealed record WorkflowApprovalTemplateSummary(ulong Id, string Name, string Description, int Timeout)
        : NameAndDescriptionResourceSummary(Id, ResourceType.WorkflowApprovalTemplate, Name, Description);

    public sealed record AdHocCommandSummary(ulong Id, string Name, JobStatus Status, string Limit)
        : NamedResourceSummary(Id, ResourceType.AdHocCommand, Name)
    {
        public override string GetDescription()
        {
            return $"{Name} Status={Status} Limit={Limit}";
        }
    }
;

    public sealed record InstanceSummary(ulong Id, string Hostname)
        : ResourceSummary(Id, ResourceType.Instance), ICacheableResource
    {
        public override string GetDescription()
        {
            return Hostname;
        }
    }

    public sealed record NotificationSummary(ulong Id, JobStatus Status, string NotificationType, ulong NotificationTemplateId)
        : ResourceSummary(Id, ResourceType.Notification), ICacheableResource
    {
        public override string GetDescription()
        {
            return $"{NotificationType} Status={Status}";
        }
    }

    public sealed record RoleSummary(ulong Id, string RoleField)
        : ResourceSummary(Id, ResourceType.Role), ICacheableResource
    {
        public override string GetDescription()
        {
            return $"RoleField={RoleField}";
        }
    }

    public sealed record SettingSummary(string Name, string Category)
        : SummaryBase;

    public sealed record SurveySummary(string Title, string Description)
        : SummaryBase;

    public sealed record TeamSummary(ulong Id, string Name, string Description)
        : NameAndDescriptionResourceSummary(Id, ResourceType.Team, Name, Description);

    public sealed record WorkflowJobTemplateNodeSummary(ulong Id, ulong UnifiedJobTemplateId)
        : ResourceSummary(Id, ResourceType.WorkflowJobTemplateNode), ICacheableResource
    {
        public override string GetDescription()
        {
            return $"UnifiedJobTemplateId={UnifiedJobTemplateId}";
        }
    }
}
