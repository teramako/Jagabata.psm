using System.Collections;
using System.Management.Automation;
using System.Text.Json;

namespace Jagabata.Cmdlets.ArgumentTransformation;

internal class DictionaryTransformation : ArgumentTransformationAttribute
{
    public DictionaryTransformation(params Type[] types)
    {
        Types = types;
    }

    protected Type[] Types { get; init; }

    public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
    {
        if (inputData is PSObject pso && pso.BaseObject is not null)
        {
            inputData = pso.BaseObject;
        }
        if (inputData is IDictionary dict)
        {
            return dict;
        }
        foreach (var t in Types)
        {
            if (inputData.GetType() != t)
            {
                continue;
            }
            var jsonElm = JsonSerializer.SerializeToElement(inputData, t, Json.SerializeOptions);
            return JsonSerializer.Deserialize<Dictionary<string, object?>>(jsonElm, Json.DeserializeOptions)
                ?? throw new ArgumentException("result is null");
        }
        throw new ArgumentException($"{nameof(inputData)} should be one of [IDictionary, {string.Join(", ", Types.Select(t => t.Name))}]");
    }
}
