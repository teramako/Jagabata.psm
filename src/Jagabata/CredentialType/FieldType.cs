using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

[JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<FieldType>))]
public enum FieldType
{
    String,
    Boolean
}

