using System.Collections;
using System.Management.Automation;
using System.Text.Json;

namespace Jagabata.Cmdlets.ArgumentTransformation
{
    public class ExtraVarsArgumentTransformationAttribute : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case string str:
                    return str;
                case IDictionary dict:
                    return JsonSerializer.Serialize(dict, Json.SerializeOptions);
                default:
                    throw new ArgumentException($"Argument should be string or IDictionary");
            }
        }
    }
}
