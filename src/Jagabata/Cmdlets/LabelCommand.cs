using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Label")]
    [OutputType(typeof(Label))]
    public class GetLabelCommand : GetCommandBase<Label>
    {
        protected override ResourceType AcceptType => ResourceType.Label;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Label", DefaultParameterSetName = "All")]
    [OutputType(typeof(Label))]
    public class FindLabelCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Inventory),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.Job),
                     nameof(ResourceType.Schedule),
                     nameof(ResourceType.WorkflowJobTemplate),
                     nameof(ResourceType.WorkflowJob),
                     nameof(ResourceType.WorkflowJobTemplateNode),
                     nameof(ResourceType.WorkflowJobNode))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Inventory,
                ResourceType.JobTemplate,
                ResourceType.Job,
                ResourceType.Schedule,
                ResourceType.WorkflowJobTemplate,
                ResourceType.WorkflowJob,
                ResourceType.WorkflowJobTemplateNode,
                ResourceType.WorkflowJobNode
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
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
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/labels/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/labels/",
                ResourceType.Job => $"{JobTemplateJob.PATH}{Id}/labels/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{Id}/labels/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Id}/labels/",
                ResourceType.WorkflowJob => $"{WorkflowJob.PATH}{Id}/labels/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{Id}/labels/",
                ResourceType.WorkflowJobNode => $"{WorkflowJobNode.PATH}{Id}/labels/",
                _ => Label.PATH
            };
            Find<Label>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "Label", SupportsShouldProcess = true)]
    [OutputType(typeof(Label))]
    public class NewLabelCommand : NewCommandBase<Label>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter(Mandatory = true)]
        [ValidateRange(1, ulong.MaxValue)]
        public ulong Organization { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "organization", Organization },
            };
            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsLifecycle.Register, "Label", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterLabelCommand : RegistrationCommandBase<Label>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Label])]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Inventory,
                ResourceType.JobTemplate,
                ResourceType.Schedule,
                ResourceType.WorkflowJobTemplate,
                ResourceType.WorkflowJobTemplateNode
        ])]
        public IResource To { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = To.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{To.Id}/labels/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{To.Id}/labels/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{To.Id}/labels/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{To.Id}/labels/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{To.Id}/labels/",
                _ => throw new ArgumentException($"Invalid resource type: {To.Type}")
            };
            Register(path, Id, To);
        }
    }

    [Cmdlet(VerbsLifecycle.Unregister, "Label", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class UnregisterLabelCommand : RegistrationCommandBase<Label>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Label])]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Inventory,
                ResourceType.JobTemplate,
                ResourceType.Schedule,
                ResourceType.WorkflowJobTemplate,
                ResourceType.WorkflowJobTemplateNode
        ])]
        public IResource From { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = From.Type switch
            {
                ResourceType.Inventory => $"{Inventory.PATH}{From.Id}/labels/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{From.Id}/labels/",
                ResourceType.Schedule => $"{Resources.Schedule.PATH}{From.Id}/labels/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{From.Id}/labels/",
                ResourceType.WorkflowJobTemplateNode => $"{WorkflowJobTemplateNode.PATH}{From.Id}/labels/",
                _ => throw new ArgumentException($"Invalid resource type: {From.Type}")
            };
            Unregister(path, Id, From);
        }
    }

    [Cmdlet(VerbsData.Update, "Label", SupportsShouldProcess = true)]
    [OutputType(typeof(Label))]
    public class UpdateLabelCommand : UpdateCommandBase<Label>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Label])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong Organization { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Organization > 0)
                sendData.Add("organization", Organization);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }
}
