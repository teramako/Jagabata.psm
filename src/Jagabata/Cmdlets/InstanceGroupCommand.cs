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

    [Cmdlet(VerbsCommon.Find, "InstanceGroup", DefaultParameterSetName = "All")]
    [OutputType(typeof(InstanceGroup))]
    public class FindInstanceGroupCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Instance),
                     nameof(ResourceType.Organization),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.Schedule),
                     nameof(ResourceType.WorkflowJobTemplateNode),
                     nameof(ResourceType.WorkflowJobNode))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Instance,
                ResourceType.Organization,
                ResourceType.Inventory,
                ResourceType.JobTemplate,
                ResourceType.Schedule,
                ResourceType.WorkflowJobTemplateNode,
                ResourceType.WorkflowJobNode
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "name", "created", "modified", "max_concurrent_jobs", "max_forks",
                                   "is_container_group", "credential", "policy_instance_percentage",
                                   "policy_instance_minimum", "policy_instance_list"])]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.Instance => $"{Instance.PATH}{Id}/instance_groups/",
                ResourceType.Organization => $"{Organization.PATH}{Id}/instance_groups/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/instance_groups/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/instance_groups/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{Id}/instance_groups/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{Id}/instance_groups/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{Id}/instance_groups/",
                _ => InstanceGroup.PATH
            };
            Find<InstanceGroup>(path);
        }
    }
}

