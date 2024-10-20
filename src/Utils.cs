using System.Text;

namespace Jagabata
{
    public class Utils
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
    }
}
