namespace Jagabata.CredentialType;

/// <summary>
/// Secret Input field
/// </summary>
public class SecretField : StringField
{
    public SecretField() : base()
    {
        Secret = true;
    }
    public SecretField(string id, string label) : base(id, label)
    {
        Secret = true;
    }
}
