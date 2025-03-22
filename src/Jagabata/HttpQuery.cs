using System.Collections;
using System.Collections.Specialized;
using System.Web;

namespace Jagabata;

public enum QueryCount : uint
{
    Infinity = 0,
    Default = 1,
}

/// <summary>
/// HTTP Query container for AWX/AnsibleTower Rest API.
/// <para>
/// This is wrapper class for <see cref="NameValueCollection"/> generated from <see cref="HttpUtility.ParseQueryString(string)"/>.
/// Primarily used from an instance created by <see cref="QueryBuilder.Build"/>
/// </para>
/// </summary>
public sealed class HttpQuery : NameValueCollection
{
    internal HttpQuery(NameValueCollection queries, uint queryCount = 1)
    {
        _queries = queries;
        QueryCount = queryCount;
    }
    public HttpQuery(string queries, uint queryCount = 1)
    {
        _queries = HttpUtility.ParseQueryString(queries);
        QueryCount = queryCount;
    }
    public HttpQuery(string queries, QueryCount queryCount) : this(queries, (uint)queryCount)
    { }

    public HttpQuery(uint queryCount = 1) : this(string.Empty, queryCount)
    { }

    public HttpQuery(QueryCount queryCount) : this(string.Empty, (uint)queryCount)
    { }

    public HttpQuery() : this(string.Empty)
    { }

    /// <summary>
    /// <see cref="NameValueCollection"/> object created by <see cref="HttpUtility.ParseQueryString(string)"/>
    /// </summary>
    private readonly NameValueCollection _queries;

    /// <summary>
    /// Query count of times (Default = 1)
    /// <c>0</c> means infinity.
    /// </summary>
    public uint QueryCount { get; set; }

    public bool IsInfinity => QueryCount == 0;

    public void SetQueryCount(QueryCount queryCount)
    {
        QueryCount = (uint)queryCount;
    }

    public override int Count => _queries.Count;

    public override KeysCollection Keys => _queries.Keys;

    public override string?[] AllKeys => _queries.AllKeys;

    public override void Add(string? name, string? value)
    {
        _queries.Add(name, value);
    }

    public new void Add(NameValueCollection c)
    {
        _queries.Add(c);
    }

    public override void Clear()
    {
        _queries.Clear();
    }

    public override bool Equals(object? obj)
    {
        return _queries.Equals(obj);
    }

    public override string? Get(int index)
    {
        return _queries.Get(index);
    }

    public override string? Get(string? name)
    {
        return _queries.Get(name);
    }

    public override IEnumerator GetEnumerator()
    {
        return _queries.GetEnumerator();
    }

    public override int GetHashCode()
    {
        return _queries.GetHashCode();
    }

    public override string? GetKey(int index)
    {
        return _queries.GetKey(index);
    }

    public override string[]? GetValues(int index)
    {
        return _queries.GetValues(index);
    }

    public override string[]? GetValues(string? name)
    {
        return _queries.GetValues(name);
    }

    public override void OnDeserialization(object? sender)
    {
        _queries.OnDeserialization(sender);
    }

    public override void Remove(string? name)
    {
        _queries.Remove(name);
    }

    public override void Set(string? name, string? value)
    {
        _queries.Set(name, value);
    }

    public override string? ToString()
    {
        return _queries.ToString();
    }
}

