using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

/// <summary>
/// Selectbox field
/// </summary>
public sealed class ChoiceField : FieldBase
{
    public ChoiceField() : base(FieldType.String)
    { }
    public ChoiceField(string id, string label) : base(FieldType.String, id, label)
    { }

    public string[] Choices { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public override object? Default
    {
        get => _default;
        set => _default = value switch
        {
            null => null,
            string str => str,
            _ => throw new InvalidCastException($"value should be string type: {value.GetType()}"),
        };
    }
    private string? _default;

    public override string ToString()
    {
        var iv = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.Append('{');
        sb.Append(iv, $" Id = {Id}");
        sb.Append(iv, $", Label = {Label}");
        sb.Append(iv, $", Type = {Type}");
        if (_default is not null)
        {
            sb.Append(iv, $", Default = {_default}");
        }
        if (Choices is not null)
        {
            sb.Append(iv, $", Choices = [{string.Join(", ", Choices)}]");
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
