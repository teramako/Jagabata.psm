using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowJob")]
    [OutputType(typeof(WorkflowJob.Detail))]
    public class GetWorkflowJobCommand : GetCommandBase<WorkflowJob.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJob])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowJob)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "WorkflowJob")]
    [OutputType(typeof(WorkflowJob))]
    public class FindWorkflowJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate])]
        [ResourceCompletions(ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate)]
        public IResource? Resource { get; set; }

        [Parameter()]
        public string[]? Name { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobLaunchType>))]
        public string[]? LaunchType { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "created", "modified", "name", "description", "unified_job_template",
                                   "launch_type", "status", "failed", "started", "finished", "canceled_on",
                                   "elapsed", "job_explanation", "work_unit_id", "workflow_job_template",
                                   "allow_simultaneous", "job_template", "is_sliced_job", "inventory",
                                   "webhook_service", "webhook_credential", "webhook_guid", "notifications",
                                   "unified_job_node", "workflow_job_template", "organization", "schedule",
                                   "created_by", "modified_by", "instance_group", "labels"])]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            if (Name is not null)
            {
                Query.Add("name__in", string.Join(',', Name));
            }
            if (Status is not null)
            {
                Query.Add("status__in", string.Join(',', Status));
            }
            if (LaunchType is not null)
            {
                Query.Add("launch_type__in", string.Join(',', LaunchType));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/slice_workflow_jobs/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/workflow_jobs/",
                _ => WorkflowJob.PATH
            };
            Find<WorkflowJob>(path);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "WorkflowJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveWorkflowJobCommand : RemoveCommandBase<WorkflowJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJob])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
