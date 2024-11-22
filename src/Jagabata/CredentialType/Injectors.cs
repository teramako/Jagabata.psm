using System.Collections;
using System.Text.Json.Serialization;

namespace Jagabata.CredentialType;

public class Injectors(StringDictionary? env = null,
                       ObjectDictionary? extraVars = null,
                       StringDictionary? file = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StringDictionary? Env { get; set; } = env;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ObjectDictionary? ExtraVars { get; set; } = extraVars;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StringDictionary? File { get; set; } = file;
}

public class StringDictionary : Dictionary<string, string>
{
    public StringDictionary()
    { }
    public StringDictionary(IDictionary dict)
    {
        foreach (var entry in dict.Cast<DictionaryEntry>())
        {
            this[$"{entry.Key}"] = $"{entry.Value}";
        }
    }
}

public class ObjectDictionary : Dictionary<string, object?>
{
    public ObjectDictionary()
    { }
    public ObjectDictionary(IDictionary dict)
    {
        foreach (var entry in dict.Cast<DictionaryEntry>())
        {
            this[$"{entry.Key}"] = entry.Value switch
            {
                IDictionary subDict => ConvertDict(subDict),
                IList list => ConvertList(list),
                _ => entry.Value
            };
        }
    }
    private static Dictionary<string, object?> ConvertDict(IDictionary dict)
    {
        return dict.Cast<DictionaryEntry>().ToDictionary(
                d => $"{d.Key}",
                d => d.Value switch
                {
                    IDictionary subDict => ConvertDict(subDict),
                    IList list => ConvertList(list),
                    _ => d.Value
                });
    }
    private static ArrayList ConvertList(IList list)
    {
        var result = new ArrayList();
        foreach (var item in list)
        {
            result.Add(item switch
            {
                IDictionary subDict => ConvertDict(subDict),
                IList subList => ConvertList(subList),
                _ => item
            });
        }
        return result;
    }
}
