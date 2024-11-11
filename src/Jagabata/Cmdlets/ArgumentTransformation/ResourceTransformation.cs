using System.Collections;
using System.Management.Automation;
using Jagabata.Resources;

namespace Jagabata.Cmdlets.ArgumentTransformation
{
    internal class ResourceIdTransformationAttribute : ResourceTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case IList list:
                    return TransformList(list);
                case null:
                    return 0;
                default:
                    return TransformToId(inputData);
            }
        }
        private List<ulong> TransformList(IList list)
        {
            var arr = new List<ulong>();
            foreach (var inputItem in list)
            {
                arr.Add(TransformToId(inputItem));
            }
            return arr;
        }
        private ulong TransformToId(object inputData)
        {
            if (inputData is PSObject pso)
            {
                inputData = pso.BaseObject;
            }

            switch (inputData)
            {
                case int:
                case long:
                    if (ulong.TryParse($"{inputData}", out var id))
                        return id;
                    throw new ArgumentException();
                case uint:
                case ulong:
                    id = (ulong)inputData;
                    return id;
            }

            var resource = TransformToResource(inputData);
            return resource.Id;
        }
    }

    internal class ResourceTransformationAttribute : ArgumentTransformationAttribute
    {
        public ResourceType[] AcceptableTypes { get; init; } = [];

        private IResource Validate(IResource resource)
        {
            if (resource.Id == 0)
            {
                throw new ArgumentException("`Id` should be greater than 0");
            }
            if (AcceptableTypes.Length != 0)
            {
                if (!AcceptableTypes.Any(type => resource.Type == type))
                {
                    throw new ArgumentException($"`Type` should be one of [{string.Join(", ", AcceptableTypes)}]: {resource.Type}");
                }
            }
            return resource;
        }

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case IList list:
                    return TransformToList(list);
                case null:
                    return new Resource(0, 0);
                default:
                    return TransformToResource(inputData);
            }
        }
        protected IList<IResource> TransformToList(IList list)
        {
            var arr = new List<IResource>();
            foreach (var inputItem in list)
            {
                arr.Add(TransformToResource(inputItem));
            }
            return arr;
        }
        protected IResource TransformToResource(object inputData)
        {
            if (inputData is PSObject pso && pso.BaseObject is not PSCustomObject)
            {
                inputData = pso.BaseObject;
            }

            (ResourceType Type, ulong Id) resourceData = (ResourceType.None, 0);

            switch (inputData)
            {
                case IResource resource:
                    return Validate(resource);
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
            return Validate(new Resource(resourceData.Type, resourceData.Id));
        }
    }
}
