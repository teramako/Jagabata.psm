using AWX.Resources;
using System.Collections.Specialized;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "UnifiedJobTemplate")]
    [OutputType(typeof(IUnifiedJobTemplate))]
    public class FindUnifiedJobTemplateCommand : FindCommandBase
    {
        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

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
                    switch (aex.InnerException)
                    {
                        case RestAPIException ex:
                            WriteVerboseResponse(ex.Response);
                            throw ex;
                        case HttpRequestException:
                            throw aex.InnerException;
                        default:
                            throw;
                    }
                }
                var resultSet = result.Contents;

                yield return resultSet;

                nextPathAndQuery = string.IsNullOrEmpty(resultSet?.Next) ? string.Empty : resultSet.Next;
            } while (getAll && !string.IsNullOrEmpty(nextPathAndQuery));
        }
        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            foreach (var resultSet in GetResultSet(UnifiedJobTemplate.PATH, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }
}
