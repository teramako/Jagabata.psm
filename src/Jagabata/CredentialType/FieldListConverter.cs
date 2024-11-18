using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

internal class FieldListConverter : JsonConverter<FieldList>
{
    private static FieldBase? ReadField(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        (string Id, string Label, string? HelpText, string[]? Choices, FieldFormat? Format, FieldType Type,
         bool? Secret, bool? Multiline, string? DefaultString, bool? DefaultBool, bool? AskAtRuntime) field =
            (string.Empty, string.Empty, null, null, null, FieldType.String, null, null, null, null, null);

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return field.Type switch
                    {
                        FieldType.Boolean => new BoolField(field.Id, field.Label)
                        {
                            Default = field.DefaultBool,
                            HelpText = field.HelpText,
                            AskAtRuntime = field.AskAtRuntime
                        },
                        FieldType.String => field.Choices is null
                            ? new StringField(field.Id, field.Label)
                            {
                                Format = field.Format,
                                Secret = field.Secret,
                                Multiline = field.Multiline,
                                Default = field.DefaultString,
                                HelpText = field.HelpText,
                                AskAtRuntime = field.AskAtRuntime
                            }
                            : new ChoiceField(field.Id, field.Label)
                            {
                                Choices = field.Choices,
                                Default = field.DefaultString,
                                HelpText = field.HelpText,
                                AskAtRuntime = field.AskAtRuntime
                            },
                        _ => throw new JsonException($"Unknown field type: {field.Type}"),
                    };
                case JsonTokenType.PropertyName:
                    string fieldPropertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                    reader.Read();
                    switch (fieldPropertyName)
                    {
                        case "id":
                            field.Id = reader.GetString() ?? "";
                            break;
                        case "label":
                            field.Label = reader.GetString() ?? "";
                            break;
                        case "help_text":
                            field.HelpText = reader.GetString();
                            break;
                        case "choices":
                            field.Choices = JsonSerializer.Deserialize<string[]>(ref reader, options);
                            break;
                        case "format":
                            field.Format = JsonSerializer.Deserialize<FieldFormat>(ref reader, options);
                            break;
                        case "secret":
                            field.Secret = reader.GetBoolean();
                            break;
                        case "multiline":
                            field.Multiline = reader.GetBoolean();
                            break;
                        case "type":
                            field.Type = JsonSerializer.Deserialize<FieldType>(ref reader, options);
                            break;
                        case "ask_at_runtime":
                            field.AskAtRuntime = reader.GetBoolean();
                            break;
                        case "default":
                            switch (reader.TokenType)
                            {
                                case JsonTokenType.String:
                                    field.DefaultString = reader.GetString() ?? "";
                                    break;
                                case JsonTokenType.True:
                                case JsonTokenType.False:
                                    field.DefaultBool = reader.GetBoolean();
                                    break;
                            }
                            break;
                    }
                    continue;
            }
        }
        throw new JsonException();
    }

    public override FieldList? Read(ref Utf8JsonReader reader,
                                          Type typeToConvert,
                                          JsonSerializerOptions options)
    {
        var container = new FieldList();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return container;
            }
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
            }
            string propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
            reader.Read();
            switch (propertyName)
            {
                case "fields":
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }

                        var field = ReadField(ref reader, options);
                        if (field is not null)
                        {
                            container.Add(field);
                        }
                    }
                    break;
                case "required":
                    var requiredFields = JsonSerializer.Deserialize<string[]>(ref reader, options);
                    if (requiredFields is not null)
                    {
                        foreach (var id in requiredFields)
                        {
                            var field = container.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.Ordinal));
                            if (field is not null)
                            {
                                field.Required = true;
                            }
                        }
                    }
                    break;
                case "dependencies":
                    container.Dependencies = JsonSerializer.Deserialize<Dictionary<string, string[]>>(ref reader, options);
                    break;
                case "metadata":
                    container.Metadata = JsonSerializer.Deserialize<Dictionary<string, object?>[]>(ref reader, options);
                    break;
            }
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer,
                               FieldList value,
                               JsonSerializerOptions options)
    {
        var requiredFields = new List<string>();

        writer.WriteStartObject();

        writer.WriteStartArray("fields");
        foreach (var field in value)
        {
            if (field.Required is not null && (bool)field.Required)
            {
                requiredFields.Add(field.Id);
            }
            JsonSerializer.Serialize(writer, field, field.GetType(), options);
        }
        writer.WriteEndArray();

        if (value.Metadata is not null)
        {
            writer.WriteStartArray("metadata");
            JsonSerializer.Serialize(writer, value.Metadata, options);
            writer.WriteEndArray();
        }

        if (value.Dependencies is not null)
        {
            writer.WriteStartArray("dependencies");
            JsonSerializer.Serialize(writer, value.Dependencies, options);
            writer.WriteEndArray();
        }

        writer.WriteStartArray("required");
        foreach (var id in requiredFields)
        {
            writer.WriteStringValue(id);
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
