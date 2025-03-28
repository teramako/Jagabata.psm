using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation;
using System.Text;

namespace Jagabata
{
    /// <summary>
    /// Lookup Type for <see cref="Filter"/>
    /// <br/>
    /// See: <a href="https://docs.ansible.com/automation-controller/latest/html/controllerapi/filtering.html">
    /// 6. Filtering — Automation Controller API Guide</a>
    /// </summary>
    public enum FilterLookupType
    {
        /// <Summary>
        /// Exact match (default lookup if not specified
        /// </Summary>
        Exact = 0,
        /// <Summary>
        /// Case-insensitive version of Exact
        /// </Summary>
        IExact = 1,
        /// <Summary>
        /// Field contains value
        /// </Summary>
        Contains = 2,
        /// <Summary>
        /// Case-insensitive version of Contains
        /// </Summary>
        IContains = 3,
        /// <Summary>
        /// Field starts with value
        /// </Summary>
        StartsWith = 4,
        /// <Summary>
        /// Case-insensitive version of StartsWith
        /// </Summary>
        IStartsWith = 5,
        /// <Summary>
        /// Field ends with value
        /// </Summary>
        EndsWith = 6,
        /// <Summary>
        /// Case-insensitive version of EndsWith
        /// </Summary>
        IEndsWith = 7,
        /// <Summary>
        /// Field matches the given Regular Expression
        /// </Summary>
        Regex = 8,
        /// <Summary>
        /// Case-insensitive version of  Regex
        /// </Summary>
        IRegex = 9,
        /// <Summary>
        /// Greater than comparsion.
        /// </Summary>
        GreaterThan = 10,
        /// <Summary>
        /// Greater than comparsion. (Alias to <c>GreaterThan</c>)
        /// </Summary>
        GT = GreaterThan,
        /// <Summary>
        /// Greater than or equal to comparsion.
        /// </Summary>
        GreaterThanOrEqual = 11,
        /// <Summary>
        /// Greater than or equal to comparsion. (Alias to <c>GreaterThanOrEqual</c>)
        /// </Summary>
        GTE = GreaterThanOrEqual,
        /// <Summary>
        /// Less than comparsion.
        /// </Summary>
        LessThan = 12,
        /// <Summary>
        /// Less than comparsion. (Alias to <c>LessThan</c>)
        /// </Summary>
        LT = LessThan,
        /// <Summary>
        /// Less than or equal to comparsion.
        /// </Summary>
        LessThanOrEqual = 13,
        /// <Summary>
        /// Less than or equal to comparsion. (Alias to <c>LessThanOrEqual</c>)
        /// </Summary>
        LTE = LessThanOrEqual,
        /// <Summary>
        /// Check whether the given field or related object is null: expects a boolean value
        /// </Summary>
        IsNull = 14,
        /// <Summary>
        /// Check whether the given fields's value is present in the list provided: expects a list of items
        /// </Summary>
        In = 15,
    }

    /// <summary>
    /// Filter item.
    /// <br/>
    /// See: <a href="https://docs.ansible.com/automation-controller/latest/html/controllerapi/filtering.html">
    /// 6. Filtering — Automation Controller API Guide</a>
    /// </summary>
    public class Filter : ISpanParsable<Filter>
    {
        public Filter()
        { }
        /// <summary>
        /// Simple constructor.
        /// <list type="table">
        /// <item><term><paramref name="key"/></term><description>See: <see cref="SetKey(ReadOnlySpan{char})"/></description></item>
        /// <item><term><paramref name="value"/></term><description>See: <see cref="SetValue(object?)"/></description></item>
        /// </list>
        /// </summary>
        public Filter(string key, object? value)
        {
            SetKey(key);
            SetValue(value);
        }
        /// <summary>
        /// For string type of <paramref name="value"/>
        /// </summary>
        public Filter(string key, string? value)
        {
            SetKey(key);
            SetValue(value);
        }
        /// <summary>
        /// For string type of <paramref name="value"/>
        /// </summary>
        public Filter(string name, string? value, FilterLookupType type, bool or = false, bool not = false)
        {
            SetKey(name);
            SetValue(value);
            Or = or;
            Not = not;
            Type = type;
        }
        /// <summary>
        /// For <see cref="DateTime"/> type of <paramref name="value"/>
        /// </summary>
        public Filter(string name, DateTime value, FilterLookupType type, bool or = false, bool not = false)
        {
            SetKey(name);
            SetValue(value);
            Type = type;
            Or = or;
            Not = not;
        }
        /// <summary>
        /// For <see cref="IList"/> type of <paramref name="value"/>.
        /// <see cref="Type"/> will be set to <see cref="FilterLookupType.In"/> automatically.
        /// </summary>
        public Filter(string name, IList value, bool or = false, bool not = false)
        {
            SetKey(name);
            SetValue(value);
            Type = FilterLookupType.In;
            Or = or;
            Not = not;
        }
        /// <summary>
        /// For unknown type of <paramref name="value"/>.
        /// </summary>
        public Filter(string name, object? value, FilterLookupType type, bool or = false, bool not = false)
        {
            SetKey(name);
            SetValue(value);
            Type = type;
            Or = or;
            Not = not;
        }
        public Filter(ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        {
            SetKey(key);
            SetValue(value);
        }
        public Filter(ReadOnlySpan<char> name, ReadOnlySpan<char> value, FilterLookupType type, bool or = false, bool not = false)
        {
            SetKey(name);
            SetValue(value);
            Type = type;
            Or = or;
            Not = not;
        }

        /// <summary>
        /// Alias to <see cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
        /// </summary>
        public static Filter Parse(ReadOnlySpan<char> s)
        {
            return Parse(s, CultureInfo.InvariantCulture);
        }
        public static Filter Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (s.IsEmpty || s.IsWhiteSpace())
                return new Filter();

            Span<Range> kvRanges = new Range[2];
            var kvRangeLength = s.Split(kvRanges, '=', StringSplitOptions.TrimEntries);
            return new Filter(s[kvRanges[0]], kvRangeLength == 2 ? s[kvRanges[1]] : []);
        }

        public static bool TryParse(ReadOnlySpan<char> s,
                                    IFormatProvider? provider,
                                    [MaybeNullWhen(false)] out Filter result)
        {
            try
            {
                result = Parse(s, provider);
            }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Alias to <see cref="Parse(string, IFormatProvider?)"/>
        /// </summary>
        public static Filter Parse(string s)
        {
            return Parse(s.AsSpan(), CultureInfo.InvariantCulture);
        }

        public static Filter Parse(string s, IFormatProvider? provider)
        {
            return Parse(s.AsSpan(), provider);
        }

        public static bool TryParse([NotNullWhen(true)] string? s,
                                    IFormatProvider? provider,
                                    [MaybeNullWhen(false)] out Filter result)
        {
            return TryParse(s.AsSpan(), provider, out result);
        }

        /// <summary>
        /// Parse <see cref="IDictionary"/> to Filter.
        /// This may be useful on PowerShell.
        /// </summary>
        /// <exception cref="ArgumentException">thrown when failed to parse value</exception>
        public static Filter Parse(IDictionary dict)
        {
            var query = new Filter();
            foreach (var keyObj in dict.Keys)
            {
                var key = $"{keyObj}";
                switch (key.ToLowerInvariant())
                {
                    case "name":
                        query.Name = $"{dict[keyObj]}";
                        continue;
                    case "value":
                        query.SetValue(dict[keyObj]);
                        continue;
                    case "type":
                        query.Type = Enum.Parse<FilterLookupType>($"{dict[keyObj]}", true);
                        continue;
                    case "or":
                        query.Or = bool.Parse($"{dict[keyObj]}");
                        continue;
                    case "not":
                        query.Not = bool.Parse($"{dict[keyObj]}");
                        continue;
                }
            }
            return query;
        }
        public static string LookupTypeToString(FilterLookupType type)
        {
            return type switch
            {
                FilterLookupType.Exact => "",
                FilterLookupType.IExact => "iexact",
                FilterLookupType.Contains => "contains",
                FilterLookupType.IContains => "icontains",
                FilterLookupType.StartsWith => "startswith",
                FilterLookupType.IStartsWith => "istartswith",
                FilterLookupType.EndsWith => "endswith",
                FilterLookupType.IEndsWith => "iendswith",
                FilterLookupType.Regex => "regex",
                FilterLookupType.IRegex => "iregex",
                FilterLookupType.GreaterThan => "gt",
                FilterLookupType.GreaterThanOrEqual => "gte",
                FilterLookupType.LessThan => "lt",
                FilterLookupType.LessThanOrEqual => "lte",
                FilterLookupType.IsNull => "isnull",
                FilterLookupType.In => "in",
                _ => "",
            };
        }

        public string Name
        {
            get => _name;
            set => SetKey(value);
        }
        private string _name = string.Empty;
        public object? Value
        {
            get => _value;
            set => SetValue(value);
        }
        private string _value = string.Empty;
        public FilterLookupType Type { get; set; } = FilterLookupType.Exact;
        public bool Or { get; set; }
        public bool Not { get; set; }

        public string GetKey()
        {
            var sb = new StringBuilder();
            if (Or) sb.Append("or__");
            if (Not) sb.Append("not__");
            sb.Append(Name);
            var lookup = LookupTypeToString(Type);
            if (!string.IsNullOrEmpty(lookup))
                sb.Append("__").Append(lookup);
            return sb.ToString();
        }
        /// <summary>
        /// Parse <paramref name="key"/> and split to <see cref="Or"/>, <see cref="Not"/>, <see cref="Name"/> and <see cref="Type"/>.
        /// </summary>
        /// <param name="key">
        /// format: <c>(or__)?(not__)?(?&lt;name&gt;\w+)(?:__(?&lt;lookupType&gt;\w+))?</c>
        /// </param>
        /// <returns>this instance</returns>
        public Filter SetKey(ReadOnlySpan<char> key)
        {
            Span<char> lowerKey = new char[key.Length];
            key.ToLowerInvariant(lowerKey);
            ReadOnlySpan<char> separator = "__";
            Span<Range> ranges = new Range[lowerKey.Count(separator) + 1];
            ((ReadOnlySpan<char>)lowerKey).Split(ranges, separator, StringSplitOptions.None);
            var rangeIndex = 0;
            if (Or = lowerKey[ranges[rangeIndex]].SequenceEqual("or"))
            {
                rangeIndex++;
            }
            if (Not = lowerKey[ranges[rangeIndex]].SequenceEqual("not"))
            {
                rangeIndex++;
            }
            ranges = ranges[rangeIndex..];
            if (ranges.Length == 0)
                throw new ArgumentException($"Invalid Filter key: {key}");
            if (ranges.Length > 1)
            {
                if (Enum.TryParse<FilterLookupType>(lowerKey[ranges[^1]], true, out var type))
                {
                    Type = type;
                    ranges = ranges[..^1];
                }
            }
            _name = lowerKey[ranges[0].Start..ranges[^1].End].ToString();
            return this;
        }

        /// <summary>
        /// Set <see cref="Type"/> to specified <see cref="FilterLookupType"/>.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetType(FilterLookupType type)
        {
            Type = type;
            return this;
        }

        public string GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Set value to <paramref name="str"/>
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetValue(string? str)
        {
            _value = str ?? string.Empty;
            return this;
        }
        /// <summary>
        /// Set value to <paramref name="str"/>
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetValue(ReadOnlySpan<char> str)
        {
            _value = str.ToString();
            return this;
        }
        /// <summary>
        /// Set value to <c>True</c> or <c>False</c> of string.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetValue(bool boolean)
        {
            _value = boolean ? "True" : "False";
            return this;
        }
        /// <summary>
        /// Set value to ISO datetime string.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetValue(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = dateTime.ToUniversalTime().ToLocalTime();
            _value = dateTime.ToString("o");
            return this;
        }
        /// <summary>
        /// Set value to <c>,</c> separated string.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetValue(IList list)
        {
            var strList = new string[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                strList[i] = $"{list[i]}";
            }
            _value = string.Join(',', strList);
            return this;
        }
        public Filter SetValue(object? value)
        {
            if (value is PSObject psobj)
                value = psobj.BaseObject;

            switch (value)
            {
                case null:
                    return SetValue(string.Empty);
                case string str:
                    return SetValue(str);
                case bool boolean:
                    return SetValue(boolean);
                case DateTime datetime:
                    return SetValue(datetime);
                case IList list:
                    return SetValue(list);
                default:
                    _value = $"{value}";
                    break;
            }
            return this;
        }

        /// <summary>
        /// Set <see cref="Or"/> flag to <c>false</c>.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetAnd()
        {
            Or = false;
            return this;
        }

        /// <summary>
        /// Set <see cref="Or"/> flag to <c>true</c>.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetOr()
        {
            Or = true;
            return this;
        }

        /// <summary>
        /// Set <see cref="Not"/> flag to <paramref name="not"/>.
        /// </summary>
        /// <returns>this instance</returns>
        public Filter SetNot(bool not = true)
        {
            Not = not;
            return this;
        }

        public override string ToString()
        {
            return $"{GetKey()}={GetValue()}";
        }
    }
}
