using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "Job")]
    [OutputType(typeof(JobTemplateJob.Detail))]
    public class GetJobTemplateJobCommand : GetCommandBase<JobTemplateJob.Detail>
    {
        protected override ResourceType AcceptType => ResourceType.Job;

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Job", DefaultParameterSetName = "All")]
    [OutputType(typeof(JobTemplateJob))]
    public class FindJobTemplateJobCommand : FindCommandBase
    {
        [Parameter(ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.JobTemplate))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public ulong Id { get; set; }

        [Parameter(Position = 0)]
        public string[]? Name { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobLaunchType>))]
        public string[]? LaunchType { get; set; }

        [Parameter()]
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
            var path = Id > 0 ? $"{JobTemplate.PATH}{Id}/jobs/" : JobTemplateJob.PATH;
            Find<JobTemplateJob>(path);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "Job", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveJobTemplateJobCommand : RemoveCommandBase<JobTemplateJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Job])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
