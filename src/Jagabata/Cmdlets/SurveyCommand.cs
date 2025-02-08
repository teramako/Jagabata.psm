using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "SurveySpec")]
    [OutputType(typeof(Resources.Survey))]
    public class GetSurveySpecCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [ValidateSet(nameof(ResourceType.JobTemplate), nameof(ResourceType.WorkflowJobTemplate))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/survey_spec/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Id}/survey_spec/",
                _ => throw new ArgumentException($"Unkown Resource Type: {Type}")
            };
            var survey = GetResource<Resources.Survey>(path);
            WriteObject(survey);
        }
    }

    [Cmdlet(VerbsLifecycle.Register, "SurveySpec", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterSurverySpecCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate)]
        [ResourceCompletions(ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate)]
        public IResource Template { get; set; } = new Resource(0, 0);

        [Parameter(ParameterSetName = "Spec")]
        [AllowEmptyString]
        public string Name { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "Spec")]
        [AllowEmptyString]
        public string Description { get; set; } = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "Spec")]
        public SurveySpec[] Spec { get; set; } = [];

        [Parameter(Mandatory = true, ParameterSetName = "Survey")]
        public Resources.Survey? Survey { get; set; }

        protected override void ProcessRecord()
        {
            var path = Template.Type switch
            {
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Template.Id}/survey_spec/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Template.Id}/survey_spec/",
                _ => throw new ArgumentException($"Invalid Resource Type: {Template.Type}")
            };
            var sendData = Survey is not null ? Survey : new Resources.Survey() { Name = Name, Description = Description, Spec = Spec };
            var dataDescription = Json.Stringify(sendData, pretty: true);
            if (ShouldProcess(dataDescription, $"Register Survey to {Template.Type} [{Template.Id}]"))
            {
                var apiResult = CreateResource<Resources.Survey>(path, sendData);
                if (apiResult.Response.IsSuccessStatusCode)
                {
                    WriteVerbose("Success");
                }
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "SurveySpec", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveSurveySpecCommand : RemoveCommandBase<SurveySpec>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate)]
        [ResourceCompletions(ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate)]
        public IResource Template { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = Template.Type switch
            {
                ResourceType.JobTemplate => JobTemplate.PATH,
                ResourceType.WorkflowJobTemplate => WorkflowJobTemplate.PATH,
                _ => throw new ArgumentException($"Invalid Resource Type: {Template.Type}")
            };
            TryDelete(path, Template.Id, $"Delete SurveySpec from {Template.Type} [{Template.Id}]");
        }
    }
}
