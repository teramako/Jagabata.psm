namespace Jagabata.CredentialType;

/// <summary>
/// Url Input field
/// </summary>
public sealed class UrlField : StringField
{
    public UrlField() : base()
    {
        Format = FieldFormat.Url;
    }
    public UrlField(string id, string label) : base(id, label)
    {
        Format = FieldFormat.Url;
    }
}
