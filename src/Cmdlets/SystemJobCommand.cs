using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "SystemJob")]
    [OutputType(typeof(SystemJob.Detail))]
    public class GetSystemJobCommand : GetCommandBase<SystemJob.Detail>
    {
        protected override ResourceType AcceptType => ResourceType.SystemJob;

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
        public ulong SystemJobTemplate { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
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
