using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Jagabata.Cmdlets
{
    public enum JobLogFormat
    {
        NotSpecified,
        txt,
        ansi,
        json,
        html
    }

    [Cmdlet(VerbsCommon.Get, "JobLog", DefaultParameterSetName = "StdOutTypeAndId")]
    [OutputType(typeof(string), ParameterSetName = ["StdOutTypeAndId", "StdOutResource"])]
    [OutputType(typeof(FileInfo), ParameterSetName = ["DownloadTypeAndId", "DownloadResource"])]
    public class GetJobLogCommand : APICmdletBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "StdOutTypeAndId", Position = 0)]
        [Parameter(Mandatory = true, ParameterSetName = "DownloadTypeAndId", Position = 0)]
        [ValidateSet(nameof(ResourceType.Job),
                     nameof(ResourceType.ProjectUpdate),
                     nameof(ResourceType.InventoryUpdate),
                     nameof(ResourceType.SystemJob),
                     nameof(ResourceType.WorkflowJob),
                     nameof(ResourceType.AdHocCommand))]
        public ResourceType Type { get; set; } = ResourceType.None;

        [Parameter(Mandatory = true, ParameterSetName = "StdOutTypeAndId", Position = 1)]
        [Parameter(Mandatory = true, ParameterSetName = "DownloadTypeAndId", Position = 1)]
        public ulong Id { get; set; } = 0;

        [Parameter(Mandatory = true, ParameterSetName = "StdOutResource", ValueFromPipeline = true, Position = 0)]
        [Parameter(Mandatory = true, ParameterSetName = "DownloadResource", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Job,
                ResourceType.ProjectUpdate,
                ResourceType.InventoryUpdate,
                ResourceType.SystemJob,
                ResourceType.WorkflowJob,
                ResourceType.AdHocCommand
        ])]
        public IResource Job { get; set; } = new Resource(0, 0);

        [Parameter(Mandatory = true, ParameterSetName = "DownloadTypeAndId")]
        [Parameter(Mandatory = true, ParameterSetName = "DownloadResource")]
        public DirectoryInfo? Download { get; set; }

        [Parameter()]
        [ValidateSet("txt", "ansi", "json", "html")]
        public JobLogFormat Format { get; set; } = JobLogFormat.NotSpecified;

        [Parameter()]
        public SwitchParameter Dark { get; set; }

        private readonly NameValueCollection Query = HttpUtility.ParseQueryString(string.Empty);
        /// <summary>
        /// HashSet to avoid duplicate retrieval of the same job
        /// </summary>
        private readonly HashSet<ulong> _jobIdSet = [];
        private readonly List<IResource> _jobs = [];

        /// <summary>
        /// Get the executed (<c>do_not_run=false</c>) WorkflowJobNode from the WorkflowJob's <paramref name="id"/> and
        /// Get the Id and Type of the Job.
        /// If the Job is a WorkflowJob, get it recursively.
        /// </summary>
        /// <param name="id"></param>
        private void GetJobsFromWorkflowJob(ulong id)
        {
            var query = HttpUtility.ParseQueryString("do_not_run=false&order_by=modified&page_size=20");
            foreach (var resultSet in GetResultSet<WorkflowJobNode>($"{WorkflowJob.PATH}{id}/workflow_nodes/?{query}", true))
            {
                foreach (var node in resultSet.Results)
                {
                    if (node.Job is null || node.SummaryFields.Job is null)
                    {
                        continue;
                    }
                    var jobId = (ulong)node.Job;
                    var type = node.SummaryFields.Job.Type;
                    switch (type)
                    {
                        case ResourceType.WorkflowJob:
                            GetJobsFromWorkflowJob(jobId);
                            break;
                        case ResourceType.WorkflowApproval:
                            break;
                        default:
                            if (_jobIdSet.Add(node.Id))
                            {
                                _jobs.Add(node.SummaryFields.Job);
                            }
                            break;
                    }
                }
            }
        }

        protected override void BeginProcessing()
        {
            if (Id > 0 && Type > 0)
            {
                Job = new Resource(Type, Id);
            }

            if (Download is null)
            {
                if (CommandRuntime.Host?.UI.SupportsVirtualTerminal ?? false)
                {
                    if (Format == JobLogFormat.NotSpecified)
                    {
                        Format = JobLogFormat.ansi;
                    }
                }
                else if (Format == JobLogFormat.ansi)
                {
                    WriteWarning("Your terminal does not support Virtual Terminal.");
                    WriteWarning("Change format to \"txt\".");
                    Format = JobLogFormat.txt;
                }
            }
            else
            {
                if (!Download.Exists)
                {
                    throw new DirectoryNotFoundException($"Download directory is not found: {Download.FullName}");
                }
                if (Format == JobLogFormat.NotSpecified)
                {
                    Format = JobLogFormat.txt;
                }
                else if (Format == JobLogFormat.ansi)
                {
                    WriteWarning($"Download text should not contain VT100 Escape Sequence.");
                    WriteWarning($"Download as \"txt\".");
                    Format = JobLogFormat.txt;
                }
            }
            Query.Add("format", $"{Format}");
            Query.Add("dark", Dark ? "1" : "0");
        }
        protected override void ProcessRecord()
        {
            if (Job.Id == 0)
            {
                return;
            }

            switch (Job.Type)
            {
                case ResourceType.WorkflowJob:
                    GetJobsFromWorkflowJob(Job.Id);
                    break;
                default:
                    if (_jobIdSet.Add(Job.Id))
                    {
                        _jobs.Add(Job);
                    }
                    break;
            }
        }
        private static string GetStdoutPath(ulong id, ResourceType type)
        {
            return type switch
            {
                ResourceType.Job => $"{JobTemplateJob.PATH}{id}/stdout/",
                ResourceType.ProjectUpdate => $"{ProjectUpdateJob.PATH}{id}/stdout/",
                ResourceType.InventoryUpdate => $"{InventoryUpdateJob.PATH}{id}/stdout/",
                ResourceType.AdHocCommand => $"{AdHocCommand.PATH}{id}/stdout/",
                ResourceType.SystemJob => $"{SystemJob.PATH}{id}/",
                _ => throw new NotImplementedException(),
            };
        }
        protected override void EndProcessing()
        {
            if (Download is not null)
            {
                foreach (var fileInfo in DownloadLogs(Download))
                {
                    WriteObject(fileInfo);
                }
            }
            else
            {
                foreach (var log in StdoutLogs(_jobs))
                {
                    WriteObject(log);
                }
            }
        }
        private IEnumerable<string> StdoutLogs(IEnumerable<IResource> jobs)
        {
            foreach (var job in jobs)
            {
                WriteHost($"==> [{job.Id}] {job.Type}\n", foregroundColor: ConsoleColor.Magenta);
                var path = GetStdoutPath(job.Id, job.Type);
                if (job.Type == ResourceType.SystemJob)
                {
                    var systemJob = GetResource<SystemJob.Detail>(path);
                    yield return systemJob.ResultStdout;
                    continue;
                }
                if (Format == JobLogFormat.json)
                {
                    var jsonLog = GetResource<JobLog>($"{path}?{Query}");
                    yield return jsonLog.Content;
                }
                else
                {
                    var acceptType = Format == JobLogFormat.html ? AcceptType.Html : AcceptType.Text;
                    var stringLog = GetResource<string>($"{path}?{Query}", acceptType);
                    yield return stringLog;
                }

            }
        }
        private IEnumerable<FileInfo> DownloadLogs(DirectoryInfo dir)
        {
            var unifiedJobsTask = UnifiedJob.Get(_jobs.Select(static job => job.Id).ToArray());
            unifiedJobsTask.Wait();
            foreach (var unifiedJob in unifiedJobsTask.Result)
            {
                if (unifiedJob is ISystemJob systemJob)
                {
                    yield return WriteSystemLog(dir, systemJob);
                    continue;
                }
                switch (Format)
                {
                    case JobLogFormat.json:
                        yield return WriteLogAsJson(dir, unifiedJob);
                        break;
                    case JobLogFormat.txt:
                    case JobLogFormat.ansi:
                        yield return WriteLogAsText(dir, unifiedJob);
                        break;
                    case JobLogFormat.html:
                        yield return WriteLogAsHtml(dir, unifiedJob);
                        break;
                    default:
                        throw new InvalidOperationException($"Unkown format: {Format}");
                }
            }
        }
        private FileInfo WriteLogAsJson(DirectoryInfo dir, IUnifiedJob unifiedJob)
        {
            FileInfo fileInfo = new(Path.Combine(dir.FullName, $"{unifiedJob.Id}.json"));
            using FileStream fileStream = fileInfo.OpenWrite();
            var path = GetStdoutPath(unifiedJob.Id, unifiedJob.Type);
            var jsonLog = GetResource<JobLog>($"{path}?{Query}");
            JsonSerializer.Serialize(fileStream, jsonLog, Json.SerializeOptions);
            return fileInfo;
        }
        private FileInfo WriteSystemLog(DirectoryInfo dir, ISystemJob systemJob)
        {
            FileInfo fileInfo = new(Path.Combine(dir.FullName, $"{systemJob.Id}.txt"));
            using FileStream fileStream = fileInfo.Open(fileInfo.Exists ? FileMode.Truncate : FileMode.CreateNew,
                                                        FileAccess.Write);
            var txtLog = systemJob.ResultStdout;
            using var ws = new StreamWriter(fileStream, Encoding.UTF8);

            ws.WriteLine("-----");
            var props = typeof(ISystemJob).GetProperties(BindingFlags.Public);
            var maxLength = props.Select(static p => p.Name.Length).Max();
            var format = $"{{0,{maxLength}}}: {{1}}";
            foreach (var prop in props)
            {
                var value = prop.GetValue(systemJob);
                var val = value switch
                {
                    IList or IDictionary => Json.Stringify(value),
                    _ => value
                };
                ws.WriteLine(format, prop.Name, val);
            }
            ws.WriteLine("-----");
            ws.WriteLine(txtLog);
            ws.Close();
            return fileInfo;
        }
        private FileInfo WriteLogAsText(DirectoryInfo dir, IUnifiedJob unifiedJob)
        {
            FileInfo fileInfo = new(Path.Combine(dir.FullName, $"{unifiedJob.Id}.txt"));
            using FileStream fileStream = fileInfo.Open(fileInfo.Exists ? FileMode.Truncate : FileMode.CreateNew,
                                                        FileAccess.Write);
            var path = GetStdoutPath(unifiedJob.Id, unifiedJob.Type);
            var txtLog = GetResource<string>($"{path}?{Query}", AcceptType.Text);
            using var ws = new StreamWriter(fileStream, Encoding.UTF8);

            ws.WriteLine("-----");
            var props = GetJobProperties(unifiedJob).ToArray();
            var maxLength = props.Select(static tuple => tuple.Key.Length).Max();
            var format = $"{{0,{maxLength}}}: {{1}}";
            foreach (var (key, value) in props)
            {
                var val = value switch
                {
                    IList or IDictionary => Json.Stringify(value),
                    _ => value
                };
                ws.WriteLine(format, key, val);
            }
            ws.WriteLine("-----");
            ws.WriteLine(txtLog);
            ws.Close();
            return fileInfo;
        }
        private FileInfo WriteLogAsHtml(DirectoryInfo dir, IUnifiedJob unifiedJob)
        {
            FileInfo fileInfo = new(Path.Combine(dir.FullName, $"{unifiedJob.Id}.html"));
            using FileStream fileStream = fileInfo.Open(fileInfo.Exists ? FileMode.Truncate : FileMode.CreateNew,
                                                        FileAccess.Write);
            var path = GetStdoutPath(unifiedJob.Id, unifiedJob.Type);
            var htmlLog = GetResource<string>($"{path}?{Query}", AcceptType.Html);
            var title = $"{unifiedJob.Id} - {HttpUtility.HtmlEncode(unifiedJob.Name)}";

            // Create Job Info Table
            var jobInfo = new StringBuilder();
            var props = typeof(IUnifiedJob).GetProperties(BindingFlags.Public);
            var format = "<tr><th>{0}</th><td>{1}</td></tr>";
            jobInfo.AppendLine("<table style=\"font-size: 12px\"><caption>Job Info</caption>");
            foreach ((string key, object? value) in GetJobProperties(unifiedJob))
            {
                var val = value switch
                {
                    IList or IDictionary => Json.Stringify(value),
                    _ => value
                };
                jobInfo.AppendFormat(CultureInfo.CurrentCulture, format, key, HttpUtility.HtmlEncode(val))
                       .AppendLine();
            }
            jobInfo.AppendLine("</table>");

            // Write Log to a fileStream as HTML
            using StreamWriter ws = new(fileStream, Encoding.UTF8);
            if (htmlLog is not null)
            {
                int bodyTagStart = htmlLog.IndexOf("<body", StringComparison.Ordinal);
                if (bodyTagStart > 0)
                {
                    int bodyTagEnd = htmlLog.IndexOf('>', bodyTagStart) + 1;
                    ws.WriteLine(htmlLog[..bodyTagEnd].Replace("<title>Type</title>", $"<title>{title}</title>"));
                    ws.WriteLine(jobInfo.ToString());
                    ws.WriteLine(htmlLog[bodyTagEnd..]);
                    ws.Close();
                    return fileInfo;
                }
            }
            ws.WriteLine("<html>");
            ws.WriteLine($"<head><meta charset=\"utf-8\"><title>{title}</title></head>");
            ws.WriteLine("<body>");
            ws.WriteLine(jobInfo.ToString());
            ws.WriteLine("<p>Ooops, Missing log data :(</p>");
            ws.WriteLine("</body></html>");
            ws.Close();
            return fileInfo;
        }
        private static IEnumerable<(string Key, object? Value)> GetJobProperties(IUnifiedJob job)
        {
            foreach (var prop in typeof(IUnifiedJob).GetProperties())
            {
                yield return (prop.Name, prop.GetValue(job));
            }
            var type = job switch
            {
                IJobTemplateJob => typeof(IJobTemplateJob),
                IProjectUpdateJob => typeof(IProjectUpdateJob),
                IInventoryUpdateJob => typeof(IInventoryUpdateJob),
                ISystemJob => typeof(ISystemJob),
                _ => null
            };
            if (type is null)
            {
                yield break;
            }
            foreach (var prop in type.GetProperties())
            {
                yield return (prop.Name, prop.GetValue(job));
            }
        }
    }
}
