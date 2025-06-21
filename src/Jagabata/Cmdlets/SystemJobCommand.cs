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
        [ResourceIdTransformation(ResourceType.SystemJob)]
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
        [ResourceIdTransformation(ResourceType.SystemJobTemplate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.SystemJobTemplate)]
        [Alias("template", "t")]
        public ulong SystemJobTemplate { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [OrderByCompletionFromHelp(ResourceType.SystemJob, SystemJobBase.PATH)]
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
            var path = SystemJobTemplate > 0
                ? $"{Resources.SystemJobTemplate.PATH}{SystemJobTemplate}/jobs/"
                : SystemJobBase.PATH;
            Find<SystemJob>(path);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "SystemJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveSystemJobCommand : RemoveCommandBase<SystemJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.SystemJob)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.SystemJob)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
