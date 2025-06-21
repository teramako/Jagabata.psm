using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowApprovalRequest")]
    [OutputType(typeof(WorkflowApproval.Detail))]
    public class GetWorkflowApprovalRequestCommand : GetCommandBase<WorkflowApproval.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.WorkflowApproval)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowApproval)]
        public override ulong[] Id { get; set; } = [];

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
        [ResourceIdTransformation(ResourceType.WorkflowApprovalTemplate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowApprovalTemplate)]
        [Alias("template", "t")]
        public ulong WorkflowApprovalTemplate { get; set; }

        [Parameter()]
        [ValidateSet(nameof(JobStatus.Pending), nameof(JobStatus.Successful), nameof(JobStatus.Failed))]
        public JobStatus[]? Status { get; set; }

        [Parameter()]
        [OrderByCompletionFromHelp(ResourceType.WorkflowApproval, WorkflowApprovalBase.PATH)]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            if (Status is not null)
            {
                Query.Add("status__in", string.Join(',', Status.Select(static s => $"{s}".ToLowerInvariant())));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = WorkflowApprovalTemplate switch
            {
                > 0 => $"{Resources.WorkflowApprovalTemplate.PATH}{WorkflowApprovalTemplate}/approvals/",
                _ => WorkflowApprovalBase.PATH
            };
            Find<WorkflowApproval>(path);
        }
    }

    public abstract class WorkflowApprovalRequestCommand : APICmdletBase
    {
        protected abstract string Command { get; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.WorkflowApproval)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowApproval)]
        public ulong Id { get; set; }

        private readonly HashSet<ulong> treatedIds = [];

        protected override void ProcessRecord()
        {
            if (treatedIds.Contains(Id))
            {
                return;
            }

            var result = CreateResource<string>($"{WorkflowApprovalBase.PATH}{Id}/{Command}/");
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

            WriteObject(new QueryBuilder().SetOrderBy("id")
                                          .BuildWithIdList(treatedIds.Order().ToArray())
                                          .SelectMany(query => GetResultSet<WorkflowApproval>(WorkflowApprovalBase.PATH, query))
                                          .SelectMany(resultSet => resultSet.Results),
                        true);
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
        [ResourceIdTransformation(ResourceType.WorkflowApproval)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowApproval)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
