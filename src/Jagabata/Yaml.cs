namespace Jagabata;

public static class Yaml
{
    /// <summary>
    /// Deserialize YAML string to Dictionary
    /// </summary>
    /// <param name="yaml">YAML or JSON string</param>
    /// <returns>Dictionary object</returns>
    public static Dictionary<string, object?> DeserializeToDict(string yaml)
    {
        return AlcEngine.Yaml.DeserializeToDict(yaml);
    }
}
