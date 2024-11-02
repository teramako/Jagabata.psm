using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Management.Automation;
using System.Web;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowApprovalRequest")]
    [OutputType(typeof(WorkflowApproval.Detail))]
    public class GetWorkflowApprovalRequestCommand : GetCommandBase<WorkflowApproval.Detail>
    {
        protected override ResourceType AcceptType => ResourceType.WorkflowApproval;

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "WorkflowApprovalRequest")]
    [OutputType(typeof(WorkflowApproval))]
    public class FindWorkflowApprovalRequestCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowApprovalTemplate])]
        public ulong WorkflowApprovalTemplate { get; set; }

        [Parameter()]
        [ValidateSet(nameof(JobStatus.Pending), nameof(JobStatus.Successful), nameof(JobStatus.Failed))]
        public JobStatus[]? Status { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            if (Status is not null)
            {
                Query.Add("status__in", string.Join(',', Status.Select(s => $"{s}".ToLowerInvariant())));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = WorkflowApprovalTemplate switch
            {
                > 0 => $"{Resources.WorkflowApprovalTemplate.PATH}{WorkflowApprovalTemplate}/approvals/",
                _ => WorkflowApproval.PATH
            };
            Find<WorkflowApproval>(path);
        }
    }

    public abstract class WorkflowApprovalRequestCommand : APICmdletBase
    {
        protected abstract string Command { get; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowApproval])]
        public ulong Id { get; set; }

        private readonly HashSet<ulong> treatedIds = [];

        protected override void ProcessRecord()
        {
            if (treatedIds.Contains(Id))
            {
                return;
            }

            var result = CreateResource<string>($"{WorkflowApproval.PATH}{Id}/{Command}/");
            if (result is null)
            {
                return;
            }
            treatedIds.Add(Id);
        }
        protected override void EndProcessing()
        {
            if (treatedIds.Count == 0)
            {
                return;
            }

            var query = HttpUtility.ParseQueryString("");
            query.Add("id__in", string.Join(',', treatedIds));
            query.Add("page_size", $"{treatedIds.Count}");
            foreach (var resultSet in GetResultSet<WorkflowApproval>(WorkflowApproval.PATH, query, false))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsLifecycle.Approve, "WorkflowApprovalRequest")]
    [OutputType(typeof(WorkflowApproval))]
    public class ApproveWorkflowApprovalCommand : WorkflowApprovalRequestCommand
    {
        protected override string Command => "approve";
    }

    [Cmdlet(VerbsLifecycle.Deny, "WorkflowApprovalRequest")]
    [OutputType(typeof(WorkflowApproval))]
    public class DenyWorkflowApprovalCommand : WorkflowApprovalRequestCommand
    {
        protected override string Command => "deny";
    }

    [Cmdlet(VerbsCommon.Remove, "WorkflowApprovalRequest", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveWorkflowApprovalRequestCommand : RemoveCommandBase<WorkflowApproval>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowApproval])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
