using System.Collections;
using System.Management.Automation;
using System.Text.Json;

namespace Jagabata.Cmdlets.ArgumentTransformation
{
    internal class ExtraVarsArgumentTransformationAttribute : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            return inputData switch
            {
                string str => str,
                IDictionary dict => JsonSerializer.Serialize(dict, Json.SerializeOptions),
                _ => throw new ArgumentException($"Argument should be string or IDictionary"),
            };
        }
    }
}
