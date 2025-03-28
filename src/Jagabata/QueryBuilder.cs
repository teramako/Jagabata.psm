using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Jagabata;

/// <summary>
/// HTTP Query builder for AWX/AnsibleTower Rest API.
/// <list type="bullet">
///     <item><see href="https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/sorting.html">Sorting</see></item>
///     <item><see href="https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/filtering.html">Filtering</see></item>
///     <item><see href="https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/pagination.html">Pagenation</see></item>
/// </list>
/// </summary>
public class QueryBuilder : ISpanParsable<QueryBuilder>
{
    public QueryBuilder()
    { }
    public QueryBuilder(ReadOnlySpan<char> query)
    {
        Span<Range> queryRanges = new Range[query.Count('&') + 1];
        var rangeLength = query.Split(queryRanges, '&', StringSplitOptions.RemoveEmptyEntries
                                                        | StringSplitOptions.TrimEntries);
        for (var i = 0; i < rangeLength; i++)
        {
            Add(Filter.Parse(query[queryRanges[i]], CultureInfo.InvariantCulture));
        }
    }
    public QueryBuilder(string query) : this(query.AsSpan())
    { }
    public QueryBuilder(params Filter[] filters)
    {
        foreach (var filter in filters)
        {
            Add(filter);
        }
    }
    public QueryBuilder(NameValueCollection queries)
    {
        for (var i = 0; i < queries.Count; i++)
        {
            var key = queries.Get(i);
            if (key is null)
                continue;

            var values = queries.GetValues(i);
            if (values is null)
                continue;

            foreach (var value in values)
            {
                Add(new Filter(key, value));
            }
        }
    }

    private const int MAX_PAGE_SIZE = 200;
    private const int DEFAULT_PAGE_SIZE = 20;
    private const int DEFAULT_PAGE_START = 0;
    private const int DEFAULT_QUERY_COUNT = 1;

    /// <summary>
    /// For <c>search</c> parameter
    /// </summary>
    public string[] SearchWords { get; set; } = [];

    /// <summary>
    /// Array of <see cref="Filter"/>s
    /// </summary>
    public Filter[] Filters => [.. _filters];
    private readonly List<Filter> _filters = [];

    /// <summary>
    /// Sort keys.
    /// Add the <c>-</c> prefix to sort in descending order.
    /// </summary>
    public string[] OrderBy { get; set; } = [];

    /// <summary>
    /// Amount of page size per 1 time.
    /// (Between 1 and 200; Default = 20)
    /// </summary>
    public ushort PageSize { get => _pageSize; set => SetPageSize(value); }
    private ushort _pageSize;

    /// <summary>
    /// Page number of first time to retrieve. (Default = 0)
    /// </summary>
    public uint StartPage { get => _startPage; set => SetStartPage(value); }
    private uint _startPage;

    /// <summary>
    /// Query count of times (Default = 1)
    /// <c>0</c> means infinity.
    /// </summary>
    public uint QueryCount { get => _queryCount; set => SetQueryCount(value); }
    private uint _queryCount = DEFAULT_QUERY_COUNT;

    public QueryBuilder Add(Filter? filter)
    {
        if (filter is null) // do nothing
            return this;

        switch (filter.Name)
        {
            case "search":
                return SetSearchWords(filter.GetValue());
            case "order_by":
                return SetOrderBy(filter.GetValue());
            case "page_size":
                return SetPageSize(ushort.Parse(filter.GetValue(),
                                                NumberStyles.None,
                                                CultureInfo.InvariantCulture));
            case "page":
                return SetStartPage(uint.Parse(filter.GetValue(),
                                               NumberStyles.None,
                                               CultureInfo.InvariantCulture));
            default:
                _filters.Add(filter);
                return this;
        }
    }
    public QueryBuilder Add(ReadOnlySpan<char> keyAndValue)
    {
        return Add(Filter.Parse(keyAndValue, CultureInfo.InvariantCulture));
    }
    public QueryBuilder Add(IDictionary? dict)
    {
        return dict is null
            ? this
            : Add(Filter.Parse(dict));
    }
    public QueryBuilder Add(string key,
                            string? value,
                            FilterLookupType lookupType = FilterLookupType.Exact,
                            bool isOr = false,
                            bool isNot = false)
    {
        return value is null
            ? this
            : Add(new Filter(key, value, lookupType, isOr, isNot));
    }
    public QueryBuilder Add(string key,
                            DateTime? value,
                            FilterLookupType lookupType,
                            bool isOr = false,
                            bool isNot = false)
    {
        return value is null
            ? this
            : Add(new Filter(key, value, lookupType, isOr, isNot));
    }
    public QueryBuilder Add(string key, IList? value, bool isOr = false, bool isNot = false)
    {
        return value is null
            ? this
            : Add(new Filter(key, value, isOr, isNot));
    }
    public QueryBuilder Add(string key, object? value)
    {
        return value is null
            ? this
            : Add(new Filter(key, value));
    }
    public QueryBuilder Add(string key,
                            object? value,
                            FilterLookupType lookupType,
                            bool isOr = false,
                            bool isNot = false)
    {
        return value is null
            ? this
            : Add(new Filter(key, value, lookupType, isOr, isNot));
    }
    public QueryBuilder Add(NameValueCollection? collection)
    {
        if (collection is null)
            return this;

        for (var i = 0; i < collection.Count; i++)
        {
            var key = collection.GetKey(i);
            if (key is null) continue;
            var values = collection.GetValues(i);
            if (values is null) continue;
            foreach (var value in values)
            {
                Add(key, value);
            }
        }
        return this;
    }

    /// <summary>
    /// Set <c>search</c> parameter
    /// <list type="bullet">
    ///     <item><paramref name="words"/> will be stored split by ','.</item>
    ///     <item>Only empty or non-blank word is valid.</item>
    /// </list>
    /// </summary>
    /// <param name="words"></param>
    /// <returns>this instance</returns>
    public QueryBuilder SetSearchWords(string? words)
    {
        SearchWords = string.IsNullOrEmpty(words)
            ? []
            : words.Split(',', StringSplitOptions.RemoveEmptyEntries
                               | StringSplitOptions.TrimEntries);
        return this;
    }
    /// <summary>
    /// Set <c>search</c> parameter
    /// <list type="bullet">
    ///     <item>Empty or space-only words will be excluded</item>
    /// </list>
    /// </summary>
    /// <param name="words"></param>
    /// <returns>this instance</returns>
    public QueryBuilder SetSearchWords(params string[]? words)
    {
        SearchWords = words is null
            ? []
            : [.. words.Where(static word => !string.IsNullOrWhiteSpace(word))];
        return this;
    }

    /// <summary>
    /// Set sort keys.
    /// Add the <c>-</c> prefix to sort in descending order.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>this instance</returns>
    public QueryBuilder SetOrderBy(params string[] keys)
    {
        OrderBy = [.. keys.Where(static key => !string.IsNullOrEmpty(key))];
        return this;
    }
    /// <summary>
    /// Set sort keys.
    /// Add the <c>-</c> prefix to sort in descending order.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>this instance</returns>
    public QueryBuilder SetOrderBy(string keys)
    {
        OrderBy = keys.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return this;
    }

    /// <summary>
    /// Set amount of page size per 1 query.
    /// (Between 1 and 200; Default = 20)
    /// </summary>
    /// <param name="pageSize">Amount of page size (Between 1 and 200; Default = 20)</param>
    /// <returns>this instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// thrown when the value less than 1 is specified to <paramref name="page"/> parameter
    /// </exception>.
    public QueryBuilder SetPageSize(ushort pageSize)
    {
        if (pageSize is < 1 or > MAX_PAGE_SIZE)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, $"Should be between 1 and {MAX_PAGE_SIZE}");
        _pageSize = pageSize;
        return this;
    }
    public QueryBuilder SetPageSize(int pageSize)
    {
        if (pageSize is < 1 or > MAX_PAGE_SIZE)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, $"Should be between 1 and {MAX_PAGE_SIZE}");
        _pageSize = (ushort)pageSize;
        return this;
    }

    /// <summary>
    /// Set page number of first time to retrieve. (Default = 1)
    /// </summary>
    /// <param name="startPage">Page number of first time to retrieve</param>
    /// <returns>this instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// thrown when the value less than 1 is specified to <paramref name="startPage"/> parameter
    /// </exception>.
    public QueryBuilder SetStartPage(uint startPage)
    {
        if (startPage < 1)
            throw new ArgumentOutOfRangeException(nameof(startPage), startPage, "Should be greater or equal 1");
        _startPage = startPage;
        return this;
    }
    public QueryBuilder SetStartPage(int startPage)
    {
        if (startPage < 1)
            throw new ArgumentOutOfRangeException(nameof(startPage), startPage, "Should be greater or equal 1");
        _startPage = (uint)startPage;
        return this;
    }

    /// <summary>
    /// Set query number of times.
    /// </summary>
    /// <remarks>
    /// AWX/AnsibleTower's Rest API has a limit to the number of data that can be retrieved in one query,
    /// so it is necessary to retrieve each page separately.
    /// To retrieve all data, specify <c>0</c>; to retrieve multiple times, specify the number of times.
    /// </remarks>
    /// <param name="queryCount">Number of times. (<c>0</c> means to ALL)</param>
    /// <returns>this instance</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// thrown when a negative value is specified to <paramref name="queryCount"/> parameter
    /// </exception>.
    public QueryBuilder SetQueryCount(uint queryCount)
    {
        _queryCount = queryCount;
        return this;
    }
    public QueryBuilder SetQueryCount(QueryCount queryCount)
    {
        _queryCount = (uint)queryCount;
        return this;
    }

    public HttpQuery Build()
    {
        var query = new HttpQuery(_queryCount);
        if (SearchWords.Length > 0)
            query.Set("search", string.Join(',', SearchWords));
        foreach (var filter in _filters)
        {
            query.Add(filter.GetKey(), filter.GetValue());
        }
        if (OrderBy.Length > 0)
            query.Set("order_by", string.Join(',', OrderBy));
        if (_pageSize > 0)
            query.Set("page_size", $"{_pageSize}");
        if (_startPage > 0)
            query.Set("page", $"{_startPage}");
        return query;
    }

    public IEnumerable<HttpQuery> BuildWithIdList<T>(T[] values)
    {
        int remaining = values.Length;
        int start = 0;
        do
        {
            var query = Build();

            int length = remaining >= MAX_PAGE_SIZE ? MAX_PAGE_SIZE : remaining;
            remaining -= MAX_PAGE_SIZE;
            query.Set("id__in", string.Join(',', values[new Range(start, start + length)]));
            query.Set("page_size", $"{length}");

            yield return query;
            start += length;
        }
        while (remaining > 0);
    }

    public static QueryBuilder Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return new QueryBuilder(s);
    }

    public static bool TryParse(ReadOnlySpan<char> s,
                                IFormatProvider? provider,
                                [MaybeNullWhen(false)] out QueryBuilder result)
    {
        try
        {
            result = new QueryBuilder(s);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static QueryBuilder Parse(string s, IFormatProvider? provider = null)
    {
        return new QueryBuilder(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s,
                                [MaybeNullWhen(false)] out QueryBuilder result)
    {
        return TryParse(s.AsSpan(), CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParse([NotNullWhen(true)] string? s,
                                IFormatProvider? provider,
                                [MaybeNullWhen(false)] out QueryBuilder result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public void Clear()
    {
        SearchWords = [];
        _filters.Clear();
        OrderBy = [];
        _pageSize = DEFAULT_PAGE_SIZE;
        _startPage = DEFAULT_PAGE_START;
    }

    public bool Contains(Filter item)
    {
        return _filters.Contains(item);
    }

    public bool Remove(Filter item)
    {
        return _filters.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _filters.RemoveAt(index);
    }
}
