namespace Jagabata.CredentialType;

/// <summary>
/// SSH Provate Key field
/// </summary>
public sealed class SshPrivateKeyField : StringField
{
    public SshPrivateKeyField() : base()
    {
        Secret = true;
        Format = FieldFormat.SshPrivateKey;
        Multiline = true;
    }
    public SshPrivateKeyField(string id, string label) : base(id, label)
    {
        Secret = true;
        Format = FieldFormat.SshPrivateKey;
        Multiline = true;
    }
}
