using System.Collections;
using System.Management.Automation;
using AWX.Resources;

namespace AWX.Cmdlets
{
    class ResourceIdTransformationAttribute : ResourceTransformationAttribute
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
        private IList<ulong> TransformList(IList list)
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
                inputData = pso.BaseObject;

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

    class ResourceTransformationAttribute : ArgumentTransformationAttribute
    {
        public ResourceType[] AcceptableTypes { get; init; } = [];

        private bool Validate(IResource resource)
        {
            if (resource.Id == 0) return false;
            if (AcceptableTypes.Length != 0)
                return AcceptableTypes.Any(type => resource.Type == type);
            return true;
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
        protected ResourceType ToResourceType(object? data)
        {
            switch (data)
            {
                case ResourceType resType:
                    return resType;
                case string strType:
                    if (Enum.TryParse<ResourceType>(strType, true, out var type))
                        return type;
                    break;
                case int intType:
                    return (ResourceType)intType;
            }
            throw new ArgumentException($"Could not convert to ResourcType: {data} ({data?.GetType().Name})");
        }
        protected ulong ToULong(object? data)
        {
            switch (data)
            {
                case int:
                case long:
                    if (ulong.TryParse($"{data}", out var id))
                        return id;
                    break;
                case uint:
                case ulong:
                    return (ulong)data;
            }
            throw new ArgumentException($"Could not convert to ulong: {data} ({data?.GetType().Name})");
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
            if (inputData is PSObject pso)
                inputData = pso.BaseObject;

            switch (inputData)
            {
                case IResource resource:
                    if (Validate(resource))
                        return resource;

                    break;
                case IDictionary dict:
                    ResourceType type = ResourceType.None;;
                    ulong id = 0;
                    foreach (var key in dict.Keys)
                    {
                        var strKey = key as string;
                        if (strKey is null) continue;
                        switch (strKey.ToLowerInvariant())
                        {
                            case "type":
                                type = ToResourceType(dict[key]);
                                continue;
                            case "id":
                                id = ToULong(dict[key]);
                                continue;
                        }
                    }
                    var res = new Resource(type, id);
                    if (Validate(res))
                        return res;

                    break;
            }
            throw new ArgumentException($"{nameof(inputData)} should be {nameof(IResource)} or {nameof(IDictionary)} has `Id` and `Type` keys." +
                                        $" And `Type` should be {string.Join(", ", AcceptableTypes)}");
        }
    }
}
