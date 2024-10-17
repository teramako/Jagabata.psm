using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Instance")]
    [OutputType(typeof(Instance))]
    public class GetInstanceCommand : GetCommandBase<Instance>
    {
        protected override ResourceType AcceptType => ResourceType.Instance;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Instance", DefaultParameterSetName = "All")]
    [OutputType(typeof(Instance))]
    public class FindInstanceCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true)]
        public ulong InstanceGroup { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = InstanceGroup > 0 ? $"{Resources.InstanceGroup.PATH}{InstanceGroup}/instances/" : Instance.PATH;
            Find<Instance>(path);
        }
    }
}

