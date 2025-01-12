using Jagabata.Resources;
using System.Management.Automation;
using System.Web;

namespace Jagabata.Cmdlets.Utilities
{
    public class JobProgressManager : Dictionary<ulong, JobProgress>
    {
        public ProgressRecord RootProgress { get; } = new(0);
        private DateTime _startTime;
        private int _intervalSeconds;
        public void Add(IUnifiedJob job, int parnetId = 0)
        {
            if (!ContainsKey(job.Id))
            {
                var jp = new JobProgress(job, parnetId);
                Add(job.Id, jp);
            }
        }
        public void Start(string activityId, int intervalSeconds)
        {
            _startTime = DateTime.Now;
            _intervalSeconds = intervalSeconds;
            RootProgress.Activity = activityId;
            RootProgress.StatusDescription = "Waiting...";
            RootProgress.SecondsRemaining = intervalSeconds;
        }
        public void UpdateProgress(int index)
        {
            var elapsed = DateTime.Now - _startTime;
            RootProgress.PercentComplete = index * 100 / _intervalSeconds;
            RootProgress.SecondsRemaining = _intervalSeconds - index;
            RootProgress.StatusDescription = $"Waiting... Elapsed: {elapsed:hh\\:mm\\:ss\\.ff}";
        }
        public void UpdateJob()
        {
            var getJobsTask = UnifiedJob.Get([.. Keys]);
            getJobsTask.Wait();
            foreach (var job in getJobsTask.Result)
            {
                if (TryGetValue(job.Id, out var jp))
                {
                    jp.UpdateJob(job);
                }
            }
        }
        public IEnumerable<JobProgress> GetAll()
        {
            foreach (var item in Values)
            {
                foreach (var jp in item.GetAll())
                {
                    yield return jp;
                }
            }
        }
        public IEnumerable<JobProgress?> GetJobLog()
        {
            List<Task<JobProgress?>> tasks = [];
            foreach (var jp in GetAll().Where(static jp => jp.Job is not null && !jp.Completed))
            {
                switch (jp.Type)
                {
                    case ResourceType.WorkflowJob:
                    case ResourceType.WorkflowApproval:
                        continue;
                    default:
                        tasks.Add(jp.GetLogAsync());
                        break;
                }
            }
            Task.WaitAll([.. tasks]);
            return tasks.Select(static t => t.Result);
        }
        public List<IUnifiedJobSummary> CleanCompleted()
        {
            List<IUnifiedJobSummary> completedJobs = [];
            foreach (var (id, jp) in this)
            {
                if (jp.SetComplete())
                {
                    if (jp.Job is not null)
                    {
                        completedJobs.Add(jp.Job);
                    }
                    Remove(id);
                }
                else
                {
                    jp.CleanCompletedChildren();
                }
            }
            return completedJobs;
        }
    }

    public class JobProgress
    {
        public JobProgress(ulong id, ResourceType type, int parentId = 0)
        {
            Id = id;
            Type = type;
            Progress = new ProgressRecord((int)(id % int.MaxValue), $"[{id}]", $"New")
            {
                ParentActivityId = parentId,
            };
            ParentId = parentId;
        }
        public JobProgress(IUnifiedJobSummary job, int parentId = 0)
            : this(job.Id, job.Type, parentId)
        {
            Job = job;
            Update();
        }
        public ulong Id { get; }
        public int ParentId { get; private set; } = 0;
        public ResourceType Type { get; private set; }
        public ProgressRecord Progress { get; }
        public IUnifiedJobSummary? Job { get; private set; }
        public bool Finished { get; private set; } = false;
        public bool Completed { get; private set; } = false;
        public Dictionary<ulong, JobProgress> Children = [];

        public IEnumerable<JobProgress> GetAll()
        {
            yield return this;
            if (Type == ResourceType.WorkflowJob && Children.Count > 0)
            {
                foreach (var child in Children.Values)
                {
                    foreach (var jp in child.GetAll())
                    {
                        yield return jp;
                    }
                }
            }
        }
        public string CurrentLog { get; private set; } = string.Empty;
        protected uint JobLogStartNext { get; private set; } = 0;

        public async Task<JobProgress?> GetLogAsync()
        {
            if (Job is null) return null;
            if (Completed) return null;
            switch (Job.Type)
            {
                case ResourceType.SystemJob:
                    CurrentLog = ((ISystemJob)Job).ResultStdout;
                    return this;
                case ResourceType.WorkflowJob:
                case ResourceType.WorkflowApproval:
                    return null;
                default:
                    var query = HttpUtility.ParseQueryString("format=json");
                    query.Add("start_line", $"{JobLogStartNext}");
                    var apiResult = await RestAPI.GetAsync<JobLog>($"{Job.Url}stdout/?{query}");
                    var log = apiResult.Contents;
                    JobLogStartNext = log.Range.End;
                    CurrentLog = log.Content;
                    return this;
            }
        }

        private void Update()
        {
            if (Job is null) return;
            if (Completed) return;
            Progress.Activity = $"[{Job.Id}]{Job.Name}";
            Progress.StatusDescription = $"{Job.Status} Elapsed: {Job.Elapsed}";
            switch (Job.Status)
            {
                case JobStatus.New:
                case JobStatus.Waiting:
                case JobStatus.Pending:
                    Progress.PercentComplete = 0;
                    break;
                case JobStatus.Running:
                    Progress.PercentComplete = 50;
                    break;
                case JobStatus.Canceled:
                case JobStatus.Error:
                case JobStatus.Failed:
                case JobStatus.Successful:
                    Progress.PercentComplete = 100;
                    if (Finished)
                    {
                        SetComplete();
                    }
                    else
                    {
                        Finished = true;
                    }
                    break;
            }
            if (Completed)
            {
                /*
                if (Children.Count > 0)
                {
                    Children.Clear();
                }
                */
                return;
            }
            if (Job.Type == ResourceType.WorkflowJob)
            {
                UpdateWorkflowJobNodes().Wait();
            }
        }

        public void UpdateJob(IUnifiedJobSummary job)
        {
            Job = job;
            Update();
        }
        public bool SetComplete()
        {
            if (!Finished)
            {
                return false;
            }
            Completed = Children.Count <= 0 || Children.Values.All(static jp => jp.SetComplete());
            return Completed;
        }
        public void CleanCompletedChildren()
        {
            if (Children.Count == 0) return;
            if (Completed)
            {
                Children.Clear();
                return;
            }
            foreach (var child in Children.Values)
            {
                child.CleanCompletedChildren();
            }
        }
        private async Task UpdateWorkflowJobNodes()
        {
            var query = HttpUtility.ParseQueryString("do_not_run=False&page_size=50&order_by=id");
            var completedIds = Children.Values.Where(static jp => jp.Completed).Select(static jp => jp.Id).ToArray();
            if (completedIds.Length > 0)
            {
                query.Add("not__job__in", string.Join(',', completedIds));
            }
            await foreach (var apiResult in RestAPI.GetResultSetAsync<WorkflowJobNode>($"{WorkflowJob.PATH}{Id}/workflow_nodes/", query, true))
            {
                foreach (var node in apiResult.Contents.Results)
                {
                    if (node.SummaryFields.ContainsKey("Job")) continue;
                    JobNodeSummary jobSummary = new(node);
                    if (Children.TryGetValue(jobSummary.Id, out var jp))
                    {
                        jp.UpdateJob(jobSummary);
                    }
                    else
                    {
                        Children.Add(jobSummary.Id, new JobProgress(jobSummary, Progress.ActivityId));
                    }
                }
            }
        }
    }

    public class JobNodeSummary : IUnifiedJobSummary
    {
        public JobNodeSummary(WorkflowJobNode node)
        {
            if (node.SummaryFields["Job"] is not JobSummary job)
            {
                throw new ArgumentException($"{nameof(job)} is null");
            }
            Id = job.Id;
            Type = job.Type;
            Url = (string)node.Related["job"];
            Name = job.Name;
            Status = job.Status;
            Elapsed = job.Elapsed;
            Failed = job.Failed;
        }
        public ulong Id { get; }
        public ResourceType Type { get; }
        public string Url { get; }
        public string Name { get; }
        public JobStatus Status { get; }
        public double Elapsed { get; }
        public bool Failed { get; }
    }
}
