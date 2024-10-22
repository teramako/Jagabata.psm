using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowApprovalTemplate")]
    [OutputType(typeof(WorkflowApprovalTemplate))]
    public class GetWorkflowApprovalTemplate : GetCommandBase<WorkflowApprovalTemplate>
    {
        protected override ResourceType AcceptType => ResourceType.WorkflowApprovalTemplate;

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }
}
