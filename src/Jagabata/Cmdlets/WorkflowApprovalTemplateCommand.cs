using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowApprovalTemplate")]
    [OutputType(typeof(WorkflowApprovalTemplate))]
    public class GetWorkflowApprovalTemplate : GetCommandBase<WorkflowApprovalTemplate>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowApprovalTemplate])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowApprovalTemplate)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }
}
