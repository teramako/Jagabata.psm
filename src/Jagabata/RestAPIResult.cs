using System.Collections.Frozen;
using System.Net;

namespace Jagabata
{
    public interface IRestAPIResponse
    {
        bool IsSuccessStatusCode { get; }
        HttpStatusCode StatusCode { get; }
        string ReasonPhrase { get; }
        Version HttpVersion { get; }
        string Method { get; }
        Uri? RequestUri { get; }
        string ContentType { get; }
        long ContentLength { get; }
        FrozenDictionary<string, IEnumerable<string>>? RequestHeaders { get; }
        FrozenDictionary<string, IEnumerable<string>> ResponseHeaders { get; }
        FrozenDictionary<string, IEnumerable<string>> ContentHeaders { get; }
        string ToString();
    }
    public class RestAPIResponse(HttpResponseMessage response) : IRestAPIResponse
    {
        public bool IsSuccessStatusCode { get; } = response.IsSuccessStatusCode;
        public HttpStatusCode StatusCode { get; } = response.StatusCode;
        public string ReasonPhrase { get; } = response.ReasonPhrase ?? string.Empty;
        public Version HttpVersion { get; } = response.Version;
        public string Method { get; } = response.RequestMessage?.Method.ToString() ?? string.Empty;
        public Uri? RequestUri { get; } = response.RequestMessage?.RequestUri;
        public string ContentType { get; } = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
        public long ContentLength { get; } = response.Content.Headers.ContentLength ?? 0;
        public FrozenDictionary<string, IEnumerable<string>>? RequestHeaders { get; } = response.RequestMessage?.Headers.ToFrozenDictionary();
        public FrozenDictionary<string, IEnumerable<string>> ResponseHeaders { get; } = response.Headers.ToFrozenDictionary();
        public FrozenDictionary<string, IEnumerable<string>> ContentHeaders { get; } = response.Content.Headers.ToFrozenDictionary();
        public override string ToString()
        {
            return $"({StatusCode:d} {ReasonPhrase}: {Method} {RequestUri?.PathAndQuery})";
        }
    }
    public interface IRestAPIResult<TValue>
    {
        /// <summary>
        /// Json deserialized object or contents string.
        /// </summary>
        TValue? Contents { get; }
        /// <summary>
        /// HTTP response
        /// </summary>
        RestAPIResponse Response { get; }
    }

    /// <summary>
    /// The contents of <see cref="RestAPI"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestAPIResult<T>(HttpResponseMessage response, T contents) : IRestAPIResult<T>
    {
        public T Contents { get; } = contents;
        public RestAPIResponse Response { get; } = new RestAPIResponse(response);
        public override string ToString()
        {
            return $"{typeof(T).Name} ({Response.StatusCode:d} {Response.ReasonPhrase}: {Response.Method} {Response.RequestUri?.PathAndQuery})";
        }
    }

    /// <summary>
    /// The contents of <see cref="RestAPI"/>.
    /// The response contents may be null especially HTTP status code is 204 (NotContent).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestAPIPostResult<T>(HttpResponseMessage response, T? contents = default(T)) : IRestAPIResult<T>
    {
        public T? Contents { get; } = contents;
        public RestAPIResponse Response { get; } = new RestAPIResponse(response);
        public override string ToString()
        {
            return $"{typeof(T).Name} ({Response.StatusCode:d} {Response.ReasonPhrase}: {Response.Method} {Response.RequestUri?.PathAndQuery})";
        }
    }
}
