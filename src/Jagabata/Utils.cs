using System.Reflection;
using System.Text;
using Jagabata.Resources;

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
            else if (isId)
            {
                subPath = string.Join('/', paths[4..]);
            }
            else
            {
                subPath = string.Join('/', paths[3..]);
            }
            return resourceField.GetCustomAttributes<ResourceSubPathAttribute>(false)
                                .Where(attr => attr.Method == method && attr.PathName == subPath && attr.IsSubPathOfId == isId)
                                .Select(attr => attr.Type)
                                .FirstOrDefault();
        }
    }
}
