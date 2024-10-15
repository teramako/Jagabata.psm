using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "JobHostSummary")]
    [OutputType(typeof(JobHostSummary))]
    public class GetJobHostSummaryCommand : GetCommandBase<JobHostSummary>
    {
        protected override ResourceType AcceptType => ResourceType.JobHostSummary;

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "JobHostSummary")]
    [OutputType(typeof(JobHostSummary))]
    public class FindJobHostSummaryCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [ValidateSet(nameof(ResourceType.Job),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.Group))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        public ulong Id { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void EndProcessing()
        {
            var path = Type switch
            {
                ResourceType.Job => $"{JobTemplateJob.PATH}{Id}/job_host_summaries/",
                ResourceType.Host => $"{Host.PATH}{Id}/job_host_summaries/",
                ResourceType.Group => $"{Group.PATH}{Id}/job_host_summaries/",
                _ => throw new ArgumentException()
            };
            Find<JobHostSummary>(path);
        }
    }
}
