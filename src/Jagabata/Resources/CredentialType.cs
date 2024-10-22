using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<CredentialTypeKind>))]
    public enum CredentialTypeKind
    {
        /// <summary>
        /// Machine
        /// </summary>
        ssh,
        /// <summary>
        /// Vault
        /// </summary>
        vault,
        /// <summary>
        /// Network
        /// </summary>
        net,
        /// <summary>
        /// Source Control
        /// </summary>
        scm,
        /// <summary>
        /// Cloud
        /// </summary>
        cloud,
        /// <summary>
        /// Container Registry
        /// </summary>
        registry,
        /// <summary>
        /// Personal Access Token
        /// </summary>
        token,
        /// <summary>
        /// Insights
        /// </summary>
        insights,
        /// <summary>
        /// External
        /// </summary>
        external,
        /// <summary>
        /// Kubernetes
        /// </summary>
        kubernetes,
        /// <summary>
        /// Galaxy/Automation Hub
        /// </summary>
        galaxy,
        /// <summary>
        /// Cryptography
        /// </summary>
        cryptography
    }

    public interface ICredentialType
    {
        /// <summary>
        /// Name of this credential type.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this credential type.
        /// </summary>
        string Description { get; }
        CredentialTypeKind Kind { get; }
        CredentialTypeInputs Inputs { get; }
        Dictionary<string, Dictionary<string, string>> Injectors { get; }
    }

    public class CredentialType(ulong id,
                                ResourceType type,
                                string url,
                                RelatedDictionary related,
                                CredentialType.Summary summaryFields,
                                DateTime created,
                                DateTime? modified,
                                string name,
                                string description,
                                CredentialTypeKind kind,
                                string nameSpace,
                                bool managed,
                                CredentialTypeInputs inputs,
                                Dictionary<string, Dictionary<string, string>> injectors)
        : ICredentialType, IResource<CredentialType.Summary>
    {
        public const string PATH = "/api/v2/credential_types/";

        /// <summary>
        /// Retrieve a Credential Type.<br/>
        /// API Path: <c>/api/v2/credential_types/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<CredentialType> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<CredentialType>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Credential Types.<br/>
        /// API Path: <c>/api/v2/credential_types/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<CredentialType> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<CredentialType>(PATH, query, getAll))
            {
                foreach (var credentialType in result.Contents.Results)
                {
                    yield return credentialType;
                }
            }
        }
        public record Summary(Capability UserCapabilities);


        public ulong Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Url { get; } = url;
        public RelatedDictionary Related { get; } = related;
        public Summary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public CredentialTypeKind Kind { get; } = kind;
        public string Namespace { get; } = nameSpace;
        public bool Managed { get; } = managed;
        public CredentialTypeInputs Inputs { get; } = inputs;
        public Dictionary<string, Dictionary<string, string>> Injectors { get; } = injectors;
    }

    public record CredentialTypeInputs(CredentialInputField[] Fields, string[]? Required)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            sb.Append("Fields = [");
            if (Fields.Length > 0) sb.Append($" {string.Join(", ", Fields.Select(field => field.Id))} ");
            sb.Append(']');
            if (Required is not null)
            {
                sb.Append(", Required = [");
                if (Required.Length > 0) sb.Append($" {string.Join(", ", Required)} ");
                sb.Append(']');
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }

    [JsonConverter(typeof(CredentialInputFieldConverter))]
    public abstract record CredentialInputField(string Id, string Label, string Type, string? HelpText);

    public record CredentialBoolInputField(string Id,
                                           string Label,
                                           string Type,
                                           string? HelpText,
                                           bool? Default)
        : CredentialInputField(Id, Label, Type, HelpText)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append($" Id = {Id}");
            sb.Append($", Label = {Label}");
            sb.Append($", Type = {Type}");
            if (Default is not null) sb.Append($", Default = {Default}");
            if (HelpText is not null) sb.Append($", HelpText = {HelpText}");
            sb.Append(" }");
            return sb.ToString();
        }
    }
    public record CredentialStringInputField(string Id,
                                             string Label,
                                             string Type,
                                             string? HelpText,
                                             string[]? Choices,
                                             string? Format,
                                             bool Secret = false,
                                             bool Multiline = false,
                                             string? Default = null)
        : CredentialInputField(Id, Label, Type, HelpText)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append($" Id = {Id}");
            sb.Append($", Label = {Label}");
            sb.Append($", Type = {Type}");
            if (Default is not null) sb.Append($", Default = {Default}");
            if (Choices is not null) sb.Append($", Choices = [{string.Join(", ", Choices)}]");
            if (Format is not null) sb.Append($", Format = {Format}");
            if (Secret) sb.Append(", Secret = True");
            if (Multiline) sb.Append(", Multiline = True");
            if (HelpText is not null) sb.Append($", HelpText = {HelpText}");
            sb.Append(" }");
            return sb.ToString();
        }
    }

    class CredentialInputFieldConverter : JsonConverter<CredentialInputField>
    {
        public override CredentialInputField? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            (string id, string label, string? help_text, string[]? choices, string? format, string type, bool secret, bool multiline) =
                ("", "", null, null, null, "string", false, false);
            string? defaultString = null;
            bool? defaultBool = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    switch (type)
                    {
                        case "boolean":
                            return new CredentialBoolInputField(id, label, type, help_text, defaultBool);
                        case "string":
                        default:
                            return new CredentialStringInputField(
                                    id, label, type, help_text, choices, format, secret, multiline, defaultString);
                    }
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
                }
                string propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                reader.Read();
                switch (propertyName)
                {
                    case "id":
                        id = reader.GetString() ?? "";
                        break;
                    case "label":
                        label = reader.GetString() ?? "";
                        break;
                    case "help_text":
                        help_text = reader.GetString();
                        break;
                    case "choices":
                        choices = JsonSerializer.Deserialize<string[]>(ref reader, options);
                        break;
                    case "format":
                        format = reader.GetString();
                        break;
                    case "secret":
                        secret = reader.GetBoolean();
                        break;
                    case "multiline":
                        multiline = reader.GetBoolean();
                        break;
                    case "type":
                        type = reader.GetString() ?? "string";
                        break;
                    case "default":
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.String:
                                defaultString = reader.GetString() ?? "";
                                break;
                            case JsonTokenType.True:
                            case JsonTokenType.False:
                                defaultBool = reader.GetBoolean();
                                break;
                        }
                        break;
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, CredentialInputField value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("label", value.Label);
            writer.WriteString("type", value.Type);
            if (value.HelpText is not null)
            {
                writer.WriteString("help_text", value.HelpText);
            }
            switch (value)
            {
                case CredentialBoolInputField boolField:
                    if (boolField.Default is not null)
                    {
                        writer.WriteBoolean("default", (bool)boolField.Default);
                    }
                    break;
                case CredentialStringInputField strField:
                    if (strField.Choices is not null)
                    {
                        writer.WritePropertyName("choices");
                        JsonSerializer.Serialize(writer, strField.Choices, options);
                    }
                    if (strField.Default is not null)
                    {
                        writer.WriteString("default", strField.Default);
                    }
                    if (strField.Format is not null)
                    {
                        writer.WriteString("format", strField.Format);
                    }
                    if (strField.Secret)
                    {
                        writer.WriteBoolean("secret", strField.Secret);
                    }
                    if (strField.Multiline)
                    {
                        writer.WriteBoolean("multiline", strField.Multiline);
                    }
                    break;
            }
            writer.WriteEndObject();
        }
    }
}

