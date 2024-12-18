using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ProjectUpdateJob")]
    [OutputType(typeof(ProjectUpdateJob.Detail))]
    public class GetProjectUpdateJobCommand : GetCommandBase<ProjectUpdateJob.Detail>
    {
        protected override ResourceType AcceptType => ResourceType.ProjectUpdate;

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
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Project])]
        public ulong Project { get; set; }

        [Parameter()]
        public string[]? Name { get; set; }

        [Parameter()]
        [ValidateSet(typeof(EnumValidateSetGenerator<JobStatus>))]
        public string[]? Status { get; set; }

        [Parameter()]
        [OrderByCompletion(Keys = ["id", "created", "modified", "name", "description", "launch_type", "status",
                                   "execution_environment", "failed", "started", "finished", "canceled_on", "elapsed",
                                   "job_explanation", "execution_node", "work_unit_id", "local_path", "scm_type",
                                   "scm_url", "scm_branch", "scm_refspec", "scm_clean", "scm_track_submodules",
                                   "scm_delete_on_update", "credential", "timeout", "scm_revision", "project"])]
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
            var path = Project > 0 ? $"{Resources.Project.PATH}{Project}/project_updates/" : ProjectUpdateJob.PATH;
            Find<ProjectUpdateJob>(path);
        }
    }

    public abstract class LaunchProjectUpdateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, Position = 0)]
        [Parameter(Mandatory = true, ParameterSetName = "CheckId", ValueFromPipeline = true, Position = 0)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Project", ValueFromPipeline = true, Position = 0)]
        [Parameter(Mandatory = true, ParameterSetName = "CheckProject", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.Project])]
        public IResource? Project { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "CheckId")]
        [Parameter(Mandatory = true, ParameterSetName = "CheckProject")]
        public SwitchParameter Check { get; set; }

        protected void CheckCanUpdate(ulong projectId)
        {
            var res = GetResource<CanUpdateProject>($"{Resources.Project.PATH}{projectId}/update/");
            var psobject = new PSObject();
            psobject.Members.Add(new PSNoteProperty("Id", projectId));
            psobject.Members.Add(new PSNoteProperty("Type", ResourceType.Project));
            psobject.Members.Add(new PSNoteProperty("CanUpdate", res.CanUpdate));
            WriteObject(psobject, false);
        }
        protected ProjectUpdateJob.Detail UpdateProject(ulong projectId)
        {
            var apiResult = CreateResource<ProjectUpdateJob.Detail>($"{Resources.Project.PATH}{projectId}/update/");
            return apiResult.Contents ?? throw new NullReferenceException();
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "ProjectUpdate")]
    [OutputType(typeof(ProjectUpdateJob), ParameterSetName = ["Id", "Project"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["CheckId", "CheckProject"])]
    public class InvokeProjectUpdateCommand : LaunchProjectUpdateCommandBase
    {
        [Parameter(ParameterSetName = "Id")]
        [Parameter(ParameterSetName = "Project")]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter(ParameterSetName = "Id")]
        [Parameter(ParameterSetName = "Project")]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (Project is not null)
            {
                Id = Project.Id;
            }
            if (Check)
            {
                CheckCanUpdate(Id);
            }
            else
            {
                var job = UpdateProject(Id);
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

    [Cmdlet(VerbsLifecycle.Start, "ProjectUpdate")]
    [OutputType(typeof(ProjectUpdateJob.Detail), ParameterSetName = ["Id", "Project"])]
    [OutputType(typeof(PSObject), ParameterSetName = ["CheckId", "CheckProject"])]
    public class StartProjectUpdateCommand : LaunchProjectUpdateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (Project is not null)
            {
                Id = Project.Id;
            }
            if (Check)
            {
                CheckCanUpdate(Id);
            }
            else
            {
                var job = UpdateProject(Id);
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
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ProjectUpdate])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
