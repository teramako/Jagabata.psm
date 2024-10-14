using AWX.Resources;
using System.Collections.Frozen;
using System.Collections.Specialized;
using System.Management.Automation;

namespace AWX.Cmdlets;

public abstract class APICmdletBase : Cmdlet
{
    private readonly ConsoleColor DefaultForegroundColor = Console.ForegroundColor;
    private readonly ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;
    /// <summary>
    /// Write message to the console as Information
    /// </summary>
    /// <param name="message"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="tags"></param>
    /// <param name="dontshow">Follow the <c>$InformationPreference</c> value, don't output to console forcely.</param>
    protected void WriteHost(string message,
                              ConsoleColor? foregroundColor = null,
                              ConsoleColor? backgroundColor = null,
                              string[]? tags = null,
                              bool dontshow = false)
    {
        var msg = new HostInformationMessage()
        {
            Message = message,
            ForegroundColor = foregroundColor ?? DefaultForegroundColor,
            BackgroundColor = backgroundColor ?? DefaultBackgroundColor,
            NoNewLine = true
        };
        List<string> infoTags = dontshow ? [] : ["PSHOST"];
        if (tags is not null) { infoTags.AddRange(tags); }
        WriteInformation(msg, infoTags.ToArray());
    }
    /// <summary>
    /// Get <c>PATH</c> field value in the Resource class
    /// </summary>
    /// <returns>API Path</returns>
    /// <exception cref="NullReferenceException">throw if <c>PATH</c> is not foudn or the value is null</exception>
    protected string GetApiPath(Type t)
    {
        if (t.IsNested && t.ReflectedType is not null)
        {
            t = t.ReflectedType;
        }
        var pathField = t.GetField("PATH", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            ?? throw new NullReferenceException($"Type {t.FullName} has no 'PATH' field.");
        var apiPath = pathField.GetValue(t) as string
            ?? throw new NullReferenceException($"Type {t.FullName}'s PATH is not string ?");
        return apiPath;
    }
    /// <summary>
    /// Send a request to retrieve a resource.
    /// (HTTP method: <c>GET</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="pathAndQuery"></param>
    /// <param name="acceptType"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    protected TValue GetResource<TValue>(string pathAndQuery, AcceptType acceptType = AcceptType.Json)
        where TValue : class
    {
        WriteVerboseRequest(pathAndQuery, Method.GET);
        try
        {
            using var apiTask = RestAPI.GetAsync<TValue>(pathAndQuery, acceptType);
            apiTask.Wait();
            var result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result.Contents;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    protected IEnumerable<ResultSet<TValue>> GetResultSet<TValue>(string path,
                                                                  NameValueCollection? query = null,
                                                                  bool getAll = false)
        where TValue : class
    {
        var pathAndQuery = path + (query is null ? "" : $"?{query}");
        foreach (var resultSet in GetResultSet<TValue>(pathAndQuery, getAll))
        {
            yield return resultSet;
        }
    }
    /// <summary>
    /// Send requests to retrieve resource list.
    /// (HTTP method: <c>GET</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="pathAndQuery"></param>
    /// <param name="getAll"></param>
    /// <returns>Returns successed responses</returns>
    protected IEnumerable<ResultSet<TValue>> GetResultSet<TValue>(string pathAndQuery, bool getAll = false)
        where TValue : class
    {
        string nextPathAndQuery = pathAndQuery;
        do
        {
            WriteVerboseRequest(nextPathAndQuery, Method.GET);
            RestAPIResult<ResultSet<TValue>>? result;
            try
            {
                using var apiTask = RestAPI.GetAsync<ResultSet<TValue>>(nextPathAndQuery);
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
    /// <summary>
    /// Send a request to create the resource.
    /// (HTTP method: <c>POST</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="pathAndQuery"></param>
    /// <param name="sendData"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    /// <exception cref="RestAPIException"/>
    protected RestAPIPostResult<TValue> CreateResource<TValue>(string pathAndQuery, object? sendData = null)
        where TValue : class
    {
        WriteVerboseRequest(pathAndQuery, Method.POST);
        try
        {
            using var apiTask = RestAPI.PostJsonAsync<TValue>(pathAndQuery, sendData);
            apiTask.Wait();
            RestAPIPostResult<TValue> result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    /// <summary>
    /// Send a request to replace the resource.
    /// (HTTP method: <c>PUT</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="pathAndQuery"></param>
    /// <param name="sendData"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    protected TValue UpdateResource<TValue>(string pathAndQuery, object sendData)
        where TValue : class
    {
        WriteVerboseRequest(pathAndQuery, Method.PUT);
        try
        {
            using var apiTask = RestAPI.PutJsonAsync<TValue>(pathAndQuery, sendData);
            apiTask.Wait();
            RestAPIResult<TValue> result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result.Contents;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    /// <summary>
    /// Send a request to modify part of the resource.
    /// (HTTP method: <c>PATCH</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="pathAndQuery"></param>
    /// <param name="sendData"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    protected TValue PatchResource<TValue>(string pathAndQuery, object sendData)
        where TValue : class
    {
        WriteVerboseRequest(pathAndQuery, Method.PATCH);
        try
        {
            using var apiTask = RestAPI.PatchJsonAsync<TValue>(pathAndQuery, sendData);
            apiTask.Wait();
            RestAPIResult<TValue> result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result.Contents;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    /// <summary>
    /// Send a request to delete the resource.
    /// (HTTP method <c>DELETE</c>)
    /// </summary>
    /// <param name="pathAndQuery"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    protected IRestAPIResponse DeleteResource(string pathAndQuery)
    {
        WriteVerboseRequest(pathAndQuery, Method.DELETE);
        try
        {
            using var apiTask = RestAPI.DeleteAsync(pathAndQuery);
            apiTask.Wait();
            RestAPIResult<string> result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result.Response;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    /// <summary>
    /// Send and get a API Help of the URI.
    /// (HTTP method <c>OPTIONS</c>)
    /// </summary>
    /// <param name="pathAndQuery"></param>
    /// <returns>Return the result if success, otherwise null</returns>
    protected ApiHelp GetApiHelp(string pathAndQuery)
    {
        WriteVerboseRequest(pathAndQuery, Method.OPTIONS);
        try
        {
            using var apiTask = RestAPI.OptionsJsonAsync<ApiHelp>(pathAndQuery);
            apiTask.Wait();
            RestAPIResult<ApiHelp> result = apiTask.Result;
            WriteVerboseResponse(result.Response);
            return result.Contents;
        }
        catch (RestAPIException ex)
        {
            WriteVerboseResponse(ex.Response);
            throw;
        }
        catch (AggregateException aex)
        {
            switch(aex.InnerException)
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
    }
    protected void WriteVerboseRequest(string pathAndQuery, Method method)
    {
        var uri = ApiConfig.Instance.Origin;
        WriteVerbose($"> Host: {uri.Host}:{uri.Port}");
        WriteVerbose($"> {method} {pathAndQuery}");
    }
    protected void WriteVerboseResponse(IRestAPIResponse response)
    {
        WriteDebugHeaders(response.RequestHeaders, '>');
        WriteVerbose($"HTTP/{response.HttpVersion} {response.StatusCode:d} {response.ReasonPhrase}");
        WriteDebugHeaders(response.ContentHeaders, '<');
        WriteDebugHeaders(response.ResponseHeaders, '<');
    }
    private void WriteDebugHeaders(FrozenDictionary<string, IEnumerable<string>>? headers, char indicator)
    {
        if (headers is null) return;
        foreach (var header in headers)
        {
            if (header.Key == "Authorization")
            {
                WriteDebug($"{indicator} {header.Key}: Bearer ************");
                continue;
            }
            WriteDebug($"{indicator} {header.Key}: {string.Join(", ", header.Value)}");
        }
    }
}
