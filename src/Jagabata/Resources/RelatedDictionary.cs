using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// <list type="bullet">
    /// <item><term>Key</term><description>Resource type name (<c>string</c>)</description></item>
    /// <item><term>Valuel</term><description>Path of URL (<c>string</c> or <c>string[]</c>)</description></item>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(Json.RelatedResourceConverter))]
    public class RelatedDictionary : Dictionary<string, object>
    {
        public bool TryGetPath(string key, [MaybeNullWhen(false)] out string path)
        {
            return TryGetPath(key, 0, out path);
        }
        public bool TryGetPath(string key, int index, [MaybeNullWhen(false)] out string path)
        {
            path = default;
            if (TryGetValue(key, out var data))
            {
                if (data is string str)
                {
                    path = str;
                    return true;
                }
                if (data is string[] strArray && strArray.Length > index)
                {
                    path = strArray[index];
                    return true;
                }
            }
            return false;
        }
    }
}
