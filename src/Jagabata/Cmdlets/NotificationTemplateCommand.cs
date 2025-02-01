using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "NotificationTemplate")]
    [OutputType(typeof(NotificationTemplate))]
    public class GetNotificationTemplateCommand : GetCommandBase<NotificationTemplate>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.NotificationTemplate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.NotificationTemplate)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "NotificationTemplate")]
    [OutputType(typeof(NotificationTemplate))]
    public class FindNotificationTemplateCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Organization)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Organization)]
        public ulong Organization { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "notification_type", "messages")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Organization > 0 ? $"{Resources.Organization.PATH}{Organization}/notification_templates/" : NotificationTemplate.PATH;
            foreach (var resultSet in GetResultSet<NotificationTemplate>(path, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.Find, "NotificationTemplateForApproval")]
    [OutputType(typeof(NotificationTemplate))]
    public class FindNotificationTemplateForApprovalCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.Organization, ResourceType.WorkflowJobTemplate])]
        [ResourceCompletions(ResourceType.Organization, ResourceType.WorkflowJobTemplate)]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "notification_type", "messages")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/notification_templates_approvals/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/notification_templates_approvals/",
                _ => throw new ArgumentException()
            };
            Find<NotificationTemplate>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "NotificationTemplateForError")]
    [OutputType(typeof(NotificationTemplate))]
    public class FindNotificationTemplateForErrorCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        ])]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        )]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "notification_type", "messages")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/notification_templates_error/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/notification_templates_error/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/notification_templates_error/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/notification_templates_error/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{Resource.Id}/notification_templates_error/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/notification_templates_error/",
                _ => throw new ArgumentException()
            };
            Find<NotificationTemplate>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "NotificationTemplateForStarted")]
    [OutputType(typeof(NotificationTemplate))]
    public class FindNotificationTemplateForStartedCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        ])]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        )]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "notification_type", "messages")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/notification_templates_started/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/notification_templates_started/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/notification_templates_started/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/notification_templates_started/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{Resource.Id}/notification_templates_started/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/notification_templates_started/",
                _ => throw new ArgumentException()
            };
            Find<NotificationTemplate>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "NotificationTemplateForSuccess")]
    [OutputType(typeof(NotificationTemplate))]
    public class FindNotificationTemplateForSuccessCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        ])]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.Project, ResourceType.InventorySource, ResourceType.JobTemplate,
            ResourceType.SystemJobTemplate, ResourceType.WorkflowJobTemplate
        )]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "organization",
                           "notification_type", "messages")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/notification_templates_success/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/notification_templates_success/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Resource.Id}/notification_templates_success/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/notification_templates_success/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{Resource.Id}/notification_templates_success/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/notification_templates_success/",
                _ => throw new ArgumentException()
            };
            Find<NotificationTemplate>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "NotificationTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(NotificationTemplate))]
    public class NewNotificationTemplateCommand : NewCommandBase<NotificationTemplate>
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true)]
        [ResourceIdTransformation(ResourceType.Organization)]
        public ulong Organization { get; set; }

        [Parameter(Mandatory = true)]
        public NotificationType Type { get; set; }

        [Parameter()]
        public IDictionary Configuration { get; set; } = new Hashtable();

        [Parameter()]
        public IDictionary Messages { get; set; } = new Hashtable();

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "organization", Organization },
                { "notification_type", $"{Type}".ToLowerInvariant() },
                { "notification_configuration", Configuration },
                { "messages", Messages }
            };
            if (Description is not null)
                sendData.Add("description", Description);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsData.Update, "NotificationTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(NotificationTemplate))]
    public class UpdateNotificationTemplateCommand : UpdateCommandBase<NotificationTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.NotificationTemplate)]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.Organization)]
        public ulong? Organization { get; set; }

        [Parameter()]
        public NotificationType? Type { get; set; }

        [Parameter()]
        public IDictionary? Configuration { get; set; }

        [Parameter()]
        public IDictionary? Messages { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Organization is not null)
                sendData.Add("organization", Organization);
            if (Type is not null)
                sendData.Add("notification_type", $"{Type}".ToLowerInvariant());
            if (Configuration is not null)
                sendData.Add("notification_configuration", Configuration);
            if (Messages is not null)
                sendData.Add("messages", Messages.Count == 0 ? null : Messages);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "NotificationTemplate", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveNotificationTemplateCommand : RemoveCommandBase<NotificationTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.NotificationTemplate)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsLifecycle.Enable, "NotificationTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class EnableNotificationTemplateCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.NotificationTemplate)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Project,
                ResourceType.InventorySource,
                ResourceType.JobTemplate,
                ResourceType.SystemJobTemplate,
                ResourceType.WorkflowJobTemplate
        ])]
        public IResource For { get; set; } = new Resource(0, 0);

        [Parameter(Mandatory = true, Position = 2)]
        [ValidateSet("Started", "Success", "Error", "Approval")]
        public string[] On { get; set; } = [];

        protected override void ProcessRecord()
        {
            var path1 = For.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{For.Id}/",
                ResourceType.Project => $"{Project.PATH}{For.Id}/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{For.Id}/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{For.Id}/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{For.Id}/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{For.Id}/",
                _ => throw new ArgumentException($"Invalid resource type: {For.Type}")
            };
            foreach (var timing in On)
            {
                if (timing == "Approval" && (For.Type != ResourceType.Organization
                                             && For.Type != ResourceType.WorkflowJobTemplate))
                {
                    WriteWarning($"{For.Type} has no \"{timing}\" notifications.");
                    continue;
                }
                var path2 = timing switch
                {
                    "Started" => "notification_templates_started/",
                    "Success" => "notification_templates_success/",
                    "Error" => "notification_templates_error/",
                    "Approval" => "notification_templates_approvals/",
                    _ => throw new ArgumentException($"(Invalid timing value: {timing}")
                };
                if (ShouldProcess($"NotificationTemplate [{Id}]", $"Enable to {For.Type} [{For.Id}] on {timing}"))
                {
                    var path = path1 + path2;
                    var sendData = new Dictionary<string, object>() { { "id", Id } };
                    var apiResult = CreateResource<string>(path, sendData);
                    if (apiResult.Response.IsSuccessStatusCode)
                    {
                        WriteVerbose($"NotificationTemplate {Id} is enabled to {For.Type} [{For.Id}] on {timing}.");
                    }
                }
            }
        }
    }

    [Cmdlet(VerbsLifecycle.Disable, "NotificationTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class DisableNotificationTemplateCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.NotificationTemplate)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Project,
                ResourceType.InventorySource,
                ResourceType.JobTemplate,
                ResourceType.SystemJobTemplate,
                ResourceType.WorkflowJobTemplate
        ])]
        public IResource For { get; set; } = new Resource(0, 0);

        [Parameter(Mandatory = true, Position = 2)]
        [ValidateSet("Started", "Success", "Error", "Approval")]
        public string[] On { get; set; } = [];

        protected override void ProcessRecord()
        {
            var path1 = For.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{For.Id}/",
                ResourceType.Project => $"{Project.PATH}{For.Id}/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{For.Id}/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{For.Id}/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{For.Id}/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{For.Id}/",
                _ => throw new ArgumentException($"Invalid resource type: {For.Type}")
            };
            foreach (var timing in On)
            {
                if (timing == "Approval" && (For.Type != ResourceType.Organization
                                             && For.Type != ResourceType.WorkflowJobTemplate))
                {
                    WriteWarning($"{For.Type} has no \"{timing}\" notifications.");
                    continue;
                }
                var path2 = timing switch
                {
                    "Started" => "notification_templates_started/",
                    "Success" => "notification_templates_success/",
                    "Error" => "notification_templates_error/",
                    "Approval" => "notification_templates_approvals/",
                    _ => throw new ArgumentException($"(Invalid timing value: {timing}")
                };
                if (ShouldProcess($"NotificationTemplate [{Id}]", $"Disable to {For.Type} [{For.Id}] on {timing}"))
                {
                    var path = path1 + path2;
                    var sendData = new Dictionary<string, object>()
                    {
                        { "id", Id },
                        { "disassociate", true }
                    };
                    var apiResult = CreateResource<string>(path, sendData);
                    if (apiResult.Response.IsSuccessStatusCode)
                    {
                        WriteVerbose($"NotificationTemplate {Id} is disabled to {For.Type} [{For.Id}] on {timing}.");
                    }
                }
            }
        }
    }
}
