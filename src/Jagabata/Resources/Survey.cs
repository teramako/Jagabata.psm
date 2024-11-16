using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// For following RestAPI:
    ///
    /// <list type="bullet">
    ///   <item><c>/api/v2/job_templates/{id}/survey_spec/</c></item>
    ///   <item><c>/api/v2/workflow_job_templates/{id}/survey_spec/</c></item>
    /// </list>
    /// </summary>
    public class Survey
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SurveySpec[] Spec { get; set; } = [];
    }

    public enum SurveySpecType
    {
        Text, Textarea, Password, Integer, Float, MultipleChoice, MultiSelect
    }

    [JsonConverter(typeof(SurveySpecConverter))]
    public class SurveySpec
    {
        public SurveySpec()
        { }
        public SurveySpec(SurveySpecType type)
        {
            Type = type;
        }
        public SurveySpec(SurveySpecType type, string variableName) : this(type)
        {
            Name = variableName;
            Variable = variableName;
        }
        public SurveySpec(SurveySpecType type, string name, string variableName) : this(type)
        {
            Name = name;
            Variable = variableName;
        }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SurveySpecType Type { get; internal set; }
        public bool Required { get; set; }
        public string Variable { get; set; } = string.Empty;
        public virtual object? Default { get; set; }
        public virtual object Choices { get; set; } = string.Empty;
        public int Min { get; set; }
        public int Max { get; set; } = 1024;
        public bool NewQuestion { get; set; }

        public override string ToString()
        {
            return $"{{ Name = {Name}, Type = {Type}, Variable = {Variable}, Default = {Default} }}";
        }
    }

    internal class SurveySpecConverter : JsonConverter<SurveySpec>
    {
        public override SurveySpec? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var spec = new SurveySpec();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return spec;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
                }
                string propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                reader.Read();
                switch (propertyName)
                {
                    case "max":
                        if (reader.TryGetInt32(out int max))
                        {
                            spec.Max = max;
                        }
                        break;
                    case "min":
                        if (reader.TryGetInt32(out int min))
                        {
                            spec.Min = min;
                        }
                        break;
                    case "type":
                        var typeName = reader.GetString();
                        if (Enum.TryParse<SurveySpecType>(typeName, true, out var type))
                        {
                            spec.Type = type;
                        }
                        break;
                    case "choices":
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.StartArray:
                                spec.Choices = JsonSerializer.Deserialize<string[]>(ref reader, options)
                                    ?? throw new JsonException();
                                break;
                            case JsonTokenType.String:
                            default:
                                spec.Choices = reader.GetString() ?? "";
                                break;
                        }
                        break;
                    case "default":
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.Number:
                                if (reader.TryGetInt32(out var defaultInt))
                                {
                                    spec.Default = defaultInt;
                                }
                                else if (reader.TryGetSingle(out var defaultFloat))
                                {
                                    spec.Default = defaultFloat;
                                }
                                break;
                            case JsonTokenType.String:
                            default:
                                spec.Default = reader.GetString() ?? "";
                                break;
                        }
                        break;
                    case "required":
                        spec.Required = reader.GetBoolean();
                        break;
                    case "variable":
                        spec.Variable = reader.GetString() ?? "";
                        break;
                    case "new_question":
                        spec.NewQuestion = reader.GetBoolean();
                        break;
                    case "question_name":
                        spec.Name = reader.GetString() ?? "";
                        break;
                    case "question_description":
                        spec.Description = reader.GetString() ?? "";
                        break;
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, SurveySpec value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("question_name", value.Name);
            writer.WriteString("question_description", value.Description);
            writer.WriteString("type", value.Type.ToString().ToLowerInvariant());
            writer.WriteBoolean("required", value.Required);
            writer.WriteString("variable", value.Variable);
            switch (value.Default)
            {
                case int intVal:
                    writer.WriteNumber("default", intVal);
                    break;
                case float floatVal:
                    writer.WriteNumber("default", floatVal);
                    break;
                default:
                    writer.WriteString("default", value.Default?.ToString() ?? "");
                    break;
            }
            if (value.Choices is IList list)
            {
                writer.WritePropertyName("choices");
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    writer.WriteStringValue(item.ToString());
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteString("choices", value.Choices.ToString());
            }
            writer.WriteNumber("min", value.Min);
            writer.WriteNumber("max", value.Max);
            writer.WriteBoolean("new_question", value.NewQuestion);
            writer.WriteEndObject();
        }
    }

}
