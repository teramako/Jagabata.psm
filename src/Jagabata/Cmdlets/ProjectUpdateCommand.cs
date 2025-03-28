using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ProjectUpdateJob")]
    [OutputType(typeof(ProjectUpdateJob.Detail))]
    public class GetProjectUpdateJobCommand : GetCommandBase<ProjectUpdateJob.Detail>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.ProjectUpdate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.ProjectUpdate)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            WriteObject(GetResource(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "ProjectUpdateJob")]
    [OutputType(typeof(ProjectUpdateJob))]
    public class FindProjectUpdateJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Project)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Project)]
        public ulong Project { get; set; }

        [Parameter()]
        public string[]? Name { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "launch_type", "status",
                           "execution_environment", "failed", "started", "finished", "canceled_on", "elapsed",
                           "job_explanation", "execution_node", "work_unit_id", "local_path", "scm_type",
                           "scm_url", "scm_branch", "scm_refspec", "scm_clean", "scm_track_submodules",
                           "scm_delete_on_update", "credential", "timeout", "scm_revision", "project")]
        public override string[] OrderBy { get; set; } = ["!id"];


        protected override void BeginProcessing()
        {
            if (Name is not null)
            {
                Query.Add("name__in", string.Join(',', Name));
            }
            if (Status is not null)
            {
                Query.Add("status__in", string.Join(',', Status));
            }
            SetupCommonQuery();
        }
        protected override void EndProcessing()
        {
            var path = Project > 0
                ? $"{Resources.Project.PATH}{Project}/project_updates/"
                : ProjectUpdateJobBase.PATH;
            Find<ProjectUpdateJob>(path);
        }
    }

    public abstract class LaunchProjectUpdateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Project)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Project)]
        [Alias("project", "p")]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Check")]
        public SwitchParameter Check { get; set; }

        protected void CheckCanUpdate(ulong projectId)
        {
            var res = GetResource<CanUpdateProject>($"{Project.PATH}{projectId}/update/");
            var psobject = new PSObject();
            psobject.Members.Add(new PSNoteProperty("Id", projectId));
            psobject.Members.Add(new PSNoteProperty("Type", ResourceType.Project));
            psobject.Members.Add(new PSNoteProperty("CanUpdate", res.CanUpdate));
            WriteObject(psobject, false);
        }
        protected bool TryUpdateProject(ulong projectId, [MaybeNullWhen(false)] out ProjectUpdateJob.Detail job)
        {
            job = CreateResource<ProjectUpdateJob.Detail>($"{Project.PATH}{projectId}/update/").Contents;
            return job is not null;
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "ProjectUpdate", DefaultParameterSetName = "Update")]
    [OutputType(typeof(ProjectUpdateJob), ParameterSetName = ["Update"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["Check"])]
    public class InvokeProjectUpdateCommand : LaunchProjectUpdateCommandBase
    {
        [Parameter(ParameterSetName = "Update")]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter(ParameterSetName = "Update")]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (Check)
            {
                CheckCanUpdate(Id);
            }
            else if (TryUpdateProject(Id, out var job))
            {
                WriteVerbose($"Update Project:{Id} => Job:[{job.Id}]");
                JobProgressManager.Add(job);
            }
        }
        protected override void EndProcessing()
        {
            if (Check)
            {
                return;
            }
            WaitJobs("Update Project", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "ProjectUpdate", DefaultParameterSetName = "Update")]
    [OutputType(typeof(ProjectUpdateJob.Detail), ParameterSetName = ["Update"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["Check"])]
    public class StartProjectUpdateCommand : LaunchProjectUpdateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (Check)
            {
                CheckCanUpdate(Id);
            }
            else if (TryUpdateProject(Id, out var job))
            {
                WriteVerbose($"Update Project:{Id} => Job:[{job.Id}]");
                WriteObject(job, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "ProjectUpdateJob", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveProjectUpdateCommand : RemoveCommandBase<ProjectUpdateJob>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.ProjectUpdate)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.ProjectUpdate)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
