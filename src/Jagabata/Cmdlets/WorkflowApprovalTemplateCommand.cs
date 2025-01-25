using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowApprovalTemplate")]
    [OutputType(typeof(WorkflowApprovalTemplate))]
    public class GetWorkflowApprovalTemplate : GetCommandBase<WorkflowApprovalTemplate>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }
}
