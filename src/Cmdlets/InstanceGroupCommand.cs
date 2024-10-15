using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "InstanceGroup")]
    [OutputType(typeof(InstanceGroup))]
    public class GetInstanceGroupCommand : GetCommandBase<InstanceGroup>
    {
        protected override ResourceType AcceptType => ResourceType.InstanceGroup;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "InstanceGroup", DefaultParameterSetName = "All")]
    [OutputType(typeof(InstanceGroup))]
    public class FindInstanceGroupCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.Instance),
                     nameof(ResourceType.Organization),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.Schedule),
                     nameof(ResourceType.WorkflowJobTemplateNode),
                     nameof(ResourceType.WorkflowJobNode))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public ulong Id { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.Instance => $"{Instance.PATH}{Id}/instance_groups/",
                ResourceType.Organization => $"{Organization.PATH}{Id}/instance_groups/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/instance_groups/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/instance_groups/",
                ResourceType.Schedule => $"{Schedule.PATH}{Id}/instance_groups/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{Id}/instance_groups/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{Id}/instance_groups/",
                _ => InstanceGroup.PATH
            };
            Find<InstanceGroup>(path);
        }
    }
}

