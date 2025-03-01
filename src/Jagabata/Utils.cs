using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Jagabata
{
    public static class Utils
    {
        public static string ToUpperCamelCase(string value)
        {
            if (value.Length < 2)
            {
                return value.ToUpperInvariant();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToUpperInvariant(value[0]));
            for (var i = 1; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case ' ':
                    case '_':
                    case '-':
                        if (i < value.Length - 1)
                        {
                            sb.Append(char.ToUpperInvariant(value[++i]));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                    default:
                        sb.Append(char.ToLowerInvariant(c)); break;
                }
            }
            return sb.ToString();

        }

        public static string ToSnakeCase(string value)
        {
            if (value.Length < 2)
            {
                return value.ToLowerInvariant();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(value[0]));
            for (var i = 1; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get <c>PATH</c> field value in the Resource class
        /// </summary>
        /// <typeparam name="TType">Type has 'PATH' static field</typeparam>
        /// <param name="apiPath">API Path; Example: <c>/api/v2/jobs/</c></param>
        public static bool TryGetApiPath<TType>([MaybeNullWhen(false)] out string apiPath)
        {
            var t = typeof(TType);
            apiPath = t.GetField("PATH", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                      ?.GetValue(t) as string;
            return apiPath is not null;
        }

        public static bool TryGetTypeFromPath(string path, Method method, out Type type)
        {
            var t = GetTypeFromPath(path, method);
            if (t is not null)
            {
                type = t;
                return true;
            }
            type = typeof(string);
            return false;
        }
        public static Type? GetTypeFromPath(string path, Method method = Method.GET)
        {
            if (!path.StartsWith('/'))
                throw new ArgumentException($"{nameof(path)} must starts with '/'");

            var paths = path.Split('/')[1..^1];
            switch (paths.Length)
            {
                case 0:
                    return null;
                case 1:
                    return (paths[0] == "api") ? typeof(Dictionary<string, object?>) : null;
                case 2:
                    if (paths[0] == "api" && (paths[1] == "v2" || paths[2] == "o"))
                    {
                        return typeof(Dictionary<string, string>);
                    }
                    return null;
            }
            if (paths[0] != "api" || (paths[1] != "v2" && paths[1] != "o"))
            {
                return null;
            }

            FieldInfo? resourceField = null;
            ResourcePathAttribute? primaryAttr = null;
            var resourceType = typeof(ResourceType);
            foreach (var fieldInfo in resourceType.GetFields())
            {
                foreach (var attr in fieldInfo.GetCustomAttributes<ResourcePathAttribute>(false))
                {
                    if (attr.PathName == paths[2] && attr.Method == method)
                    {
                        resourceField = fieldInfo;
                        primaryAttr = attr;
                        break;
                    }
                }
                if (primaryAttr is not null) break;
            }

            if (primaryAttr is null || resourceField is null)
                return null;
            if (paths.Length == 3)
            {
                return primaryAttr.Type;
            }
            var isId = false;
            if (paths.Length > 3)
            {
                isId = ulong.TryParse(paths[3], out _);
            }
            string subPath;
            if (paths.Length == 4 && isId)
            {
                return resourceField.GetCustomAttributes<ResourceIdPathAttribute>(false)
                                    .Where(attr => attr.Method == method)
                                    .Select(attr => attr.Type)
                                    .FirstOrDefault();
            }
            else
            {
                subPath = isId ? string.Join('/', paths[4..]) : string.Join('/', paths[3..]);
            }
            return resourceField.GetCustomAttributes<ResourceSubPathAttribute>(false)
                                .Where(attr => attr.Method == method && attr.PathName == subPath && attr.IsSubPathOfId == isId)
                                .Select(attr => attr.Type)
                                .FirstOrDefault();
        }

        private static readonly char[] Whitespaces =
        [
            '\u0008',
            '\u0009',
            '\u000A',
            '\u000C',
            '\u000D',
            '\u0020',
            '\u0085',
            '\u00A0',
            '\u1680',
            '\u2000',
            '\u2001',
            '\u2002',
            '\u2003',
            '\u2004',
            '\u2005',
            '\u2006',
            '\u2007',
            '\u2008',
            '\u2009',
            '\u200A',
            '\u2028',
            '\u2029',
            '\u202F',
            '\u205F',
            '\u3000',
        ];
        public static string QuoteIfNeed(ReadOnlySpan<char> value, char quote = '\'', bool force = false)
        {
            bool needQuoted = force || value.ContainsAny(Whitespaces);
            StringBuilder sb = new(value.Length + (needQuoted ? 2 : 0));
            if (needQuoted)
                sb.Append(quote);
            int start = 0;
            while (true)
            {
                int length = value[start..].IndexOf(quote);
                if (length < 0)
                {
                    sb.Append(value[start..]);
                    break;
                }
                if (length > 0)
                {
                    sb.Append(value.Slice(start, length));
                }
                sb.Append(needQuoted ? quote : '`')
                  .Append(quote);
                start += length + 1;
            }
            if (needQuoted)
                sb.Append(quote);
            return sb.ToString();
        }
    }
}
