using System.Management.Automation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
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
                .WithTypeConverter(new PSObjectTypeConverter())
                .Build());

    /// <summary>
    /// Serialize <see cref="PSObject"/>
    /// </summary>
    /// <remarks>
    /// Reference <c>ConvertTo-Yaml</c> cmdlet.
    /// See: <see href="https://github.com/cloudbase/powershell-yaml/blob/master/src/PowerShellYamlSerializer.cs"/>
    /// </remarks>
    internal class PSObjectTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            var result = typeof(PSObject).IsAssignableFrom(type);
            Console.WriteLine($"Accepts: {type} => {result}");
            return result;
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotImplementedException();
        }

        private static void EmitNull(IEmitter emitter)
        {
            emitter.Emit(new Scalar(AnchorName.Empty, "tag:yaml.org,2002:null", string.Empty, ScalarStyle.Plain, true, false));
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            Console.WriteLine($"WriteYaml: value type = {value?.GetType()}");
            if (value is not PSObject obj)
            {
                EmitNull(emitter);
                return;
            }
            var objType = obj.BaseObject.GetType();
            if (!typeof(PSCustomObject).IsAssignableFrom(objType))
            {
                serializer(obj.BaseObject, objType);
                return;
            }
            emitter.Emit(new MappingStart(AnchorName.Empty, TagName.Empty, true, MappingStyle.Block));
            foreach (var prop in obj.Properties)
            {
                serializer(prop.Name, prop.Name.GetType());
                if (prop.Value is null)
                {
                    EmitNull(emitter);
                    continue;
                }
                serializer(prop.Value, prop.Value.GetType());
            }
            emitter.Emit(new MappingEnd());
        }
    }
}
