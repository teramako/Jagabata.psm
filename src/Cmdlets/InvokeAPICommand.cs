using Jagabata.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text.Json;
using System.Web;

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
            var query = HttpUtility.ParseQueryString(QueryString);
            NameValueCollection? queryInPath = null;
            if (Path.IndexOf('?') > 0)
            {
                var buf = Path.Split('?', 2);
                Path = buf[0];
                queryInPath = HttpUtility.ParseQueryString(buf[1]);
                queryInPath.Add(query);
                pathAndQuery = $"{Path}?{queryInPath}";
                return;
            }
            if (query.Count > 0)
            {
                pathAndQuery = $"{Path}?{query}";
            }
            else
            {
                pathAndQuery = Path;
            }
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
                    throw new ArgumentNullException(nameof(SenData));
                case Method.PATCH:
                    if (SenData is not null)
                    {
                        var task = RestAPI.PatchJsonAsync<string>(pathAndQuery, SenData);
                        task.Wait();
                        return task.Result;
                    }
                    throw new ArgumentNullException(nameof(SenData));
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

    class ApiPathCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName,
                                                              string wordToComplete, CommandAst commandAst,
                                                              IDictionary fakeBoundParameters)
        {
            var paths = wordToComplete.Split('/');
            Method method = Method.GET;
            if (fakeBoundParameters.Contains("Method"))
            {
                var param = fakeBoundParameters["Method"] as string;
                Enum.TryParse<Method>(param, true, out method);
            }
            switch (paths.Length)
            {
                case <= 2:
                    foreach (var item in Complete()) { yield return item; }
                    break;
                case 3:
                    foreach (var item in Complete(paths[2])) { yield return item; }
                    break;
                case 4:
                    foreach (var item in Complete(method, paths[3])) { yield return item; }
                    break;
                case 5:
                    foreach (var item in Complete(method, paths[3], paths[4])) { yield return item; }
                    break;
                case 6:
                    foreach (var item in Complete(method, paths[3], paths[4], paths[5])) { yield return item; }
                    break;
            }
        }
        private static IEnumerable<CompletionResult> Complete(string p2 = "")
        {
            string[] paths = ["v2", "o"];
            foreach (var path in paths)
            {
                if (!string.IsNullOrEmpty(p2) && !path.StartsWith(p2))
                    continue;
                yield return new CompletionResult($"/api/{path}/");
            }
        }
        private static IEnumerable<CompletionResult> Complete(Method method, string p3)
        {
            foreach (var field in typeof(ResourceType).GetFields())
            {
                foreach (var attr in field.GetCustomAttributes<ResourcePathAttribute>(false))
                {
                    if (attr.Virtual || method != attr.Method)
                    {
                        if (!field.GetCustomAttributes<ResourceSubPathBase>(false)
                                  .Where(attr => attr.Method == method)
                                  .Any())
                        {
                            continue;
                        }
                    }
                    if (attr.PathName.StartsWith(p3))
                    {
                        var text = $"/api/v2/{attr.PathName}/";
                        var tooltip = string.IsNullOrEmpty(attr.Description)
                                      ? $"{method} {field.Name}"
                                      : attr.Description;
                        yield return new CompletionResult(text, attr.PathName, CompletionResultType.ParameterValue,
                                                          tooltip);
                        break;
                    }
                }

            }
        }
        private static IEnumerable<CompletionResult> Complete(Method method, string p3, string p4)
        {
            ResourcePathAttribute? p3Attr = null;
            FieldInfo? resourceField = null;

            foreach (var field in typeof(ResourceType).GetFields())
            {
                var attr = field.GetCustomAttributes<ResourcePathAttribute>(false).FirstOrDefault();
                if (attr is null) continue;
                if (attr.PathName == p3)
                {
                    resourceField = field;
                    p3Attr = attr;
                    break;
                }
            }
            if (p3Attr is null) yield break;
            if (resourceField is null) yield break;
            var p4IsId = ulong.TryParse(p4, out _);

            var subPathAttrs = resourceField.GetCustomAttributes<ResourceSubPathBase>(false)
                                            .Where(attr => attr.Method == method)
                                            .ToArray();
            if (subPathAttrs.Length == 0) yield break;

            foreach (var subAttr in subPathAttrs)
            {
                var tooltip = string.IsNullOrEmpty(subAttr.Description)
                              ? $"{method} {resourceField.Name}"
                              : subAttr.Description;
                string completeionText;
                string listItemText;
                switch (subAttr)
                {
                    case ResourceIdPathAttribute:
                        if (!p4IsId) continue;
                        // TODO: 最終的にはID番号の補完もしたい
                        completeionText = $"/api/v2/{p3}/{p4}/";
                        listItemText = $"{p3}/{p4}/";
                        yield return new CompletionResult(completeionText, listItemText,
                                                          CompletionResultType.ParameterValue, tooltip);
                        yield break;
                    case ResourceSubPathAttribute subPathAttr:
                        if (p4IsId && subPathAttr.IsSubPathOfId)
                        {
                            completeionText = $"/api/v2/{p3}/{p4}/{subPathAttr.PathName}";
                            listItemText = $"{p3}/{p4}/{subPathAttr.PathName}";
                        }
                        else if (!p4IsId && !subPathAttr.IsSubPathOfId)
                        {
                            completeionText = $"/api/v2/{p3}/{subPathAttr.PathName}/";
                            listItemText = $"{p3}/{subPathAttr.PathName}/";
                        }
                        else
                        {
                            continue;
                        }
                        yield return new CompletionResult(completeionText, listItemText,
                                                          CompletionResultType.ParameterValue, tooltip);
                        break;
                    default:
                        continue;
                }
            }
        }
        private static IEnumerable<CompletionResult> Complete(Method method, string p3, string p4, string p5)
        {
            ResourcePathAttribute? p3Attr = null;
            FieldInfo? resourceField = null;

            foreach (var field in typeof(ResourceType).GetFields())
            {
                var attr = field.GetCustomAttributes<ResourcePathAttribute>(false).FirstOrDefault();
                if (attr is null) continue;
                if (attr.PathName == p3)
                {
                    resourceField = field;
                    p3Attr = attr;
                    break;
                }
            }

            if (p3Attr is null) yield break;
            if (resourceField is null) yield break;
            var p4IsId = ulong.TryParse(p4, out _);

            foreach (var subPathAttr in resourceField.GetCustomAttributes<ResourceSubPathAttribute>(false)
                                                     .Where(attr => attr.Method == method))
            {
                if (p4IsId != subPathAttr.IsSubPathOfId) continue;
                var compWord = p4IsId ? p5 : $"{p4}/{p5}";
                if (subPathAttr.PathName.StartsWith(compWord))
                {
                    var text = $"/api/v2/{p3}/{p4}/{subPathAttr.PathName}/";
                    var tooltip = string.IsNullOrEmpty(subPathAttr.Description)
                                  ? $"{method} {resourceField.Name}"
                                  : subPathAttr.Description;
                    yield return new CompletionResult(text, subPathAttr.PathName, CompletionResultType.ParameterValue,
                                                      tooltip);
                }

            }

        }
    }
}
