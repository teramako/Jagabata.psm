using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Instance")]
    [OutputType(typeof(Instance))]
    public class GetInstanceCommand : GetCommandBase<Instance>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Instance])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Instance)]
        public override ulong[] Id { get; set; } = [];

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
        [OrderByCompletion(Keys = ["id", "hostname", "uuid", "created", "modified", "last_seen", "health_check_started",
                                   "last_health_check", "errors", "capacity_adjustment", "version", "capacity", "cpu",
                                   "memory", "cpu_capacity", "mem_capacity", "enabled", "managed_by_policy", "node_type",
                                   "node_state", "ip_address", "listener_port", "peers_from_control_nodes"])]
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

