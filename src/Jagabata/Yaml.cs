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

    /// <summary>
    /// Serialize the specified object to string.
    /// <list type="bullet">
    ///     <item>PascalCase to snake_case</item>
    ///     <item>Indented sequences</item>
    ///     <item>Disable aliases</item>
    /// </list>
    /// </summary>
    /// <param name="obj">object to be serialized</param>
    /// <remarks>
    /// CAUTION: Not supported <see cref="System.Management.Automation.PSObject"/>
    /// </remarks>
    public static string SerializeToString(object obj)
    {
        return AlcEngine.Yaml.SerializeToString(obj);
    }
}
