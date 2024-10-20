using Jagabata.Resources;
using System.Management.Automation;
using System.Web;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Dashboard")]
    [OutputType(typeof(Dashboard))]
    public class GetDashboardCommand : APICmdletBase
    {
        protected override void ProcessRecord()
        {
            var apiResult = GetResource<Dashboard>(Dashboard.PATH);
            WriteObject(apiResult, false);
        }
    }

    [Cmdlet(VerbsCommon.Get, "JobStatistics")]
    [OutputType(typeof(JobStatistics))]
    public class GetJobStatisticsCommand : APICmdletBase
    {
        [Parameter(Position = 0)]
        [ValidateSet("month", "two_weeks", "week", "day")]
        public string Period { get; set; } = string.Empty;

        [Parameter(Position = 1)]
        [ValidateSet("all", "inv_sync", "playbook_run", "scm_update")]
        public string JobType { get; set; } = string.Empty;

        protected override void BeginProcessing()
        {
            var query = HttpUtility.ParseQueryString("");
            if (!string.IsNullOrEmpty(Period))
            {
                query.Set("period", Period);
            }
            if (!string.IsNullOrEmpty(JobType))
            {
                query.Set("job_type", JobType);
            }
            var path = JobStatisticsContainer.PATH;
            if (query.Count > 0)
            {
                path += $"?{query}";
            }
            var apiResult = GetResource<JobStatisticsContainer>(path);
            WriteObject(apiResult.Jobs, false);
        }
    }
}
