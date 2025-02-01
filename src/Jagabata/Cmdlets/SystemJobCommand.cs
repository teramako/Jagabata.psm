using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "SystemJob")]
    [OutputType(typeof(SystemJob.Detail))]
    public class GetSystemJobCommand : GetCommandBase<SystemJob.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.SystemJob])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.SystemJob)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "SystemJob")]
    [OutputType(typeof(SystemJob))]
    public class FindSystemJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.SystemJobTemplate])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.SystemJobTemplate)]
        public ulong SystemJobTemplate { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "unified_job_template",
                           "launch_type", "status", "execution_environment", "failed", "started", "finished",
                           "canceled_on", "elapsed", "job_explanation", "execution_node", "work_unit_id",
                           "system_job_template", "job_type", "schedule", "notifications", "created_by",
                           "modified_by", "dependent_jobs", "labels", "instance_group")]
        public override string[] OrderBy { get; set; } = ["!id"];


        protected override void BeginProcessing()
        {
            if (Status is not null)
            {
                Query.Add("status__in", string.Join(',', Status));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = SystemJobTemplate > 0 ? $"{Resources.SystemJobTemplate.PATH}{SystemJobTemplate}/jobs/" : SystemJob.PATH;
            Find<SystemJob>(path);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "SystemJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveSystemJobCommand : RemoveCommandBase<SystemJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.SystemJob])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
