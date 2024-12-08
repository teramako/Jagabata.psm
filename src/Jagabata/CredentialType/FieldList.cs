using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

[JsonConverter(typeof(FieldListConverter))]
public class FieldList : List<FieldBase>
{
    public Dictionary<string, string[]>? Dependencies { get; internal set; }
    public Dictionary<string, object?>[]? Metadata { get; internal set; }
}
