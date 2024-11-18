using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

[JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<FieldFormat>))]
public enum FieldFormat
{
    SshPrivateKey,
    VaultId,
    Url
}
