using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Schedule")]
    [OutputType(typeof(Resources.Schedule))]
    public class GetScheduleCommand : GetCommandBase<Resources.Schedule>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Schedule])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Schedule)]
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

    [Cmdlet(VerbsCommon.Find, "Schedule", DefaultParameterSetName = "All")]
    [OutputType(typeof(Resources.Schedule))]
    public class FindScheduleCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Project),
                     nameof(ResourceType.InventorySource),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.SystemJobTemplate),
                     nameof(ResourceType.WorkflowJobTemplate))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Project,
                ResourceType.InventorySource,
                ResourceType.JobTemplate,
                ResourceType.SystemJobTemplate,
                ResourceType.WorkflowJobTemplate
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("rrule", "id", "created", "modified", "name", "description", "extra_data", "inventory",
                           "execution_environment", "unified_job_template", "enabled", "dtstart", "dtend", "next_run",
                           "created_by", "modified_by", "credentials", "instance_groups", "labels")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.Project => $"{Project.PATH}{Id}/schedules/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Id}/schedules/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/schedules/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{Id}/schedules/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Id}/schedules/",
                _ => Resources.Schedule.PATH
            };
            base.Find<Resources.Schedule>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Schedule", SupportsShouldProcess = true)]
    [OutputType(typeof(Resources.Schedule))]
    public class NewScheduleCommand : NewCommandBase<Resources.Schedule>
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true)]
        public string RRule { get; set; } = string.Empty;

        [Parameter()]
        public SwitchParameter Disabled { get; set; }

        [Parameter(Mandatory = true)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Project,
                ResourceType.InventorySource,
                ResourceType.JobTemplate,
                ResourceType.SystemJobTemplate,
                ResourceType.WorkflowJobTemplate
        ])]
        public IResource Template { get; set; } = new Resource(0, 0);

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation] // Translate IDictionary to JSON string
        public string? ExtraData { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ValidateSet(nameof(Resources.JobType.Run), nameof(Resources.JobType.Check))]
        public JobType? JobType { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Tags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SkipTags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        public bool? DiffMode { get; set; }

        [Parameter()]
        public JobVerbosity? Verbosity { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Forks { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? JobSliceCount { get; set; }

        [Parameter()]
        public int? Timeout { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var dict = new Dictionary<string, object>()
            {
                { "name", Name },
                { "rrule", RRule }
            };
            if (Description is not null)
                dict.Add("description", Description);
            if (Disabled)
                dict.Add("enabled", false);
            if (ExtraData is not null)
                dict.Add("extra_data", ExtraData);
            if (Inventory is not null)
                dict.Add("inventory", Inventory);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (JobType is not null)
                dict.Add("job_type", $"{JobType}".ToLowerInvariant());
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (DiffMode is not null)
                dict.Add("diff_mode", DiffMode);
            if (Verbosity is not null)
                dict.Add("verbosity", (int)Verbosity);
            if (Forks is not null)
                dict.Add("forks", Forks);
            if (ExecutionEnvironment is not null)
                dict.Add("execution_environment", ExecutionEnvironment);
            if (JobSliceCount is not null)
                dict.Add("job_slice_count", JobSliceCount);
            if (Timeout is not null)
                dict.Add("timeout", Timeout);

            return dict;
        }

        protected override void ProcessRecord()
        {
            var path = Template.Type switch
            {
                ResourceType.Project => $"{Project.PATH}{Template.Id}/schedules/",
                ResourceType.InventorySource => $"{InventorySource.PATH}{Template.Id}/schedules/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Template.Id}/schedules/",
                ResourceType.SystemJobTemplate => $"{SystemJobTemplate.PATH}{Template.Id}/schedules/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Template.Id}/schedules/",
                _ => throw new ArgumentException("Invalid type")
            };
            if (TryCreate(path, out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsData.Update, "Schedule", SupportsShouldProcess = true)]
    [OutputType(typeof(Resources.Schedule))]
    public class UpdateScheduleCommand : UpdateCommandBase<Resources.Schedule>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Schedule])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public string RRule { get; set; } = string.Empty;

        [Parameter()]
        public bool? Enable { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation] // Translate IDictionary to JSON string
        public string? ExtraData { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ValidateSet(nameof(Resources.JobType.Run), nameof(Resources.JobType.Check))]
        public JobType? JobType { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Tags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SkipTags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        public bool? DiffMode { get; set; }

        [Parameter()]
        public JobVerbosity? Verbosity { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Forks { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? JobSliceCount { get; set; }

        [Parameter()]
        public int? Timeout { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var dict = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                dict.Add("name", Name);
            if (Description is not null)
                dict.Add("description", Description);
            if (!string.IsNullOrEmpty(RRule))
                dict.Add("rrule", RRule);
            if (Enable is not null)
                dict.Add("enabled", Enable);
            if (ExtraData is not null)
                dict.Add("extra_data", ExtraData);
            if (Inventory is not null)
                dict.Add("inventory", Inventory);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (JobType is not null)
                dict.Add("job_type", $"{JobType}".ToLowerInvariant());
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (DiffMode is not null)
                dict.Add("diff_mode", DiffMode);
            if (Verbosity is not null)
                dict.Add("verbosity", (int)Verbosity);
            if (Forks is not null)
                dict.Add("forks", Forks);
            if (ExecutionEnvironment is not null)
                dict.Add("execution_environment", ExecutionEnvironment);
            if (JobSliceCount is not null)
                dict.Add("job_slice_count", JobSliceCount);
            if (Timeout is not null)
                dict.Add("timeout", Timeout);

            return dict;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Schedule", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveScheduleCommand : RemoveCommandBase<Resources.Schedule>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Schedule])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsCommon.Show, "Schedule")]
    [OutputType(typeof(SchedulePreview))]
    public class TestSchedule : APICmdletBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "RRule", ValueFromPipeline = true, Position = 0)]
        public string? RRule { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Schedule", ValueFromPipeline = true, Position = 0)]
        public Resources.Schedule? Schedule { get; set; }

        protected override void ProcessRecord()
        {
            var sendData = new Dictionary<string, string>();
            if (RRule is not null)
                sendData.Add("rrule", RRule);
            else if (Schedule is not null)
                sendData.Add("rrule", Schedule.Rrule);

            var res = CreateResource<SchedulePreview>(SchedulePreview.PATH, sendData);
            WriteObject(res.Contents, false);
        }
    }
}
