using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "JobHostSummary")]
    [OutputType(typeof(JobHostSummary))]
    public class GetJobHostSummaryCommand : GetCommandBase<JobHostSummary>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.JobHostSummary)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.JobHostSummary)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "JobHostSummary")]
    [OutputType(typeof(JobHostSummary))]
    public class FindJobHostSummaryCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.Job, ResourceType.Host, ResourceType.Group)]
        [ResourceCompletions(ResourceType.Job, ResourceType.Host, ResourceType.Group)]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "job", "host", "constructed_host", "host_name", "changed",
                           "dark", "failures", "ok", "processed", "skipped", "failed", "ignored", "rescued")]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.Job => $"{JobTemplateJob.PATH}{Resource.Id}/job_host_summaries/",
                ResourceType.Host => $"{Host.PATH}{Resource.Id}/job_host_summaries/",
                ResourceType.Group => $"{Group.PATH}{Resource.Id}/job_host_summaries/",
                _ => throw new ArgumentException()
            };
            Find<JobHostSummary>(path);
        }
    }
}
