using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using System.Collections;

namespace AWX
{
    public class Yaml
    {
        /// <summary>
        /// Deserialize YAML string to Dictionary
        /// </summary>
        /// <param name="yaml">YAML or JSON string</param>
        /// <returns>Dictionary object</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, object?> DeserializeToDict(string yaml)
        {
            var parser = new Parser(new StringReader(yaml));
            parser.Consume<StreamStart>();
            parser.Consume<DocumentStart>();
            if (!parser.TryConsume<MappingStart>(out _))
            {
                var msg = "YAML root should be dictionary.";
                var current = parser.Current;
                if (current is not null)
                {
                    msg += $": {current.GetType().Name} {{ Start: [{current.Start}] End: [{current.End}] }}";
                }
                throw new ArgumentException(msg);
            }
            try
            {
                return ParseDict(parser);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Faild to deserialize YAML", ex);
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
            if (int.TryParse(stringValue, out var intVal))
                return intVal;
            if (long.TryParse(stringValue, out var longVal))
                return longVal;
            if (double.TryParse(stringValue, out var doubleVal))
                return doubleVal;

            return stringValue;
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
}
