using System.Management.Automation;
using System.Text.Json;
using Jagabata.Cmdlets.Completer;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "API", DefaultParameterSetName = "NonSendData")]
    public class InvokeAPICommand : APICmdletBase
    {
        [Parameter(Mandatory = true, Position = 0)]
        public Method Method { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ArgumentCompleter(typeof(ApiPathCompleter))]
        public string Path { get; set; } = string.Empty;

        [Parameter(Position = 2)]
        public string QueryString { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "SendData", Mandatory = true, ValueFromPipeline = true)]
        public object? SenData { get; set; }

        [Parameter()]
        public SwitchParameter AsRawString { get; set; }

        private string pathAndQuery = string.Empty;

        protected override void BeginProcessing()
        {
            var query = new HttpQuery(QueryString);
            if (Path.IndexOf('?') > 0)
            {
                var buf = Path.Split('?', 2);
                Path = buf[0];
                var queryInPath = new HttpQuery(buf[1]) { query };
                pathAndQuery = $"{Path}?{queryInPath}";
                return;
            }
            pathAndQuery = query.Count > 0 ? $"{Path}?{query}" : Path;
        }
        private IRestAPIResult<string> InvokeAPI(string pathAndQuery)
        {
            switch (Method)
            {
                case Method.GET:
                    {
                        var task = RestAPI.GetAsync<string>(pathAndQuery);
                        task.Wait();
                        return task.Result;
                    }
                case Method.POST:
                    {
                        var task = RestAPI.PostJsonAsync<string>(pathAndQuery, SenData);
                        task.Wait();
                        return task.Result;
                    }
                case Method.PUT:
                    if (SenData is not null)
                    {
                        var task = RestAPI.PutJsonAsync<string>(pathAndQuery, SenData);
                        task.Wait();
                        return task.Result;
                    }
                    throw new InvalidDataException($"Should specify {nameof(SenData)}");
                case Method.PATCH:
                    if (SenData is not null)
                    {
                        var task = RestAPI.PatchJsonAsync<string>(pathAndQuery, SenData);
                        task.Wait();
                        return task.Result;
                    }
                    throw new InvalidDataException($"Should specify {nameof(SenData)}");
                case Method.DELETE:
                    {
                        var task = RestAPI.DeleteAsync(pathAndQuery);
                        task.Wait();
                        return task.Result;
                    }
                case Method.OPTIONS:
                    {
                        var task = RestAPI.OptionsJsonAsync<string>(pathAndQuery);
                        task.Wait();
                        return task.Result;
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(pathAndQuery)) { return; }
            WriteVerboseRequest(pathAndQuery, Method);

            var result = InvokeAPI(pathAndQuery);
            WriteVerboseResponse(result.Response);
            if (!AsRawString && result.Response.ContentType == "application/json" && result.Contents is not null)
            {
                if (Utils.TryGetTypeFromPath(pathAndQuery, Method, out var type))
                {
                    if (type != typeof(string))
                    {
                        var obj = JsonSerializer.Deserialize(result.Contents, type, Json.DeserializeOptions);
                        WriteObject(obj, true);
                        return;
                    }
                }
                else
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(result.Contents, Json.DeserializeOptions);
                    try
                    {
                        var obj = Json.ObjectToInferredType(json, true);
                        WriteObject(obj, false);
                        return;
                    }
                    catch (Exception ex)
                    {
                        WriteWarning($"Could not convert to inferred type. Fallback to string: {ex.Message}");
                    }
                }
            }
            WriteObject(result.Contents);
        }
    }
}
