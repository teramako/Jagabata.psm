using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowJobNode")]
    [OutputType(typeof(WorkflowJobNode))]
    public class GetWorkflowJobNodeCommand : GetCommandBase<WorkflowJobNode>
    {
        protected override ResourceType AcceptType => ResourceType.WorkflowJobNode;

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
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJob])]
        public ulong Job { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJobNode", ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJobNode])]
        public ulong Node { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJobNode", Position = 1)]
        public WorkflowJobNodeLinkState Linked { get; set; }

        [Parameter()]
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
                path = $"{WorkflowJob.PATH}{Job}/workflow_nodes/";
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
