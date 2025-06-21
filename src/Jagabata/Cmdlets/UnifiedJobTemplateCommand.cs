using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "UnifiedJobTemplate")]
    [OutputType(typeof(IUnifiedJobTemplate))]
    public class FindUnifiedJobTemplateCommand : FindCommandBase
    {
        [Parameter()]
        [OrderByCompletionFromHelp(ResourceType.UnifiedJobTemplate, UnifiedJobTemplate.PATH)]
        public override string[] OrderBy { get; set; } = ["id"];

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
            } while ((query.IsInfinity || ++count < query.QueryCount)
                     && !string.IsNullOrEmpty(nextPathAndQuery));
        }
        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            foreach (var resultSet in GetResultSet(UnifiedJobTemplate.PATH, Query.Build()))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }
}
