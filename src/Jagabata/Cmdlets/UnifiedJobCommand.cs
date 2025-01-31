using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Cmdlets.Utilities;
using Jagabata.Resources;
using System.Collections.Specialized;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "UnifiedJob", DefaultParameterSetName = "All")]
    [OutputType(typeof(IUnifiedJob))]
    public class FindUnifiedJobCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.WorkflowJobTemplate),
                     nameof(ResourceType.Project),
                     nameof(ResourceType.InventorySource),
                     nameof(ResourceType.SystemJobTemplate),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.Host),
                     nameof(ResourceType.Group),
                     nameof(ResourceType.Schedule),
                     nameof(ResourceType.Instance),
                     nameof(ResourceType.InstanceGroup))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.JobTemplate,
                ResourceType.WorkflowJobTemplate,
                ResourceType.Project,
                ResourceType.InventorySource,
                ResourceType.SystemJobTemplate,
                ResourceType.Inventory,
                ResourceType.Host,
                ResourceType.Group,
                ResourceType.Schedule,
                ResourceType.Instance,
                ResourceType.InstanceGroup
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "unified_job_template",
                           "launch_type", "status", "execution_environment", "failed", "started", "finished",
                           "canceled_on", "elapsed", "job_explanation", "execution_node", "controller_node",
                           "work_unit_id", "notifications", "organization", "schedule", "created_by",
                           "modified_by", "credentials", "instance_group", "labels")]
        public override string[] OrderBy { get; set; } = ["!id"];

        private IEnumerable<ResultSet> GetResultSet(string path,
                                                    NameValueCollection? query = null,
                                                    bool getAll = false)
        {
            var nextPathAndQuery = path + (query is null ? "" : $"?{query}");
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

                nextPathAndQuery = string.IsNullOrEmpty(resultSet?.Next) ? string.Empty : resultSet.Next;
            } while (getAll && !string.IsNullOrEmpty(nextPathAndQuery));
        }
        private void WriteResultSet(string path)
        {
            foreach (var resultSet in GetResultSet(path, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
        private void WriteResultSet<T>(string path) where T : class
        {
            foreach (var resultSet in GetResultSet<T>(path, Query, All))
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
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            switch (Type)
            {
                case ResourceType.JobTemplate:
                    WriteResultSet<JobTemplateJob>($"{JobTemplate.PATH}{Id}/jobs/");
                    break;
                case ResourceType.WorkflowApprovalTemplate:
                    WriteResultSet<WorkflowJob>($"{WorkflowJobTemplate.PATH}{Id}/workflow_jobs/");
                    break;
                case ResourceType.Project:
                    WriteResultSet<ProjectUpdateJob>($"{Project.PATH}{Id}/project_updates/");
                    break;
                case ResourceType.InventorySource:
                    WriteResultSet<InventoryUpdateJob>($"{InventorySource.PATH}{Id}/inventory_updates/");
                    break;
                case ResourceType.SystemJobTemplate:
                    WriteResultSet<SystemJob>($"{SystemJob.PATH}{Id}/jobs/");
                    break;
                case ResourceType.Inventory:
                    WriteResultSet<AdHocCommand>($"{Inventory.PATH}{Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Host:
                    WriteResultSet<AdHocCommand>($"{Host.PATH}{Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Group:
                    WriteResultSet<AdHocCommand>($"{Group.PATH}{Id}/ad_hoc_commands/");
                    break;
                case ResourceType.Schedule:
                    WriteResultSet($"{Resources.Schedule.PATH}{Id}/jobs/");
                    break;
                case ResourceType.Instance:
                    WriteResultSet($"{Instance.PATH}{Id}/jobs/");
                    break;
                case ResourceType.InstanceGroup:
                    WriteResultSet($"{InstanceGroup.PATH}{Id}/jobs/");
                    break;
                default:
                    WriteResultSet(UnifiedJob.PATH);
                    break;
            };
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

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                     ResourceType.Job,
                     ResourceType.ProjectUpdate,
                     ResourceType.InventoryUpdate,
                     ResourceType.SystemJob,
                     ResourceType.AdHocCommand,
                     ResourceType.WorkflowJob
        ])]
        public IResource? Job { get; set; }

        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (Job is not null)
            {
                Type = Job.Type;
                Id = Job.Id;
            }

            JobProgressManager.Add(Id, new JobProgress(Id, Type));
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
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Job,
                ResourceType.ProjectUpdate,
                ResourceType.InventoryUpdate,
                ResourceType.SystemJob,
                ResourceType.AdHocCommand,
                ResourceType.WorkflowJob
        ])]
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
                ResourceType.Job => $"{JobTemplateJob.PATH}{Id}/cancel/",
                ResourceType.ProjectUpdate => $"{ProjectUpdateJob.PATH}{Id}/cancel/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJob.PATH}{Id}/cancel/",
                ResourceType.AdHocCommand => $"{AdHocCommand.PATH}{Id}/cancel/",
                ResourceType.SystemJob => $"{SystemJob.PATH}{Id}/cancel/",
                ResourceType.WorkflowJob => $"{WorkflowJob.PATH}{Id}/cancel/",
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
