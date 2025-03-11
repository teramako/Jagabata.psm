using System.Collections;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Jagabata.AlcEngine;

public static class Yaml
{
    /// <summary>
    /// Deserialize YAML string to Dictionary
    /// </summary>
    /// <param name="yaml">YAML or JSON string</param>
    /// <returns>Dictionary object</returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Dictionary<string, object?> DeserializeToDict(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml))
        {
            return [];
        }
        var parser = new Parser(new StringReader(yaml));
        parser.Consume<StreamStart>();
        parser.Consume<DocumentStart>();
        if (!parser.TryConsume<MappingStart>(out _))
        {
            // parsed as `Scalar` if the string is only document separator such as "---\n".
            // returns empty Dictionary in this case.
            return parser.Current is Scalar scalar && string.IsNullOrWhiteSpace(scalar.Value)
                ? []
                : throw new InvalidDataException($"YAML root is not dictionary.: {parser.Current}");
        }
        try
        {
            return ParseDict(parser);
        }
        catch (Exception ex)
        {
            throw new InvalidDataException("Faild to deserialize YAML", ex);
        }
    }

    private static object? ParseScalar(Scalar scalar)
    {
        if (scalar.IsQuotedImplicit)
            return scalar.Value;
        var stringValue = scalar.Value;
        switch (stringValue.ToLowerInvariant())
        {
            case "null":
                return null;
            case "true":
            case "yes":
                return true;
            case "false":
            case "no":
                return false;
        }
        return int.TryParse(stringValue, out var intVal)
            ? intVal
            : long.TryParse(stringValue, out var longVal)
            ? longVal
            : double.TryParse(stringValue, out var doubleVal)
            ? doubleVal
            : stringValue;
    }

    private static Dictionary<string, object?> ParseDict(IParser parser)
    {
        var dict = new Dictionary<string, object?>();
        while (!parser.TryConsume<MappingEnd>(out _))
        {
            var key = parser.Consume<Scalar>();
            if (parser.TryConsume<MappingStart>(out _))
            {
                dict.Add(key.Value, ParseDict(parser));
            }
            else if (parser.TryConsume<SequenceStart>(out _))
            {
                dict.Add(key.Value, ParseArray(parser));
            }
            else if (parser.TryConsume<Scalar>(out var scalar))
            {
                dict.Add(key.Value, ParseScalar(scalar));
            }
        }
        return dict;
    }

    private static object?[] ParseArray(IParser parser)
    {
        var array = new ArrayList();
        while (!parser.TryConsume<SequenceEnd>(out _))
        {
            if (parser.TryConsume<MappingStart>(out _))
            {
                array.Add(ParseDict(parser));
            }
            else if (parser.TryConsume<SequenceStart>(out _))
            {
                array.Add(ParseArray(parser));
            }
            else if (parser.TryConsume<Scalar>(out var scalar))
            {
                array.Add(ParseScalar(scalar));
            }
        }
        return array.ToArray();
    }

}
