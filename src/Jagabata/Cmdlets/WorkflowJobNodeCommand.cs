using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowJobNode")]
    [OutputType(typeof(WorkflowJobNode))]
    public class GetWorkflowJobNodeCommand : GetCommandBase<WorkflowJobNode>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.WorkflowJobNode)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowJobNode)]
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

    [Cmdlet(VerbsCommon.Find, "WorkflowJobNode", DefaultParameterSetName = "All")]
    [OutputType(typeof(WorkflowJobNode))]
    public class FindWorkflowJobNodeCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJob", ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.WorkflowJob)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowJob)]
        public ulong Job { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJobNode", ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.WorkflowJobNode)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowJobNode)]
        public ulong Node { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJobNode", Position = 1)]
        public WorkflowJobNodeLinkState Linked { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "extra_data", "inventory", "execution_environment",
                           "job", "workflow_job", "unified_job_template", "success_nodes", "failure_nodes",
                           "always_nodes", "all_parents_must_converge", "do_not_run", "identifier", "labels")]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = WorkflowJobNode.PATH;
            if (Job > 0)
            {
                path = $"{WorkflowJobBase.PATH}{Job}/workflow_nodes/";
            }
            else if (Node > 0)
            {
                path = Linked switch
                {
                    WorkflowJobNodeLinkState.Always => $"{WorkflowJobNode.PATH}{Node}/always_nodes/",
                    WorkflowJobNodeLinkState.Failure => $"{WorkflowJobNode.PATH}{Node}/failure_nodes/",
                    WorkflowJobNodeLinkState.Success => $"{WorkflowJobNode.PATH}{Node}/success_nodes/",
                    _ => throw new ArgumentException()
                };
            }
            Find<WorkflowJobNode>(path);
        }
    }
}
