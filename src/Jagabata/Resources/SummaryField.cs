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

    public abstract class SummaryBase
    {
        protected abstract string[] DisplayProperties { get; }
        protected bool PrintMembers([MaybeNullWhen(false)] out ReadOnlySpan<char> contents)
        {
            contents = default;
            if (DisplayProperties.Length == 0)
                return false;

            var sb = new StringBuilder();

            bool hasContents = false;
            var t = GetType();
            foreach (var prop in DisplayProperties.Select(t.GetProperty))
            {
                if (prop is null) continue;
                var val = prop.GetValue(this);
                if (val is not null)
                {
                    if (val is string and "")
                        continue;

                    if (hasContents)
                        sb.Append(", ");

                    sb.Append(System.Globalization.CultureInfo.InvariantCulture,
                              $"{prop.Name} = {val}");
                    hasContents = true;
                }
            }
            if (hasContents)
                contents = sb.ToString();
            return hasContents;
        }
        public override string ToString()
        {
            return PrintMembers(out var contents) ? $"{{ {contents} }}" : "{ … }";
        }
    }

    public abstract class ResourceSummary
        : SummaryBase, IResource, ICacheableResource
    {
        public abstract ResourceType Type { get; }
        public abstract ulong Id { get; }
        protected override string[] DisplayProperties => [];
        public override string ToString()
        {
            return PrintMembers(out var contents)
                ? $"{Type}:{Id} {{ {contents} }}"
                : $"{Type}:{Id}";
        }
        protected virtual CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, string.Empty, CacheType.Summary);
        }
        CacheItem ICacheableResource.GetCacheItem()
        {
            return GetCacheItem();
        }
    }

    public abstract class NamedResourceSummary
        : ResourceSummary, IResource, ICacheableResource
    {
        public abstract string Name { get; }
        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, string.Empty, CacheType.Summary);
        }
        public sealed override string ToString()
        {
            return PrintMembers(out var contents)
                ? $"{Type}:{Id}:{Name} {{ {contents} }}"
                : $"{Type}:{Id}:{Name}";
        }
    }

    public abstract class NameAndDescriptionResourceSummary
        : NamedResourceSummary, IResource, ICacheableResource
    {
        public abstract string Description { get; }
        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description, CacheType.Summary);
        }
        protected override string[] DisplayProperties => [nameof(Description)];
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
        Unattach = 1 << 7,
    }

    // Application in Token
    public sealed class ApplicationSummary(ulong id, string name) : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.OAuth2Application;
        public override ulong Id => id;
        public override string Name => name;
    }

    // List<Group> in Host
    public sealed class GroupSummary(ulong id, string name) : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Group;
        public override ulong Id => id;
        public override string Name => name;
    }

    // List<Label> in Inventory, Job, JobTemplate, WorkflowJob, WorkflowJobTemplate
    public sealed class LabelSummary(ulong id, string name) : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Label;
        public override ulong Id => id;
        public override string Name => name;

    }

    // Host in AdHocCommandJobEvent, JobEvent, JobHostSummary
    public sealed class HostSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Host;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;

    }

    // Organization in Application, Credential, ExecutionEnvironment, Inventory, InventorySource, InventoryUpdate,
    //                 JobTemplate, Job, Label, NotificationTemplate, Project, ProjectUpdate, Team, WorkflowJobtemplate
    public sealed class OrganizationSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Organization;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // CredentialType in Credential
    public sealed class CredentialTypeSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.CredentialType;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // ObjectRoles in Credential, InstanceGroup, Inventory, JobTemplate, Project, Team, WorkflowJobTemplate
    public sealed class ObjectRoleSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Role;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // JobTemplate in Job
    public sealed class JobTemplateSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.JobTemplate;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // NotificationTemplate in Notification
    public sealed class NotificationTemplateSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.NotificationTemplate;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // WorkflowJobTemplate in WorkflowApproval, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplateNode
    public sealed class WorkflowJobTemplateSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.WorkflowJobTemplateNode;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // WorkflowJob in WorkflowApproval, WorkflowJobNode
    public sealed class WorkflowJobSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.WorkflowJob;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    // Actor in ActivityStream
    // CreatedBy in AdHocCommand, Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory,
    //              InventorySource, InventoryUpdate, JobTemplate, Job, Label, NotificationTemplate, Organization,
    //              Project, Schedule, Team, WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // ModifiedBy in Credential, CredentialInputSource, ExecutionEnvironment, Group, Inventory InventorySource,
    //               JobTemplate, Label, NotificationTemplate, Organization, Project, Schedule, Team,
    //               WorkflowApprovalTemplate, WorkflowJob, WorkflowJobTemplate
    // User in Token
    // ApprovedOrDeniedBy in WorkflowApproval
    public sealed class UserSummary(ulong id, string username, string firstName, string lastName)
        : ResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.User;
        public override ulong Id => id;
        public string Username => username;
        public string FirstName => firstName;
        public string LastName => lastName;
        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Username, string.Empty, CacheType.Summary);
        }
        public override string ToString()
        {
            return $"{Type}:{Id}:{Username}";
        }
    }

    // ObjectRoles in Organization
    public sealed class OrganizationObjectRoleSummary(ulong id, string name, string description, bool userOnly = false)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Role;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public bool UserOnly => userOnly;
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(UserOnly)];
    }

    // ExecutionEnvironment in AdHocCommand, InventorySource, InventoryUpdate, JobTemplate, Job, SystemJob
    // DefaultEnvironment in Organization, Project, ProjectUpdate
    // ResolvedEnvironment in SystemJobTemplate, WorkflowApprovalTemplate
    public sealed class EnvironmentSummary(ulong id, string name, string description, string image)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.ExecutionEnvironment;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public string Image => image;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Image", Image);
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Image)];
    }

    // RelatedFieldCounts in Organization
    public sealed class RelatedFieldCountsSummary(int inventories, int teams, int users, int jobTemplates, int admins,
                                                  int projects)
        : SummaryBase
    {
        public int Inventories => inventories;
        public int Teams => teams;
        public int Users => users;
        public int JobTemplates => jobTemplates;
        public int Admins => admins;
        public int Projects => projects;
        protected override string[] DisplayProperties =>
        [
            nameof(Inventories),
            nameof(Teams),
            nameof(Users),
            nameof(JobTemplates),
            nameof(Admins),
            nameof(Projects)
        ];
    }

    // List<Token> in Application
    public sealed class TokenSummary(ulong id, string token, string scope)
        : ResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.OAuth2AccessToken;
        public override ulong Id => id;
        public string Token => token;
        public string Scope => scope;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Scope", Scope);
            return item;
        }
        protected override string[] DisplayProperties => [nameof(Scope), nameof(Token)];
    }

    public class ListSummary<T>(int count, T[] results)
    {
        public int Count => count;
        public T[] Results => results;
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
    public sealed class CredentialSummary(ulong id, string name, string description, string kind, bool cloud = false,
                                          bool kubernetes = false, ulong? credentialTypeId = null)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Credential;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public string Kind => kind;
        public bool Cloud => cloud;
        public bool Kubernetes => kubernetes;
        public ulong? CredentialTypeId => credentialTypeId;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Kind", Kind);
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(Kind),
            nameof(Cloud),
            nameof(Kubernetes),
            nameof(CredentialTypeId)
        ];
    }

    // Credentials in JobTemplate, Job
    public sealed class JobTemplateCredentialSummary(ulong id, string name, string description, string kind, bool cloud)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Credential;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public string Kind => kind;
        public bool Cloud => cloud;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Kind", Kind);
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Kind), nameof(Cloud)];
    }

    // LastJob in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public sealed class LastJobSummary(ulong id, string name, string description, DateTime? finished,
                                       JobStatus status, bool failed)
        : SummaryBase
    {
        public ulong Id => id;
        public string Name => name;
        public string Description => description;
        public DateTime? Finished => finished;
        public JobStatus Status => status;
        public bool Failed => failed;
        protected override string[] DisplayProperties =>
        [
            nameof(Id),
            nameof(Name),
            nameof(Description),
            nameof(Status),
            nameof(Finished),
        ];
    }

    // RecentJobs in Host
    public sealed class HostRecentJobSummary(ulong id, string name, ResourceType type, JobStatus status,
                                             DateTime? finished)
        : NamedResourceSummary
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public override string Name => name;
        public JobStatus Status => status;
        public DateTime? Finished => finished;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            if (Finished is not null)
            {
                item.Metadata.Add("Fnished", $"{Finished}");
            }
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(Finished)];
    }

    // RecentJobs in JobTemplate, WorkflowJobTemplate
    public sealed class RecentJobSummary(ulong id, JobStatus status, DateTime? finished,
                                          DateTime? canceledOn, ResourceType type)
        : ResourceSummary, ICacheableResource
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public JobStatus Status => status;
        public DateTime? Finished => finished;
        public DateTime? CanceledOn => canceledOn;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Fnished", $"{Finished}");
            item.Metadata.Add("CanceledOn", $"{CanceledOn}");
            return item;
        }
        protected override string[] DisplayProperties => [nameof(Status), nameof(Finished), nameof(CanceledOn)];
    }

    // Job in ActivityStream
    public sealed class JobTemplateJobSummary(ulong id, string name, string description, JobStatus status, bool failed,
                                              double elapsed)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Job;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        public double Elapsed => elapsed;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Elapsed", $"{Elapsed}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(Elapsed)];
    }

    // Job in WorkflowJobNode
    public sealed class WorkflowJobNodeJobSummary(ulong id, string name, string description, JobStatus status,
                                                  bool failed, double elapsed, ResourceType type)
        : NameAndDescriptionResourceSummary
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        public double Elapsed => elapsed;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Elapsed", $"{Elapsed}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(Elapsed)];
    }

    // LastJob in Host
    public sealed class HostLastJobSummary(ulong id, string name, string description, JobStatus status, bool failed,
                                           double elapsed, ulong jobTemplateId, string jobTemplateName)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Job;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        public double Elapsed => elapsed;
        public ulong JobTemplateId => jobTemplateId;
        public string JobTemplateName => jobTemplateName;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Elapsed", $"{Elapsed}");
            item.Metadata.Add("Template", $"[{ResourceType.JobTemplate}:{JobTemplateId}] {JobTemplateName}");
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(Status),
            nameof(Elapsed),
            nameof(JobTemplateId),
            nameof(JobTemplateName)
        ];
    }

    // Job in JobEvent, JobHostSummary
    public sealed class JobExSummary(ulong id, string name, string description, JobStatus status, bool failed,
                                     double elapsed, ResourceType type, ulong jobTemplateId, string jobTemplateName)
        : NameAndDescriptionResourceSummary
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        public double Elapsed => elapsed;
        public ulong JobTemplateId => jobTemplateId;
        public string JobTemplateName => jobTemplateName;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Elapsed", $"{Elapsed}");
            item.Metadata.Add("Template", $"[{ResourceType.JobTemplate}:{JobTemplateId}] {JobTemplateName}");
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(Status),
            nameof(Elapsed),
            nameof(JobTemplateId),
            nameof(JobTemplateName)
        ];
    }

    // SourceWorkflowJob in Job, WorkflowApproval
    public sealed class SourceWorkflowJobSummary(ulong id, string name, string description, JobStatus status,
                                                 bool failed, double elapsed)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.WorkflowJob;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        public double Elapsed => elapsed;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Elapsed", $"{Elapsed}");
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(Status),
            nameof(Elapsed)
        ];
    }

    // AncestorJob in Job
    public sealed class AncestorJobSummary(ulong id, string name, ResourceType type, string url)
        : NamedResourceSummary
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public override string Name => name;
        public string Url => url;
    }

    // LastJobHostSummary in Host
    public sealed class LastJobHostSummary(ulong id, bool failed)
        : ResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.JobHostSummary;
        public override ulong Id => id;
        public bool Failed => failed;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Failed", $"{Failed}");
            return item;
        }
        protected override string[] DisplayProperties => [nameof(Failed)];
    }

    // LastUpdate in InventorySource, JobTemplate, Project, SystemJobTemplate, WorkflowApprovalTemplate, WorkflowJobTemplate
    public sealed class LastUpdateSummary(ulong id, string name, string description, JobStatus status, bool failed)
        : SummaryBase
    {
        public ulong Id => id;
        public string Name => name;
        public string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        protected override string[] DisplayProperties => [nameof(Id), nameof(Name), nameof(Description), nameof(Status)];
    }

    // Inventory in AdHocCommand, Group, Host, InventorySource, InventoryUpdate, JobTemplate, Job, Schedule, WorkflowJobTemplate
    public sealed class InventorySummary(ulong id, string name, string description, bool hasActiveFailures,
                                         int totalHosts, int hostsWithActiveFailures, int totalGroups,
                                         bool hasInventorySources, int totalInventorySources,
                                         int inventorySourcesWithFailures, ulong organizationId, string kind)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Inventory;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public bool HasActiveFailures => hasActiveFailures;
        public int TotalHosts => totalHosts;
        public int HostsWithActiveFailures => hostsWithActiveFailures;
        public int TotalGroups => totalGroups;
        public bool HasInventorySources => hasInventorySources;
        public int TotalInventorySources => totalInventorySources;
        public int InventorySourcesWithFailures => inventorySourcesWithFailures;
        public ulong OrganizationId => organizationId;
        public string Kind => kind;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Kind", $"{Kind}");
            item.Metadata.Add("Organization", $"{OrganizationId}");
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(Kind),
            nameof(TotalHosts),
            nameof(TotalGroups),
            nameof(OrganizationId),
        ];
    };

    // InventorySource in InventoryUpdate
    public sealed class InventorySourceSummary(ulong id, string name, InventorySourceSource source, DateTime lastUpdated,
                                               JobStatus status)
        : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.InventorySource;
        public override ulong Id => id;
        public override string Name => name;
        public InventorySourceSource Source => source;
        public DateTime LastUpdated => lastUpdated;
        public JobStatus Status => status;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("LastUpdated", $"{LastUpdated}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Source), nameof(LastUpdated), nameof(Status)];
    };

    // SourceProject in InventorySource, InventoryUpdate
    // Project in JobTemplate, Job, ProjectUpdate
    public sealed class ProjectSummary(ulong id, string name, string description, JobTemplateStatus status,
                                       string scmType, bool allowOverride)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Project;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobTemplateStatus Status => status;
        public string ScmType => scmType;
        public bool AllowOverride => allowOverride;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("ScmType", string.IsNullOrEmpty(ScmType) ? "Local" : ScmType);
            item.Metadata.Add("Status", $"{Status}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(ScmType)];
    }

    // ProjectUpdate in ProjectUpdateJobEvent
    public sealed class ProjectUpdateSummary(ulong id, string name, string description, JobStatus status, bool failed)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.ProjectUpdate;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public JobStatus Status => status;
        public bool Failed => failed;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Failed", $"{Failed}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(Failed)];
    }

    // UnifiedJobTemplate in InventoryUpdate, Job, ProjectUpdate, Schedule, SystemJob, WorkflowApproval, WorkflowJob,
    //                       WorkflowJobNode, WorkflowJobTemplateNode
    public sealed class UnifiedJobTemplateSummary(ulong id, string name, string description, ResourceType unifiedJobType)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => unifiedJobType switch
        {
            ResourceType.Job => ResourceType.JobTemplate,
            ResourceType.ProjectUpdate => ResourceType.Project,
            ResourceType.InventoryUpdate => ResourceType.InventorySource,
            ResourceType.WorkflowJob => ResourceType.WorkflowJobTemplate,
            ResourceType.SystemJob => ResourceType.SystemJobTemplate,
            ResourceType.WorkflowApproval => ResourceType.WorkflowApprovalTemplate,
            _ => ResourceType.None
        };
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public ResourceType UnifiedJobType => unifiedJobType;
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(UnifiedJobType)];
    }

    // InstanceGroup in AdHocCommand, InventoryUpdate, Job, ProjectUpdate, SystemJob
    public sealed class InstanceGroupSummary(ulong id, string name, bool isContainerGroup)
        : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.InstanceGroup;
        public override ulong Id => id;
        public override string Name => name;
        public bool IsContainerGroup => isContainerGroup;
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(IsContainerGroup)];
    }

    // Owners in Credential
    public sealed class OwnerSummary(ulong id, ResourceType type, string name, string description, string url)
        : NameAndDescriptionResourceSummary
    {
        public override ResourceType Type => type;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public string Url => url;
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Url)];
    }

    // Schedule in InventoryUpdate, Job, ProjectUpdate, SystemJob, WorkflowJob
    public sealed class ScheduleSummary(ulong id, string name, string description, DateTime nextRun)
        : NameAndDescriptionResourceSummary
    {
        public override ResourceType Type => ResourceType.Schedule;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public DateTime NextRun => nextRun;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("NextRun", $"{NextRun}");
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(NextRun)];
    }

    // RecentNotification in NotificationTemplate
    public sealed class RecentNotificationSummary(ulong id, JobStatus status, DateTime created, string error)
        : ResourceSummary, ICacheableResource
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Notification;
        public override ulong Id => id;
        public JobStatus Status => status;
        public DateTime Created => created;
        public string Error => error;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Created", $"{Created}");
            item.Metadata.Add("Error", Error);
            return item;
        }
        protected override string[] DisplayProperties => [nameof(Status), nameof(Created), nameof(Error)];
    }

    // WorkflowApprovalTemplate in WorkflowApproval
    public sealed class WorkflowApprovalTemplateSummary(ulong id, string name, string description, int timeout)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.WorkflowApprovalTemplate;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public int Timeout => timeout;
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Timeout)];
    }

    public sealed class AdHocCommandSummary(ulong id, string name, JobStatus status, string limit)
        : NamedResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.AdHocCommand;
        public override ulong Id => id;
        public override string Name => name;
        public JobStatus Status => status;
        public string Limit => limit;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Limit", Limit);
            return item;
        }
        protected override string[] DisplayProperties => [.. base.DisplayProperties, nameof(Status), nameof(Limit)];
    }
;

    public sealed class InstanceSummary(ulong id, string hostname)
        : ResourceSummary, ICacheableResource
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Instance;
        public override ulong Id => id;
        public string Hostname { get; } = hostname;
        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Hostname, string.Empty, CacheType.Summary);
        }
        public override string ToString()
        {
            return $"{Type}:{Id}:{Hostname}";
        }
    }

    public sealed class NotificationSummary(ulong id, JobStatus status, string notificationType,
                                            ulong notificationTemplateId)
        : ResourceSummary, ICacheableResource
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Notification;
        public override ulong Id => id;
        public JobStatus Status => status;
        public string NotificationType => notificationType;
        public ulong NotificationTemplateId => notificationTemplateId;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Status", $"{Status}");
            item.Metadata.Add("Type", NotificationType);
            item.Metadata.Add("Template", $"[{ResourceType.NotificationTemplate}:{NotificationTemplateId}]");
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            nameof(Status),
            nameof(NotificationType),
            nameof(NotificationTemplateId)
        ];
    }

    public sealed class RoleSummary(ulong id, string roleField)
        : ResourceSummary, ICacheableResource
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Role;
        public override ulong Id => id;
        public string RoleField => roleField;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Field", RoleField);
            return item;
        }
        protected override string[] DisplayProperties => [nameof(RoleField)];
    }

    public sealed class SettingSummary(string name, string category)
        : SummaryBase
    {
        public string Name => name;
        public string Category => category;
        protected override string[] DisplayProperties => [nameof(Name), nameof(Category)];
    }

    public sealed class SurveySummary(string title, string description)
        : SummaryBase
    {
        public string Title => title;
        public string Description => description;
        protected override string[] DisplayProperties => [nameof(Title), nameof(Description)];
    }

    public sealed class TeamSummary(ulong id, string name, string description)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.Team;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
    }

    public sealed class WorkflowJobTemplateNodeSummary(ulong id, ulong unifiedJobTemplateId)
        : ResourceSummary, ICacheableResource
    {
        [JsonIgnore]
        public override ResourceType Type => ResourceType.WorkflowJobTemplateNode;
        public override ulong Id => id;
        public ulong UnifiedJobTemplateId => unifiedJobTemplateId;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            item.Metadata.Add("Template", $"{UnifiedJobTemplateId}");
            return item;
        }
        protected override string[] DisplayProperties => [nameof(UnifiedJobTemplateId)];
    }

    // DirectAccess, IndirectAccess in User (from /api/v2/*/access_list/)
    public sealed class AccessSummary(AccessRoleSummary role, string[] descendantRoles)
        : SummaryBase
    {
        public AccessRoleSummary Role => role;
        public string[] DescendantRoles => descendantRoles;
        protected override string[] DisplayProperties => [nameof(Role)];
    }

    public sealed class AccessRoleSummary(ulong id, string name, string description, string? resourceName,
                                          ResourceType? resourceType, RelatedDictionary? related,
                                          Capability userCapabilities)
        : NameAndDescriptionResourceSummary
    {
        [JsonIgnore]
        public override ResourceType Type => Jagabata.ResourceType.Role;
        public override ulong Id => id;
        public override string Name => name;
        public override string Description => description;
        public string? ResourceName => resourceName;
        public ResourceType? ResourceType => resourceType;
        public RelatedDictionary? Related => related;
        public Capability UserCapabilities => userCapabilities;
        protected override CacheItem GetCacheItem()
        {
            var item = base.GetCacheItem();
            if (ResourceType is not null)
            {
                item.Metadata.Add("Resource", $"[{ResourceType}] {ResourceName}");
            }
            return item;
        }
        protected override string[] DisplayProperties =>
        [
            .. base.DisplayProperties,
            nameof(ResourceName),
            nameof(ResourceType),
        ];
    }
}
