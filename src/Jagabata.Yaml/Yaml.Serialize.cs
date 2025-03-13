using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Jagabata.AlcEngine;

public static partial class Yaml
{
    /// <summary>
    /// Serialize the specified object to string.
    /// <list type="bullet">
    ///     <item>PascalCase to snake_case</item>
    ///     <item>Indented sequences</item>
    ///     <item>Disable aliases</item>
    /// </list>
    /// </summary>
    /// <param name="obj">object to be serialized</param>
    public static string SerializeToString(object obj)
    {
        return serializer.Value.Serialize(obj);
    }

    private static readonly Lazy<ISerializer> serializer = new(static () =>
            new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithIndentedSequences()
                .DisableAliases()
                .Build());
}
