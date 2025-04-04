using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Cmdlets.Utilities;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "UnifiedJob")]
    [OutputType(typeof(IUnifiedJob))]
    public class FindUnifiedJobCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate, ResourceType.Project,
            ResourceType.InventorySource, ResourceType.SystemJobTemplate, ResourceType.Inventory, ResourceType.Host,
            ResourceType.Group, ResourceType.Schedule, ResourceType.Instance, ResourceType.InstanceGroup
        )]
        [ResourceCompletions(
            ResourceType.JobTemplate, ResourceType.WorkflowJobTemplate, ResourceType.Project,
            ResourceType.InventorySource, ResourceType.SystemJobTemplate, ResourceType.Inventory, ResourceType.Host,
            ResourceType.Group, ResourceType.Schedule, ResourceType.Instance, ResourceType.InstanceGroup
        )]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "unified_job_template",
                           "launch_type", "status", "execution_environment", "failed", "started", "finished",
                           "canceled_on", "elapsed", "job_explanation", "execution_node", "controller_node",
                           "work_unit_id", "notifications", "organization", "schedule", "created_by",
                           "modified_by", "credentials", "instance_group", "labels")]
        public override string[] OrderBy { get; set; } = ["!id"];

        private IEnumerable<ResultSet> GetResultSet(string path, HttpQuery query)
        {
            var nextPathAndQuery = query.Count == 0 ? path : $"{path}?{query}";
            var count = 0;
            do
            {
                WriteVerboseRequest(nextPathAndQuery, Method.GET);
                RestAPIResult<ResultSet>? result;
                try
                {
                    using var apiTask = RestAPI.GetAsync<ResultSet>(nextPathAndQuery);
                    apiTask.Wait();
                    result = apiTask.Result;
                    WriteVerboseResponse(result.Response);
                }
                catch (RestAPIException ex)
                {
                    WriteVerboseResponse(ex.Response);
                    throw;
                }
                catch (AggregateException aex)
                {
                    if (aex.InnerException is RestAPIException ex)
                    {
                        WriteVerboseResponse(ex.Response);
                        throw ex;
                    }
                    throw;
                }
                var resultSet = result.Contents;

                yield return resultSet;

                nextPathAndQuery = resultSet.Next ?? string.Empty;
            } while ((query.IsInfinity || ++count < query.QueryCount)
                     && !string.IsNullOrEmpty(nextPathAndQuery));
        }
        private void WriteResultSet(string path)
        {
            foreach (var resultSet in GetResultSet(path, Query.Build()))
            {
                WriteObject(resultSet.Results, true);
            }
        }
        private void WriteResultSet<T>(string path) where T : class
        {
            foreach (var resultSet in GetResultSet<T>(path, Query.Build()))
            {
                WriteObject(resultSet.Results, true);
            }
        }
        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            switch (Resource?.Type)
            {
                case ResourceType.JobTemplate:
                    WriteResultSet<JobTemplateJob>($"{JobTemplate.PATH}{Resource.Id}/jobs/");
                    break;
                case ResourceType.WorkflowApprovalTemplate:
                    WriteResultSet<WorkflowJob>($"{WorkflowJobTemplate.PATH}{Resource.Id}/workflow_jobs/");
                    break;
                case ResourceType.Project:
                    WriteResultSet<ProjectUpdateJob>($"{Project.PATH}{Resource.Id}/project_updates/");
                    break;
                case ResourceType.InventorySource:
                    WriteResultSet<InventoryUpdateJob>($"{InventorySource.PATH}{Resource.Id}/inventory_updates/");
                    break;
                case ResourceType.SystemJobTemplate:
                    WriteResultSet<SystemJob>($"{SystemJobBase.PATH}{Resource.Id}/jobs/");
                    break;
                case ResourceType.Inventory:
                    WriteResultSet<AdHocCommand>($"{Inventory.PATH}{Resource.Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Host:
                    WriteResultSet<AdHocCommand>($"{Host.PATH}{Resource.Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Group:
                    WriteResultSet<AdHocCommand>($"{Group.PATH}{Resource.Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Schedule:
                    WriteResultSet($"{Resources.Schedule.PATH}{Resource.Id}/jobs/");
                    break;
                case ResourceType.Instance:
                    WriteResultSet($"{Instance.PATH}{Resource.Id}/jobs/");
                    break;
                case ResourceType.InstanceGroup:
                    WriteResultSet($"{InstanceGroup.PATH}{Resource.Id}/jobs/");
                    break;
                default:
                    WriteResultSet(UnifiedJob.PATH);
                    break;
            }
        }
    }

    [Cmdlet(VerbsLifecycle.Wait, "UnifiedJob")]
    [OutputType(typeof(JobTemplateJob),
                typeof(ProjectUpdateJob),
                typeof(InventoryUpdateJob),
                typeof(SystemJob),
                typeof(AdHocCommand),
                typeof(WorkflowJob))]
    public class WaitJobCommand : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.Job, ResourceType.ProjectUpdate, ResourceType.InventoryUpdate,
            ResourceType.SystemJob, ResourceType.AdHocCommand, ResourceType.WorkflowJob
        )]
        [ResourceCompletions(
            ResourceType.Job, ResourceType.ProjectUpdate, ResourceType.InventoryUpdate,
            ResourceType.SystemJob, ResourceType.AdHocCommand, ResourceType.WorkflowJob
        )]
        public IResource Job { get; set; } = new Resource(0, 0);

        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            JobProgressManager.Add(Job.Id, new JobProgress(Job.Id, Job.Type));
        }
        protected override void EndProcessing()
        {
            JobProgressManager.UpdateJob();
            ShowJobLog(SuppressJobLog);
            JobProgressManager.CleanCompleted();
            WaitJobs("Wait Job", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Stop, "UnifiedJob", DefaultParameterSetName = "AssociatedWith")]
    [OutputType(typeof(PSObject))]
    public class StopJobCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Job),
                     nameof(ResourceType.ProjectUpdate),
                     nameof(ResourceType.InventoryUpdate),
                     nameof(ResourceType.SystemJob),
                     nameof(ResourceType.AdHocCommand),
                     nameof(ResourceType.WorkflowJob))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", Position = 0)]
        [ResourceTransformation(
            ResourceType.Job, ResourceType.ProjectUpdate, ResourceType.InventoryUpdate,
            ResourceType.SystemJob, ResourceType.AdHocCommand, ResourceType.WorkflowJob
        )]
        public IResource? Job { get; set; }

        [Parameter()]
        public SwitchParameter Determine { get; set; }

        protected override void ProcessRecord()
        {
            if (Job is not null)
            {
                Type = Job.Type;
                Id = Job.Id;
            }

            var path = Type switch
            {
                ResourceType.Job => $"{JobTemplateJobBase.PATH}{Id}/cancel/",
                ResourceType.ProjectUpdate => $"{ProjectUpdateJobBase.PATH}{Id}/cancel/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJobBase.PATH}{Id}/cancel/",
                ResourceType.AdHocCommand => $"{AdHocCommandBase.PATH}{Id}/cancel/",
                ResourceType.SystemJob => $"{SystemJobBase.PATH}{Id}/cancel/",
                ResourceType.WorkflowJob => $"{WorkflowJobBase.PATH}{Id}/cancel/",
                _ => throw new NotImplementedException()
            };
            var psobject = new PSObject();
            psobject.Members.Add(new PSNoteProperty("Id", Id));
            psobject.Members.Add(new PSNoteProperty("Type", Type));
            if (Determine)
            {
                var result = GetResource<Dictionary<string, bool>>(path);
                result.TryGetValue("can_cancel", out bool canCancel);
                psobject.Members.Add(new PSNoteProperty("CanCancel", canCancel));
                WriteObject(psobject);
            }
            else
            {
                try
                {
                    var apiResult = CreateResource<string>(path);
                    psobject.Members.Add(new PSNoteProperty("Status", apiResult.Response.StatusCode));
                }
                catch (RestAPIException ex)
                {
                    psobject.Members.Add(new PSNoteProperty("Status", ex.Response.StatusCode));
                }
                WriteObject(psobject);
            }
        }
    }
}
