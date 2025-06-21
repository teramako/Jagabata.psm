using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation;

namespace Jagabata.Cmdlets.ArgumentTransformation
{
    /// <summary>
    /// Transform argument(s) or pipeline input(s) to Resource ID (ulong);
    /// </summary>
    internal class ResourceIdTransformationAttribute(params ResourceType[] acceptTypes)
        : ResourceTransformationAttribute(acceptTypes)
    {
        private const ulong FALLBACK_VALUE = 0;
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case IList list:
                    return TransformToList(list, engineIntrinsics).Select(static res => res.Id).ToArray();
                case null:
                    return FALLBACK_VALUE;
                default:
                    var resource = TransformToResource(inputData);
                    if (!Validate(resource, out var warningMessage))
                    {
                        WriteWarning(engineIntrinsics,
                                     $"Skip the inputted resource [{resource.Type}:{resource.Id}]: {warningMessage}");
                        return FALLBACK_VALUE;
                    }
                    return resource.Id;
            }
        }
    }

    /// <summary>
    /// Transform argument(s) or pipeline input(s) to IResource object(s)
    /// </summary>
    internal class ResourceTransformationAttribute(params ResourceType[] acceptTypes)
        : ArgumentTransformationAttribute
    {
        public ResourceType[] AcceptableTypes { get; init; } = acceptTypes;

        private static readonly Resource FALLBACK_VALUE = new(0, 0);

        protected bool Validate(IResource resource, [MaybeNullWhen(true)] out string msg)
        {
            if (resource.Id == 0)
            {
                msg = "Resource ID should be greater than 0";
                return false;
            }
            if (AcceptableTypes.Length != 0)
            {
                if (!AcceptableTypes.Any(type => resource.Type == type))
                {
                    msg = AcceptableTypes.Length == 1
                          ? $"Resource type should be \"{AcceptableTypes[0]}\""
                          : $"Resource type should be one of [{string.Join(", ", AcceptableTypes)}]";
                    return false;
                }
            }
            msg = null;
            return true;
        }

        protected static void WriteWarning(EngineIntrinsics engineIntrinsics, string message)
        {
            var warningPreference = (ActionPreference)engineIntrinsics.SessionState.PSVariable.GetValue("WarningPreference", ActionPreference.Continue);
            switch (warningPreference)
            {
                case ActionPreference.SilentlyContinue:
                case ActionPreference.Ignore:
                    return;
                default:
                    engineIntrinsics.Host.UI.WriteWarningLine(message);
                    break;
            }
        }

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case IList list:
                    return TransformToList(list, engineIntrinsics);
                case null:
                    return FALLBACK_VALUE;
                default:
                    var resource = TransformToResource(inputData);
                    if (!Validate(resource, out var warningMessage))
                    {
                        WriteWarning(engineIntrinsics,
                                     $"Skip the inputted resource [{resource.Type}:{resource.Id}]: {warningMessage}");
                        return FALLBACK_VALUE;
                    }
                    return resource;
            }
        }
        protected IList<IResource> TransformToList(IList list, EngineIntrinsics engineIntrinsics)
        {
            var arr = new List<IResource>();
            foreach (var inputItem in list)
            {
                var resource = TransformToResource(inputItem);
                if (!Validate(resource, out var warningMessage))
                {
                    WriteWarning(engineIntrinsics, $"Skip the inputted resource [{resource.Type}:{resource.Id}]: {warningMessage}");
                    continue;
                }
                arr.Add(resource);
            }
            return arr;
        }
        protected IResource TransformToResource(object inputData)
        {
            if (inputData is PSObject pso && pso.BaseObject is not PSCustomObject)
            {
                inputData = pso.BaseObject;
            }

            if (AcceptableTypes.Length == 1 && LanguagePrimitives.TryConvertTo<ulong>(inputData, out var id))
            {
                return new Resource(AcceptableTypes[0], id);
            }

            (ResourceType Type, ulong Id) resourceData = (ResourceType.None, 0);

            switch (inputData)
            {
                case string str:
                    return Resource.Parse(str, CultureInfo.InvariantCulture);
                case IResource resource:
                    return resource;
                case IDictionary dict:
                    foreach (var key in dict.Keys)
                    {
                        if (key is not string strKey)
                        {
                            continue;
                        }

                        switch (strKey.ToLowerInvariant())
                        {
                            case "type":
                                if (!LanguagePrimitives.TryConvertTo<ResourceType>(dict[key], out resourceData.Type))
                                {
                                    throw new ArgumentException($"Could not convert '{key}' to ResourcType: {dict[key]}");
                                }
                                break;
                            case "id":
                                if (!LanguagePrimitives.TryConvertTo<ulong>(dict[key], out resourceData.Id))
                                {
                                    throw new ArgumentException($"Could not convert '{key}' to ulong: {dict[key]}");
                                }
                                break;
                        }
                        if (resourceData.Id > 0 && resourceData.Type > 0)
                        {
                            break;
                        }
                    }
                    break;
                case PSObject ps:
                    foreach (var p in ps.Properties)
                    {
                        switch (p.Name.ToLowerInvariant())
                        {
                            case "type":
                                if (!LanguagePrimitives.TryConvertTo<ResourceType>(p.Value, out resourceData.Type))
                                {
                                    throw new ArgumentException($"Could not convert '{p.Name}' to ResourcType: {p.Value}");
                                }
                                break;
                            case "id":
                                if (!LanguagePrimitives.TryConvertTo<ulong>(p.Value, out resourceData.Id))
                                {
                                    throw new ArgumentException($"Could not convert '{p.Name}' to ulong: {p.Value}");
                                }
                                break;
                        }
                        if (resourceData.Id > 0 && resourceData.Type > 0)
                        {
                            break;
                        }
                    }
                    break;
                default:
                    {
                        var t = inputData.GetType();
                        var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        foreach (var p in props)
                        {
                            switch (p.Name.ToLowerInvariant())
                            {
                                case "type":
                                    var typeObj = p.GetValue(inputData);
                                    if (!LanguagePrimitives.TryConvertTo<ResourceType>(typeObj, out resourceData.Type))
                                    {
                                        throw new ArgumentException($"Could not convert '{p.Name}' to ResourcType: {typeObj}");
                                    }
                                    break;
                                case "id":
                                    var idObj = p.GetValue(inputData);
                                    if (!LanguagePrimitives.TryConvertTo<ulong>(idObj, out resourceData.Id))
                                    {
                                        throw new ArgumentException($"Could not convert '{p.Name}' to ulong: {idObj}");
                                    }
                                    break;
                            }
                            if (resourceData.Id > 0 && resourceData.Type > 0)
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
            return new Resource(resourceData.Type, resourceData.Id);
        }
    }
}
