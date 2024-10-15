using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "HostMetric")]
    [OutputType(typeof(HostMetric))]
    public class GetHostMetricCommand : GetCommandBase<HostMetric>
    {
        protected override ResourceType AcceptType => ResourceType.HostMetrics;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "HostMetric", DefaultParameterSetName = "All")]
    [OutputType(typeof(HostMetric))]
    public class FindHostMetricCommand : FindCommandBase
    {
        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            Find<HostMetric>(HostMetric.PATH);
        }
    }
}
