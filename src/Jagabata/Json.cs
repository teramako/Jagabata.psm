using Jagabata.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Management.Automation;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Jagabata
{
    public static class Json
    {
        public static string Stringify(object obj, bool pretty = false)
        {
            var options = pretty ? SerializeOptions : DeserializeOptions;
            return JsonSerializer.Serialize(obj, options);
        }
        public static OrderedDictionary ToDict(JsonElement json)
        {
            var dict = new OrderedDictionary();
            foreach (var kv in json.EnumerateObject())
            {
                dict.Add(kv.Name, ObjectToInferredType(kv.Value));
            }
            return dict;
        }
        public static object?[] ToArray(JsonElement json)
        {
            var length = json.GetArrayLength();
            object?[] array = new object?[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = ObjectToInferredType(json[i]);
            }
            return array;
        }
        public static object? ObjectToInferredType(JsonElement val, bool isRoot = false)
        {
            switch (val.ValueKind)
            {
                case JsonValueKind.Object:
                    if (isRoot)
                    {
                        if (val.TryGetProperty("type", out var typeValue))
                        {
                            var key = Utils.ToUpperCamelCase(typeValue.GetString() ?? string.Empty);
                            var fieldInfo = typeof(ResourceType).GetField(key);
                            if (fieldInfo is not null)
                            {
                                var attr = fieldInfo.GetCustomAttributes<ResourcePathAttribute>(false).FirstOrDefault();
                                if (attr is not null && attr.Type is not null)
                                {
                                    var resourceType = attr.Type;
                                    if (resourceType.IsGenericType && resourceType.IsSubclassOf(typeof(ResultSetBase)))
                                    {
                                        resourceType = resourceType.GenericTypeArguments[0];
                                    }
                                    var obj = val.Deserialize(resourceType, DeserializeOptions);
                                    if (obj is not null)
                                    {
                                        return obj;
                                    }
                                }
                            }
                        }
                        var isResultSet = true;
                        foreach (var kv in val.EnumerateObject())
                        {
                            switch (kv.Name)
                            {
                                case "count":
                                case "next":
                                case "previous":
                                    continue;
                                case "results":
                                    if (kv.Value.ValueKind == JsonValueKind.Array)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        isResultSet = false;
                                    }
                                    break;
                                default:
                                    isResultSet = false;
                                    break;
                            }
                        }
                        if (isResultSet)
                        {
                            return val.Deserialize<ResultSet>(DeserializeOptions);
                        }
                    }
                    return ToDict(val);
                case JsonValueKind.Array:
                    return ToArray(val);
                case JsonValueKind.String:
                    return val.GetString() ?? string.Empty;
                case JsonValueKind.Number:
                    return val.GetRawText().IndexOf('.') > 0 ? val.GetDouble() : val.GetInt64();
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return val.GetBoolean();
                case JsonValueKind.Null:
                default:
                    return null;
                    // throw new ArgumentNullException(nameof(val));
            }
        }
        /// <summary>
        /// "related" property converter.<br/>
        /// See <see cref="RelatedDictionary"/>
        /// </summary>
        public class RelatedResourceConverter : JsonConverter<RelatedDictionary>
        {
            public override RelatedDictionary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dict = new RelatedDictionary();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return dict;
                    }
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
                    }
                    string? propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");

                    reader.Read();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.String:
                            var val = reader.GetString();
                            if (val is not null)
                                dict.Add(propertyName, val);
                            break;
                        case JsonTokenType.StartArray:
                            var list = new List<string>();
                            reader.Read();
                            while (reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.String)
                                {
                                    throw new JsonException($"[InArray] value is not String: {reader.TokenType}");
                                }
                                string? arrayVal = reader.GetString();
                                if (!string.IsNullOrEmpty(arrayVal))
                                    list.Add(arrayVal);

                                reader.Read();
                            }
                            dict.Add(propertyName, list.ToArray());
                            break;
                        default:
                            throw new JsonException($"TokenType is not String or StartArray: {reader.TokenType}");
                    }

                }
                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, RelatedDictionary value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (var kv in value)
                {
                    writer.WritePropertyName(kv.Key);
                    if (kv.Value is IList list)
                    {
                        writer.WriteStartArray();
                        foreach (var item in list)
                        {
                            writer.WriteStringValue($"{item}");
                        }
                        writer.WriteEndArray();
                    }
                    else
                    {
                        writer.WriteStringValue($"{kv.Value}");
                    }
                }
                writer.WriteEndObject();
            }
        }
        /// <summary>
        /// Enum Converter
        /// <list type="bullet">Serialize: to string of "snake_case"</list>
        /// <list type="bullet">Deserialize: to Enum named "UpperCamelCase"</list>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        public class EnumUpperCamelCaseStringConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
        {
            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var val = reader.GetString() ?? throw new JsonException($"The value of Type {typeToConvert.Name} must not be null");
                if (string.IsNullOrEmpty(val))
                {
                    return default;
                }
                string upperCamelCaseName = Utils.ToUpperCamelCase(val);
                return Enum.TryParse(upperCamelCaseName, true, out TEnum enumVal)
                    ? enumVal
                    : throw new JsonException($"'{upperCamelCaseName}' is not in Enum {typeToConvert.Name}");
            }

            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                string val = Utils.ToSnakeCase(value.ToString());
                writer.WriteStringValue(val);
            }
        }
        /// <summary>
        /// <see cref="DateTime"/> Converter.
        /// <list type="bullet">Serialize to string of UTC</list>
        /// <list type="bullet">Deserialize to Local DateTime</list>
        /// </summary>
        public class LocalDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.GetDateTime().ToLocalTime();
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
            }
        }
        /// <summary>
        /// Json Serialize / Deserialize OPTIONS for this API.
        /// <list type="bullet">
        ///   <item>Property PathName to snake_case for serialization</item>
        ///   <item>Property PathName In case sensitive for deserialization</item>
        ///   <item>DateTime to Local time zone for deserialization, to Utc for serialization</item>
        ///   <item>Non classed object to <c>Dictionary&lt;string, object?&gt;</c> for deserialization</item>
        ///   <item>Non classed array to <c>object?[]</c> for deserialization</item>
        /// </list>
        /// </summary>
        public static readonly JsonSerializerOptions DeserializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new LocalDateTimeConverter(),
                new DictConverter(),
                new SecureStringConverter(false),
                // new ArrayConverter()
            }
        };
        public static readonly JsonSerializerOptions SerializeOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            Converters =
            {
                new PSObjectConverter(),
                new LocalDateTimeConverter(),
                new SecureStringConverter(true),
            }
        };
        /// <summary>
        /// Converter to deserialize PSObject while preventing circular references
        /// </summary>
        /// <remarks>
        /// **Don't use for deserializing**
        /// </remarks>
        private class PSObjectConverter : JsonConverter<PSObject>
        {
            public override PSObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, PSObject value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.BaseObject, options);
            }
        }

        /// <summary>
        /// Converter to serialize/deserialize SecureString
        /// </summary>
        private class SecureStringConverter(bool mask) : JsonConverter<System.Security.SecureString>
        {
            public override System.Security.SecureString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var secureString = new System.Security.SecureString();
                var stringPassword = reader.GetString();

                if (stringPassword is null)
                    return secureString;

                foreach (char c in stringPassword)
                    secureString.AppendChar(c);
                return secureString;
            }
            public override void Write(Utf8JsonWriter writer, System.Security.SecureString value, JsonSerializerOptions options)
            {
                if (mask)
                {
                    writer.WriteStringValue("*****");
                }
                else
                {
                    var passwordString = System.Runtime.InteropServices.Marshal.PtrToStringUni(
                            System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(value));
                    writer.WriteStringValue(passwordString);
                }
            }
        }

        /// <summary>
        /// Deserialize JSON to <see cref="Dictionary{string, object?}"/> and serialize
        /// </summary>
        private class DictConverter : JsonConverter<Dictionary<string, object?>>
        {
            private static readonly ArrayConverter arrayConverter = new();
            public override Dictionary<string, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dict = new Dictionary<string, object?>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
                    }
                    string? propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                    reader.Read();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.Null:
                            dict.Add(propertyName, null); break;
                        case JsonTokenType.String:
                            if (reader.TryGetDateTime(out DateTime datetime))
                            {
                                dict.Add(propertyName, datetime.ToLocalTime());
                            }
                            else
                            {
                                dict.Add(propertyName, reader.GetString());
                            }
                            break;
                        case JsonTokenType.Number:
                            if (reader.TryGetInt32(out int int32val))
                            {
                                dict.Add(propertyName, int32val);
                            }
                            else if (reader.TryGetInt64(out long int64val))
                            {
                                dict.Add(propertyName, int64val);
                            }
                            else if (reader.TryGetUInt64(out ulong uint64val))
                            {
                                dict.Add(propertyName, uint64val);
                            }
                            else if (reader.TryGetDouble(out double doubleVal))
                            {
                                dict.Add(propertyName, doubleVal);
                            }
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            dict.Add(propertyName, reader.GetBoolean()); break;
                        case JsonTokenType.StartArray:
                            dict.Add(propertyName, arrayConverter.Read(ref reader, typeof(IList), options));
                            break;
                        case JsonTokenType.StartObject:
                            dict.Add(propertyName, Read(ref reader, typeToConvert, options));
                            break;
                        default:
                            throw new JsonException($"Invalid TokenType: {reader.TokenType}");
                    }
                }
                return dict;
            }

            public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> dict, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (var (key, val) in dict)
                {
                    writer.WritePropertyName(key);
                    JsonSerializer.Serialize(writer, val, options);
                }
                writer.WriteEndObject();
            }
        }
        /// <summary>
        /// Deserialize JSON to object[] and serialize
        /// </summary>
        private class ArrayConverter : JsonConverter<object?[]>
        {
            private static readonly DictConverter dictConverter = new();
            public override object?[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ArrayList array = [];
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndArray:
                            return array.ToArray();
                        case JsonTokenType.String:
                            if (reader.TryGetDateTime(out DateTime datetime))
                            {
                                array.Add(datetime);
                            }
                            else
                            {
                                array.Add(reader.GetString());
                            }
                            break;
                        case JsonTokenType.Number:
                            if (reader.TryGetInt32(out int int32val))
                            {
                                array.Add(int32val);
                            }
                            else if (reader.TryGetDouble(out double doubleVal))
                            {
                                array.Add(doubleVal);
                            }
                            else if (reader.TryGetInt64(out long int64val))
                            {
                                array.Add(int64val);
                            }
                            else if (reader.TryGetUInt64(out ulong uint64val))
                            {
                                array.Add(uint64val);
                            }
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            array.Add(reader.GetBoolean()); break;
                        case JsonTokenType.Null:
                            array.Add(null); break;
                        case JsonTokenType.StartArray:
                            array.Add(Read(ref reader, typeToConvert, options));
                            break;
                        case JsonTokenType.StartObject:
                            array.Add(dictConverter.Read(ref reader, typeof(Dictionary<string, object?>), options));
                            break;
                        default:
                            throw new JsonException($"Invalid TokenType: {reader.TokenType}");
                    }
                }
                throw new JsonException($"End unexpectly: {reader.TokenType}");
            }

            public override void Write(Utf8JsonWriter writer, object?[] list, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                foreach (var val in list)
                {
                    JsonSerializer.Serialize(writer, val, options);
                }
                writer.WriteEndArray();
            }
        }

        public class CapabilityConverter : JsonConverter<Capability>
        {
            public override Capability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var cap = Capability.None;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndObject:
                            return cap;
                        case JsonTokenType.PropertyName:
                            var propertyName = reader.GetString();
                            reader.Read();
                            var flag = reader.GetBoolean();
                            if (flag && propertyName is not null)
                            {
                                cap |= Enum.Parse<Capability>(propertyName, true);
                            }
                            break;
                        default:
                            throw new JsonException();
                    }
                }
                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, Capability value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (var capName in value.ToString("g").Split(", "))
                {
                    writer.WritePropertyName(capName.ToLowerInvariant());
                    writer.WriteBooleanValue(true);
                }
                writer.WriteEndObject();
            }
        }

        internal class SummaryFieldsHostConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeLastJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<HostLastJobSummary>(ref reader, options);
            }
            protected override object? DeserializeRecentJobs(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<HostRecentJobSummary>(ref reader, options);
            }
        }
        internal class SummaryFieldsOrganizationConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeRoles(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<Dictionary<string, OrganizationObjectRoleSummary>>(ref reader, options);
            }
        }
        internal class SummaryFieldsWorkflowJobNodeConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<WorkflowJobNodeJobSummary>(ref reader, options);
            }
        }
        internal class SummaryFieldsJobEventConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<JobExSummary>(ref reader, options);
            }
        }
        internal class SummaryFieldsJobHostSummaryConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<JobExSummary>(ref reader, options);
            }
        }
        internal class SummaryFieldsConverter : JsonConverter<SummaryFieldsDictionary>
        {
            public override SummaryFieldsDictionary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dict = new SummaryFieldsDictionary();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException($"TokenType is not PropertyName: {reader.TokenType}");
                    }
                    string? propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                    reader.Read();
                    string key = Utils.ToUpperCamelCase(propertyName);
                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        var array = new ArrayList();
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                            {
                                break;
                            }
                            array.Add(Deserialize(key, ref reader, options));
                        }
                        dict.Add(key, array.ToArray());
                    }
                    else
                    {
                        dict.Add(key, Deserialize(key, ref reader, options) ?? throw new JsonException($"{key}'s value is null"));
                    }
                }
                return dict;
            }
            private object? Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                switch (key)
                {
                    case "Actor":
                    case "CreatedBy":
                    case "ModifiedBy":
                    case "ApprovedOrDeniedBy":
                    case "User":
                        return JsonSerializer.Deserialize<UserSummary>(ref reader, options);
                    case "AdHocCommand":
                        return JsonSerializer.Deserialize<AdHocCommandSummary>(ref reader, options);
                    case "AncestorJob":
                        return JsonSerializer.Deserialize<AncestorJobSummary>(ref reader, options);
                    case "Application":
                        return JsonSerializer.Deserialize<ApplicationSummary>(ref reader, options);
                    case "Credential":
                    case "SourceCredential":
                    case "TargetCredential":
                        return JsonSerializer.Deserialize<CredentialSummary>(ref reader, options);
                    case "Credentials":
                        return JsonSerializer.Deserialize<JobTemplateCredentialSummary>(ref reader, options);
                    case "CredentialType":
                        return JsonSerializer.Deserialize<CredentialTypeSummary>(ref reader, options);
                    case "ExecutionEnvironment":
                    case "DefaultEnvironment":
                    case "ResolvedEnvironment":
                        return JsonSerializer.Deserialize<EnvironmentSummary>(ref reader, options);
                    case "DirectAccess":
                    case "IndirectAccess":
                        return JsonSerializer.Deserialize<AccessSummary>(ref reader, options);
                    case "Group":
                        return JsonSerializer.Deserialize<GroupSummary>(ref reader, options);
                    case "Groups":
                        return JsonSerializer.Deserialize<ListSummary<GroupSummary>>(ref reader, options);
                    case "Host":
                        return JsonSerializer.Deserialize<HostSummary>(ref reader, options);
                    case "Instance":
                        return JsonSerializer.Deserialize<InstanceSummary>(ref reader, options);
                    case "InstanceGroup":
                        return JsonSerializer.Deserialize<InstanceGroupSummary>(ref reader, options);
                    case "Inventory":
                        return JsonSerializer.Deserialize<InventorySummary>(ref reader, options);
                    case "InventorySource":
                        return JsonSerializer.Deserialize<InventorySourceSummary>(ref reader, options);
                    case "Job":
                        return DeserializeJob(ref reader, options);
                    case "JobTemplate":
                        return JsonSerializer.Deserialize<JobTemplateSummary>(ref reader, options);
                    case "LastJob":
                        return DeserializeLastJob(ref reader, options);
                    case "LastJobHostSummary":
                        return JsonSerializer.Deserialize<LastJobHostSummary>(ref reader, options);
                    case "LastUpdate":
                        return JsonSerializer.Deserialize<LastUpdateSummary>(ref reader, options);
                    case "Label":
                        return JsonSerializer.Deserialize<LabelSummary>(ref reader, options);
                    case "Labels":
                        return JsonSerializer.Deserialize<ListSummary<LabelSummary>>(ref reader, options);
                    case "Notification":
                        return JsonSerializer.Deserialize<NotificationSummary>(ref reader, options);
                    case "NotificationTemplate":
                        return JsonSerializer.Deserialize<NotificationTemplateSummary>(ref reader, options);
                    case "OAuth2AccessToken":
                        return JsonSerializer.Deserialize<TokenSummary>(ref reader, options);
                    case "Organization":
                        return JsonSerializer.Deserialize<OrganizationSummary>(ref reader, options);
                    case "ObjectRoles":
                        return DeserializeRoles(ref reader, options);
                    case "Owners":
                        return JsonSerializer.Deserialize<OwnerSummary>(ref reader, options);
                    case "Project":
                    case "SourceProject":
                        return JsonSerializer.Deserialize<ProjectSummary>(ref reader, options);
                    case "ProjectUpdate":
                        return JsonSerializer.Deserialize<ProjectUpdateSummary>(ref reader, options);
                    case "RecentJobs":
                        return DeserializeRecentJobs(ref reader, options);
                    case "RecentNotifications":
                        return JsonSerializer.Deserialize<RecentNotificationSummary>(ref reader, options);
                    case "RelatedFieldCounts":
                        return JsonSerializer.Deserialize<RelatedFieldCountsSummary>(ref reader, options);
                    case "Role":
                        return JsonSerializer.Deserialize<RoleSummary>(ref reader, options);
                    case "Schedule":
                        return JsonSerializer.Deserialize<ScheduleSummary>(ref reader, options);
                    case "Setting":
                        return JsonSerializer.Deserialize<SettingSummary>(ref reader, options);
                    case "SourceWorkflowJob":
                        return JsonSerializer.Deserialize<SourceWorkflowJobSummary>(ref reader, options);
                    case "Survey":
                        return JsonSerializer.Deserialize<SurveySummary>(ref reader, options);
                    case "Team":
                        return JsonSerializer.Deserialize<TeamSummary>(ref reader, options);
                    case "Tokens":
                        return JsonSerializer.Deserialize<ListSummary<TokenSummary>>(ref reader, options);
                    case "UnifiedJobTemplate":
                        return JsonSerializer.Deserialize<UnifiedJobTemplateSummary>(ref reader, options);
                    case "UserCapabilities":
                        return JsonSerializer.Deserialize<Capability>(ref reader, options);
                    case "WorkflowApprovalTemplate":
                        return JsonSerializer.Deserialize<WorkflowApprovalTemplateSummary>(ref reader, options);
                    case "WorkflowJob":
                        return JsonSerializer.Deserialize<WorkflowJobSummary>(ref reader, options);
                    case "WorkflowJobTemplate":
                        return JsonSerializer.Deserialize<WorkflowJobTemplateSummary>(ref reader, options);
                    case "WorkflowJobTemplateNode":
                        return JsonSerializer.Deserialize<WorkflowJobTemplateNodeSummary>(ref reader, options);
                    default:
                        throw new JsonException($"Unkown property name: {key}");
                }
            }
            protected virtual object? DeserializeLastJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<LastJobSummary>(ref reader, options);
            }
            protected virtual object? DeserializeRoles(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<Dictionary<string, ObjectRoleSummary>>(ref reader, options);
            }
            protected virtual object? DeserializeRecentJobs(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<RecentJobSummary>(ref reader, options);
            }
            protected virtual object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<JobTemplateJobSummary>(ref reader, options);
            }

            public override void Write(Utf8JsonWriter writer, SummaryFieldsDictionary value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (string key in value.Keys)
                {
                    writer.WritePropertyName(Utils.ToSnakeCase(key));
                    JsonSerializer.Serialize(writer, value[key], options);
                }
                writer.WriteEndObject();
            }
        }
    }
}
