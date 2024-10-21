using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Collections;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "SystemJobTemplate")]
    [OutputType(typeof(SystemJobTemplate))]
    public class GetSystemJobTemplateCommand : GetCommandBase<SystemJobTemplate>
    {
        protected override ResourceType AcceptType => ResourceType.SystemJobTemplate;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "SystemJobTemplate")]
    [OutputType(typeof(SystemJobTemplate))]
    public class FindSystemJobTemplateCommand : FindCommandBase
    {
        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];


        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            Find<SystemJobTemplate>(SystemJobTemplate.PATH);
        }
    }

    public class LaunchSystemJobTemplateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, Position = 0)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Template", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.SystemJobTemplate])]
        public IResource? SystemJobTemplate { get; set; }

        [Parameter()]
        public IDictionary? ExtraVars { get; set; }

        protected Hashtable CreateSendData()
        {
            var dict = new Hashtable();
            if (ExtraVars is not null)
            {
                dict.Add("extra_vars", ExtraVars);
            }
            return dict;
        }
        protected SystemJob.Detail Launch(ulong id)
        {
            var apiResult = CreateResource<SystemJob.Detail>($"{Resources.SystemJobTemplate.PATH}{id}/launch/", CreateSendData());
            return apiResult.Contents ?? throw new NullReferenceException();
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "SystemJobTemplate")]
    [OutputType(typeof(SystemJob))]
    public class InvokeSystemJobTemplateCommand : LaunchSystemJobTemplateCommandBase
    {
        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (SystemJobTemplate is not null)
            {
                Id = SystemJobTemplate.Id;
            }
            var job = Launch(Id);
            WriteVerbose($"Launch SystemJobTemplate:{Id} => Job:[{job.Id}]");
            JobProgressManager.Add(job);
        }
        protected override void EndProcessing()
        {
            WaitJobs("Launch SystemJobTemplate", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "SystemJobTemplate")]
    [OutputType(typeof(SystemJob.Detail))]
    public class StartSystemJobTemplateCommand : LaunchSystemJobTemplateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (SystemJobTemplate is not null)
            {
                Id = SystemJobTemplate.Id;
            }
            var job = Launch(Id);
            WriteVerbose($"Launch SystemJobTemplate:{Id} => Job:[{job.Id}]");
            WriteObject(job);
        }
    }
}
