using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// 基本的には 
    /// <list type="bullet">
    /// <item><term>Key</term><description>Resource　type name (<c>string</c>)</description></item>
    /// <item><term>Valuel</term><description>Path of URL (<c>string</c> or <c>string[]</c>)</description></item>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(Json.RelatedResourceConverter))]
    public class RelatedDictionary : Dictionary<string, object>
    {
        // TODO: 何かメソッドを追加
    }
}
