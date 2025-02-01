using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "InstanceGroup")]
    [OutputType(typeof(InstanceGroup))]
    public class GetInstanceGroupCommand : GetCommandBase<InstanceGroup>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.InstanceGroup])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.InstanceGroup)]
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

    [Cmdlet(VerbsCommon.Find, "InstanceGroup")]
    [OutputType(typeof(InstanceGroup))]
    public class FindInstanceGroupCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes =
        [
            ResourceType.Instance, ResourceType.Organization, ResourceType.Inventory, ResourceType.JobTemplate,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode, ResourceType.WorkflowJobNode
        ])]
        [ResourceCompletions(
            ResourceType.Instance, ResourceType.Organization, ResourceType.Inventory, ResourceType.JobTemplate,
            ResourceType.Schedule, ResourceType.WorkflowJobTemplateNode, ResourceType.WorkflowJobNode
        )]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "name", "created", "modified", "max_concurrent_jobs", "max_forks",
                           "is_container_group", "credential", "policy_instance_percentage",
                           "policy_instance_minimum", "policy_instance_list")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Instance => $"{Instance.PATH}{Resource.Id}/instance_groups/",
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/instance_groups/",
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/instance_groups/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/instance_groups/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{Resource.Id}/instance_groups/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{Resource.Id}/instance_groups/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{Resource.Id}/instance_groups/",
                _ => InstanceGroup.PATH
            };
            Find<InstanceGroup>(path);
        }
    }
}

