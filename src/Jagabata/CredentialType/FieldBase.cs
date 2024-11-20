using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Jagabata.CredentialType;

public abstract partial class FieldBase : IEquatable<FieldBase>
{
    public FieldBase(FieldType type)
    {
        Type = type;
    }
    public FieldBase(FieldType type, string id)
    {
        Type = type;
        Id = id;
    }
    public FieldBase(FieldType type, string id, string label)
    {
        Type = type;
        Id = id;
        Label = label;
    }

    [GeneratedRegex(@"^[a-zA-Z_]+[a-zA-Z0-9_]*$")]
    private static partial Regex idPattern();

    /// <summary>
    /// A unique name used to reference the field value
    /// </summary>
    [JsonPropertyOrder(1)]
    public string Id
    {
        get => _id;
        init
        {
            if (!idPattern().IsMatch(value))
            {
                throw new InvalidDataException("Id must be '^[a-zA-Z_]+[a-zA-Z0-9]*$'");
            }
            _id = value;
        }
    }
    private string _id = string.Empty;

    /// <summary>
    /// A unique label for the field
    /// </summary>
    [JsonPropertyOrder(2)]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Field type: "String" or "Boolean" (default: "String")
    /// </summary>
    [JsonPropertyOrder(3)]
    public FieldType Type { get; } = FieldType.String;

    /// <summary>
    /// User-facing short text describing the field.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HelpText { get; set; }

    [JsonIgnore]
    public bool? Required { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual object? Default { get; set; }

    /// <summary>
    /// If true, this field is given a checkbox that can be used to prompt for input on startup.
    /// </summary>
    /// <remarks>
    /// For only managed CredentialType.
    /// (Not supported for custom CredentialType)
    /// </remarks>
    [JsonIgnore]
    public bool? AskAtRuntime { get; internal set; }

    public bool Equals(FieldBase? other)
    {
        return other is not null && string.Equals(Id, other.Id, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is FieldBase field && Equals(field);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
