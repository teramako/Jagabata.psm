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
            var options = pretty ? SerializeOptionsForPrettyPrinting : SerializeOptions;
            return JsonSerializer.Serialize(obj, options);
        }
        private static OrderedDictionary ToDict(JsonElement json)
        {
            var dict = new OrderedDictionary();
            foreach (var kv in json.EnumerateObject())
            {
                dict.Add(kv.Name, ObjectToInferredType(kv.Value));
            }
            return dict;
        }
        private static object?[] ToArray(JsonElement json)
        {
            var length = json.GetArrayLength();
            object?[] array = new object?[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = ObjectToInferredType(json[i]);
            }
            return array;
        }

        /// <summary>
        /// Deserialize JSON string to Jagabata resource object or <see cref="OrderedDictionary"/>
        /// </summary>
        /// <param name="json">Json String</param>
        /// <returns>Deserialized object or <c>null</c></returns>
        public static object? Load(ReadOnlySpan<char> json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json, DeserializeOptions);
            return ObjectToInferredType(jsonElement, true);
        }

        /// <summary>
        /// Deserialize <see cref="JsonElement"/> to Jagabata resource object or <see cref="OrderedDictionary"/>
        /// </summary>
        /// <param name="val">Json String</param>
        /// <param name="isRoot">when <c>true</c>, infer the object type</param>
        /// <returns>Deserialized object or <c>null</c></returns>
        internal static object? ObjectToInferredType(JsonElement val, bool isRoot = false)
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
                    if (val.TryGetInt32(out var intVal))
                        return intVal;
                    else if (val.TryGetInt64(out var longVal))
                        return longVal;
                    else if (val.TryGetUInt64(out var ulongVal))
                        return ulongVal;
                    else if (val.TryGetDouble(out var doubleVal))
                        return doubleVal;
                    throw new JsonException($"Could not convert number: {val.GetRawText()}");
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
        internal class RelatedResourceConverter : JsonConverter<RelatedDictionary>
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
                            dict.Add(propertyName, reader.GetString() ?? string.Empty);
                            break;
                        case JsonTokenType.StartArray:
                            dict.Add(propertyName, JsonSerializer.Deserialize<string[]>(ref reader, options) ?? []);
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
        internal class EnumUpperCamelCaseStringConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
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
        private class LocalDateTimeConverter : JsonConverter<DateTime>
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

        private static readonly Lazy<LocalDateTimeConverter> _localDateTimeConverter = new();
        private static readonly Lazy<SecureStringConverter> _secureStringConverter = new(static () => new SecureStringConverter(mask: false));
        private static readonly Lazy<SecureStringConverter> _maskedSecureStringConverter = new(static () => new SecureStringConverter(mask: true));
        private static readonly Lazy<PSObjectConverter> _psObjectConverter = new();
        private static readonly Lazy<DictConverter> _dictConverter = new();
        private static readonly Lazy<ArrayConverter> _arrayConverter = new();

        private static readonly Lazy<JavaScriptEncoder> _javaScriptEncoder = new(static () => JavaScriptEncoder.Create(UnicodeRanges.All));

        /// <seealso cref="DeserializeOptions"/>
        private static readonly Lazy<JsonSerializerOptions> _deserializeOptions = new(static () => new JsonSerializerOptions()
        {
            // UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                _localDateTimeConverter.Value,
                _dictConverter.Value,
                _secureStringConverter.Value,
            }
        });
        /// <summary>
        /// Json Serialize / Deserialize OPTIONS for this API.
        /// <list type="bullet">
        ///   <item>Property name to snake_case for serialization</item>
        ///   <item>Property name in case sensitive for deserialization</item>
        ///   <item>DateTime to Local time zone for deserialization, to Utc for serialization</item>
        ///   <item>Non classed object to <c>Dictionary&lt;string, object?&gt;</c> for deserialization</item>
        /// </list>
        /// </summary>
        public static JsonSerializerOptions DeserializeOptions => _deserializeOptions.Value;

        /// <seealso cref="SerializeOptions"/>
        private static readonly Lazy<JsonSerializerOptions> _serializeOptions = new(static () => new JsonSerializerOptions()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            Encoder = _javaScriptEncoder.Value,
            Converters =
            {
                _psObjectConverter.Value,
                _localDateTimeConverter.Value,
                _maskedSecureStringConverter.Value,
            }
        });
        /// <summary>
        /// Json Serialize OPTIONS for this API.
        /// <list type="bullet">
        ///   <item>Serialize property name to snake_case</item>
        ///   <item>Serialize <see cref="DateTime"/> to Utc</item>
        ///   <item>Serialize <see cref="PSObject"/></item>
        ///   <item>Serialize <see cref="System.Security.SecureString"/> to masked (<c>*****</c>) string</item>
        /// </list>
        /// </summary>
        public static JsonSerializerOptions SerializeOptions => _serializeOptions.Value;

        /// <seealso cref="SerializeOptionsForPrettyPrinting"/>
        private static readonly Lazy<JsonSerializerOptions> _serializeOptionsForPrettyPrinting = new(static () => new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            Encoder = _javaScriptEncoder.Value,
            Converters =
            {
                _psObjectConverter.Value,
                _localDateTimeConverter.Value,
                _maskedSecureStringConverter.Value,
            }
        });
        /// <summary>
        /// Json Serialize OPTIONS for pretty printing
        /// <list type="bullet">
        ///   <item>Serialize property name to snake_case</item>
        ///   <item>Serialize <see cref="DateTime"/> to Utc</item>
        ///   <item>Serialize <see cref="PSObject"/></item>
        ///   <item>Serialize <see cref="System.Security.SecureString"/> to masked (<c>*****</c>) string</item>
        /// </list>
        /// </summary>
        public static JsonSerializerOptions SerializeOptionsForPrettyPrinting => _serializeOptionsForPrettyPrinting.Value;

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
                if (value.BaseObject is PSCustomObject)
                {
                    writer.WriteStartObject();
                    foreach (var prop in value.Properties)
                    {
                        switch (prop.MemberType)
                        {
                            case PSMemberTypes.CodeProperty:
                            case PSMemberTypes.Property:
                            case PSMemberTypes.NoteProperty:
                            case PSMemberTypes.ScriptProperty:
                                writer.WritePropertyName(prop.Name);
                                JsonSerializer.Serialize(writer, prop.Value, options);
                                continue;
                        }
                    }
                    writer.WriteEndObject();
                }
                else
                {
                    JsonSerializer.Serialize(writer, value.BaseObject, options);
                }
            }
        }

        /// <summary>
        /// Converter to serialize/deserialize <see cref="System.Security.SecureString"/>
        /// </summary>
        /// <param name="mask">Serialize to masked string (<c>*****</c>) when <c>true</c></param>
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
        /// Deserialize <c>Object</c> to <c>Dictionary&lt;string, object?&gt;</c>
        /// </summary>
        private class DictConverter : JsonConverter<Dictionary<string, object?>>
        {
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
                            dict.Add(propertyName, _arrayConverter.Value.Read(ref reader, typeof(IList), options));
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
                            else if (reader.TryGetInt64(out long int64val))
                            {
                                array.Add(int64val);
                            }
                            else if (reader.TryGetUInt64(out ulong uint64val))
                            {
                                array.Add(uint64val);
                            }
                            else if (reader.TryGetDouble(out double doubleVal))
                            {
                                array.Add(doubleVal);
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
                            array.Add(_dictConverter.Value.Read(ref reader, typeof(Dictionary<string, object?>), options));
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

        internal class CapabilityConverter : JsonConverter<Capability>
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
            protected override object? DeserializeLastJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<HostLastJobSummary>(ref reader, options, isArray);
            }
            protected override object? DeserializeRecentJobs(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<HostRecentJobSummary>(ref reader, options, isArray);
            }
        }
        internal class SummaryFieldsOrganizationConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeRoles(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<Dictionary<string, OrganizationObjectRoleSummary>>(ref reader, options, isArray);
            }
        }
        internal class SummaryFieldsWorkflowJobNodeConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<WorkflowJobNodeJobSummary>(ref reader, options, isArray);
            }
        }
        internal class SummaryFieldsJobEventConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<JobExSummary>(ref reader, options, isArray);
            }
        }
        internal class SummaryFieldsJobHostSummaryConverter : SummaryFieldsConverter
        {
            protected override object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<JobExSummary>(ref reader, options, isArray);
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
                    string propertyName = reader.GetString() ?? throw new JsonException("PropertyName is null");
                    reader.Read();
                    string key = Utils.ToUpperCamelCase(propertyName);
                    dict.Add(key, Deserialize(key, ref reader, options, reader.TokenType == JsonTokenType.StartArray)
                            ?? throw new JsonException($"{key}'s value is null"));
                }
                return dict;
            }
            protected static object? Deserialize<T>(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray = false)
            {
                return isArray
                    ? JsonSerializer.Deserialize<T[]>(ref reader, options)
                    : JsonSerializer.Deserialize<T>(ref reader, options);
            }
            private object? Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray = false)
            {
                switch (key)
                {
                    case "Actor":
                    case "CreatedBy":
                    case "ModifiedBy":
                    case "ApprovedOrDeniedBy":
                    case "User":
                        return Deserialize<UserSummary>(ref reader, options, isArray);
                    case "AdHocCommand":
                        return Deserialize<AdHocCommandSummary>(ref reader, options, isArray);
                    case "AncestorJob":
                        return Deserialize<AncestorJobSummary>(ref reader, options, isArray);
                    case "Application":
                    case "OAuth2Application":
                        return Deserialize<ApplicationSummary>(ref reader, options, isArray);
                    case "Credential":
                    case "SourceCredential":
                    case "TargetCredential":
                        return Deserialize<CredentialSummary>(ref reader, options, isArray);
                    case "Credentials":
                        return Deserialize<JobTemplateCredentialSummary>(ref reader, options, isArray);
                    case "CredentialType":
                        return Deserialize<CredentialTypeSummary>(ref reader, options, isArray);
                    case "ExecutionEnvironment":
                    case "DefaultEnvironment":
                    case "ResolvedEnvironment":
                        return Deserialize<EnvironmentSummary>(ref reader, options, isArray);
                    case "DirectAccess":
                    case "IndirectAccess":
                        return Deserialize<AccessSummary>(ref reader, options, isArray);
                    case "Group":
                        return Deserialize<GroupSummary>(ref reader, options, isArray);
                    case "Groups":
                        return Deserialize<ListSummary<GroupSummary>>(ref reader, options, isArray);
                    case "Host":
                        return Deserialize<HostSummary>(ref reader, options, isArray);
                    case "Instance":
                        return Deserialize<InstanceSummary>(ref reader, options, isArray);
                    case "InstanceGroup":
                        return Deserialize<InstanceGroupSummary>(ref reader, options, isArray);
                    case "Inventory":
                        return Deserialize<InventorySummary>(ref reader, options, isArray);
                    case "InventorySource":
                        return Deserialize<InventorySourceSummary>(ref reader, options, isArray);
                    case "Job":
                        return DeserializeJob(ref reader, options, isArray);
                    case "JobTemplate":
                        return Deserialize<JobTemplateSummary>(ref reader, options, isArray);
                    case "LastJob":
                        return DeserializeLastJob(ref reader, options, isArray);
                    case "LastJobHostSummary":
                        return Deserialize<LastJobHostSummary>(ref reader, options, isArray);
                    case "LastUpdate":
                        return Deserialize<LastUpdateSummary>(ref reader, options, isArray);
                    case "Label":
                        return Deserialize<LabelSummary>(ref reader, options, isArray);
                    case "Labels":
                        return Deserialize<ListSummary<LabelSummary>>(ref reader, options, isArray);
                    case "Notification":
                        return Deserialize<NotificationSummary>(ref reader, options, isArray);
                    case "NotificationTemplate":
                        return Deserialize<NotificationTemplateSummary>(ref reader, options, isArray);
                    case "OAuth2AccessToken":
                        return Deserialize<TokenSummary>(ref reader, options, isArray);
                    case "Organization":
                        return Deserialize<OrganizationSummary>(ref reader, options, isArray);
                    case "ObjectRoles":
                        return DeserializeRoles(ref reader, options, isArray);
                    case "Owners":
                        return Deserialize<OwnerSummary>(ref reader, options, isArray);
                    case "Project":
                    case "SourceProject":
                        return Deserialize<ProjectSummary>(ref reader, options, isArray);
                    case "ProjectUpdate":
                        return Deserialize<ProjectUpdateSummary>(ref reader, options, isArray);
                    case "RecentJobs":
                        return DeserializeRecentJobs(ref reader, options, isArray);
                    case "RecentNotifications":
                        return Deserialize<RecentNotificationSummary>(ref reader, options, isArray);
                    case "RelatedFieldCounts":
                        return Deserialize<RelatedFieldCountsSummary>(ref reader, options, isArray);
                    case "Role":
                        return Deserialize<RoleSummary>(ref reader, options, isArray);
                    case "Schedule":
                        return Deserialize<ScheduleSummary>(ref reader, options, isArray);
                    case "Setting":
                        return Deserialize<SettingSummary>(ref reader, options, isArray);
                    case "SourceWorkflowJob":
                        return Deserialize<SourceWorkflowJobSummary>(ref reader, options, isArray);
                    case "Survey":
                        return Deserialize<SurveySummary>(ref reader, options, isArray);
                    case "Team":
                        return Deserialize<TeamSummary>(ref reader, options, isArray);
                    case "Tokens":
                        return Deserialize<ListSummary<TokenSummary>>(ref reader, options, isArray);
                    case "UnifiedJobTemplate":
                        return Deserialize<UnifiedJobTemplateSummary>(ref reader, options, isArray);
                    case "UserCapabilities":
                        return Deserialize<Capability>(ref reader, options, isArray);
                    case "WebhookCredential":
                        return Deserialize<WebhookCredentialSummary>(ref reader, options, isArray);
                    case "WorkflowApprovalTemplate":
                        return Deserialize<WorkflowApprovalTemplateSummary>(ref reader, options, isArray);
                    case "WorkflowJob":
                        return Deserialize<WorkflowJobSummary>(ref reader, options, isArray);
                    case "WorkflowJobTemplate":
                        return Deserialize<WorkflowJobTemplateSummary>(ref reader, options, isArray);
                    case "WorkflowJobTemplateNode":
                        return Deserialize<WorkflowJobTemplateNodeSummary>(ref reader, options, isArray);
                    default:
                        throw new JsonException($"Unkown property name: {key}");
                }
            }
            protected virtual object? DeserializeLastJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<LastJobSummary>(ref reader, options, isArray);
            }
            protected virtual object? DeserializeRoles(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<Dictionary<string, ObjectRoleSummary>>(ref reader, options, isArray);
            }
            protected virtual object? DeserializeRecentJobs(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<RecentJobSummary>(ref reader, options, isArray);
            }
            protected virtual object? DeserializeJob(ref Utf8JsonReader reader, JsonSerializerOptions options, bool isArray)
            {
                return Deserialize<JobTemplateJobSummary>(ref reader, options, isArray);
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
