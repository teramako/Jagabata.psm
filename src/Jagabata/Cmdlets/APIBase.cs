using Jagabata.Resources;
using System.Collections.Frozen;
using System.Management.Automation;

namespace Jagabata.Cmdlets;

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
        WriteInformation(msg, [.. infoTags]);
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

            // Add a resource to the cache for completion
            if (result.Contents is IResource res)
            {
                Caches.Add(res);
            }

            return result.Contents;
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
    }
    /// <summary>
    /// Send requests to retrieve resource list.
    /// (HTTP method: <c>GET</c>)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="path"></param>
    /// <param name="query"></param>
    /// <returns>Returns successed responses</returns>
    protected IEnumerable<ResultSet<TValue>> GetResultSet<TValue>(string path, HttpQuery? query = null)
        where TValue : class
    {
        query ??= [];
        var nextPathAndQuery = query.Count == 0 ? path : $"{path}?{query}";
        var count = 0;
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

            // Add resources to the cache for completion
            Caches.Add(resultSet.Results.OfType<IResource>());

            yield return resultSet;

            nextPathAndQuery = resultSet.Next;
        }
        while ((query.IsInfinity || ++count < query.QueryCount)
               && !string.IsNullOrEmpty(nextPathAndQuery));
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

            // Add a resource to the cache for completion
            if (result.Contents is IResource res)
            {
                Caches.Add(res);
            }
            return result;
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

            // Add a resource to the cache for completion
            if (result.Contents is IResource res)
            {
                Caches.Add(res);
            }

            return result.Contents;
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

            // Add cache for completion
            if (result.Contents is IResource res)
            {
                Caches.Add(res);
            }

            return result.Contents;
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
