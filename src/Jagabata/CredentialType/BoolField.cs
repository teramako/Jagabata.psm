using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

/// <summary>
/// Checkbox field
/// </summary>
public sealed class BoolField : FieldBase
{
    public BoolField() : base(FieldType.Boolean)
    { }
    public BoolField(string id, string label) : base(FieldType.Boolean, id, label)
    { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public override object? Default
    {
        get => _default;
        set => _default = value switch
        {
            null => null,
            bool boolValue => boolValue,
            _ => throw new InvalidCastException($"value should be boolean type: {value.GetType()}"),
        };
    }
    private bool? _default;

    public override string ToString()
    {
        var iv = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.Append('{');
        sb.Append(iv, $" Id = {Id}");
        sb.Append(iv, $", Label = {Label}");
        sb.Append(iv, $", Type = {Type}");
        if (Required is not null)
        {
            sb.Append(iv, $", Required = {Required}");
        }
        if (Default is not null)
        {
            sb.Append(iv, $", Default = {Default}");
        }
        if (AskAtRuntime is not null)
        {
            sb.Append(iv, $", AskAtRuntime = {AskAtRuntime}");
        }
        if (HelpText is not null)
        {
            sb.Append(iv, $", HelpText = {HelpText}");
        }
        sb.Append(" }");
        return sb.ToString();
    }
}
