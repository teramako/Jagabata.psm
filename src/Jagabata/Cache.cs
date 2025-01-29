using Jagabata.Resources;

namespace Jagabata;

public interface ICacheableResource : IResource
{
    string GetDescription();
}

public interface IHasCachableItems
{
    IEnumerable<ICacheableResource> GetCacheableItems();
}

public class Caches
{
    private static readonly Lazy<List<Item>> _items = new(static () => []);
    internal static List<Item> Data => _items.Value;
    // FIXME: to be configurable
    public static int MAX_COUNT = 100;

    public enum CacheType
    {
        Container, Summary
    }
    public class Item(ICacheableResource res, CacheType cacheType = CacheType.Container) : IEquatable<Item>
    {
        public ResourceType Type { get; } = res.Type;
        public ulong Id { get; } = res.Id;
        public string Description { get; set; } = res.GetDescription();
        public CacheType CacheType { get; } = cacheType;
        public DateTime CachedTimestamp { get; internal set; } = DateTime.Now;

        public bool Equals(Item? other)
        {
            return other is not null && Type == other.Type && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Item);
        }

        public override int GetHashCode()
        {
            return ((int)Type).GetHashCode() ^ Id.GetHashCode();
        }
    }

    public static void Add(Item item)
    {
        var index = Data.IndexOf(item);
        if (index >= 0)
        {
            var currentItem = Data[index];
            Data.RemoveAt(index);
            if (currentItem.CacheType == CacheType.Summary)
            {
                Data.Add(item);
            }
            else if (item.CacheType == CacheType.Container)
            {
                Data.Add(item);
            }
            else
            {
                currentItem.CachedTimestamp = item.CachedTimestamp;
                Data.Add(currentItem);
            }
        }
        else
        {
            Data.Add(item);
        }

        while (Data.Count > MAX_COUNT)
        {
            Data.RemoveAt(0);
        }
    }

    private static IEnumerable<Item> GetItems(IResource resource)
    {
        if (resource is ICacheableResource res)
        {
            yield return new Item(res, CacheType.Container);
        }
        if (resource is IHasCachableItems c)
        {
            foreach (var child in c.GetCacheableItems())
            {
                yield return new Item(child, CacheType.Summary);
            }
        }
    }
    private static IEnumerable<Item> GetItems(IEnumerable<IResource> resources)
    {
        foreach (var resource in resources)
        {
            if (resource is ICacheableResource res)
            {
                yield return new Item(res, CacheType.Container);
            }
            if (resource is IHasCachableItems c)
            {
                foreach (var child in c.GetCacheableItems())
                {
                    yield return new Item(child, CacheType.Summary);
                }
            }
        }
    }

    public static void Add(IResource resource)
    {
        foreach (var item in new HashSet<Item>(GetItems(resource)))
        {
            Add(item);
        }
    }

    public static void Add(IEnumerable<IResource> resources)
    {
        foreach (var item in new HashSet<Item>(GetItems(resources)))
        {
            Add(item);
        }
    }

    public static void Clear()
    {
        Data.Clear();
    }

    public static IEnumerable<Item> GetEnumerator(params ResourceType[] types)
    {
        return types is { Length: 0 } ? Data : Data.Where(item => types.Contains(item.Type));
    }

    internal static IEnumerable<ICacheableResource> GetCacheableResources(IDictionary<string, object> summaryFields)
    {
        foreach (var summaryItem in summaryFields.Values)
        {
            switch (summaryItem)
            {
                case Array arr:
                    foreach (var item in arr.OfType<ICacheableResource>())
                    {
                        yield return item;
                    }
                    continue;
                case ListSummary<ICacheableResource> list:
                    foreach (var item in list.Results.AsEnumerable())
                    {
                        yield return item;
                    }
                    continue;
                case ICacheableResource res:
                    yield return res;
                    continue;
                default:
                    continue;
            }
        }
    }
}
