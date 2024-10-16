namespace AWX.Cmdlets;

/// <summary>
/// Link status between nodes for WorkflowJobNode and WorkflowJobTemplateNode
/// </summary>
public enum WorkflowJobNodeLinkState
{
    Always,
    Failure,
    Success
}
