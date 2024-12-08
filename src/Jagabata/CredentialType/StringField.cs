using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

/// <summary>
/// Normal Input field
/// </summary>
public class StringField : FieldBase
{
    public StringField() : base(FieldType.String)
    { }
    public StringField(string id, string label) : base(FieldType.String, id, label)
    { }

    /// <summary>
    /// Optional, can be used to enforce data format validity for SSH private key data
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FieldFormat? Format { get; internal set; }

    /// <summary>
    /// Optional, can be used to provide a default value if the field is left empty;
    /// when creating a credential of this type, credential forms will use this value
    /// as a prefill when making credentials of this type
    /// </summary>
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

    /// <summary>
    /// If true, the field value will be encrypted
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Secret { get; internal set; }

    /// <summary>
    /// If true, the field should be rendered as multi-line for input entry
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Multiline { get; set; }

    public override string ToString()
    {
        var iv = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.Append('{');
        sb.Append(iv, $" Id = {Id}");
        sb.Append(iv, $", Label = {Label}");
        sb.Append(iv, $", Type = {Type}");
        if (Secret is not null)
        {
            sb.Append(iv, $", Secret = {Secret}");
        }
        if (_default is not null)
        {
            sb.Append(iv, $", Default = {_default}");
        }
        if (Format is not null)
        {
            sb.Append(iv, $", Format = {Format}");
        }
        if (Multiline is not null)
        {
            sb.Append(iv, $", Multiline = {Multiline}");
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
