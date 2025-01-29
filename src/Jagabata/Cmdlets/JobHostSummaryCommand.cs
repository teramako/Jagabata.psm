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
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.JobHostSummary])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.JobHostSummary)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "JobHostSummary", DefaultParameterSetName = "AssociatedWith")]
    [OutputType(typeof(JobHostSummary))]
    public class FindJobHostSummaryCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Job),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.Group))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput")]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Job,
                ResourceType.Host,
                ResourceType.Group
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "created", "modified", "job", "host", "constructed_host", "host_name",
                                   "changed", "dark", "failures", "ok", "processed", "skipped", "failed",
                                   "ignored", "rescued"])]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

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
