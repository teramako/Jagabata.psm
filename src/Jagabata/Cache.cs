using System.Text;
using Jagabata.Resources;

namespace Jagabata;

public interface ICacheableResource : IResource
{
    CacheItem GetCacheItem();
}

public interface IHasCacheableItems
{
    IEnumerable<CacheItem> GetCacheableItems();
}

public enum CacheType
{
    Container, Summary
}

public class CacheItem(ResourceType type, ulong id, string name, string description,
                       CacheType cacheType = CacheType.Container)
    : IEquatable<CacheItem>
{
    public ResourceType Type { get; } = type;
    public ulong Id { get; } = id;
    public string Name { get; internal set; } = name;
    public string Description { get; set; } = description;
    public Dictionary<string, string> Metadata { get; internal set; } = [];
    public CacheType CacheType { get; } = cacheType;
    public DateTime CachedTimestamp { get; internal set; } = DateTime.Now;

    public bool Equals(CacheItem? other)
    {
        return other is not null && Type == other.Type && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as CacheItem);
    }

    public override int GetHashCode()
    {
        return ((int)Type).GetHashCode() ^ Id.GetHashCode();
    }
    /// <summary>
    /// Returns <paramref name="Type"/>:<paramref name="Id"/>:<paramref name="Name"/>
    /// or <paramref name="Type"/>:<paramref name="Id"/> (when <paramref name="Name"/> is empty)
    /// </summary>
    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? $"{Type}:{Id}" : $"{Type}:{Id}:{Name}";
    }
    /// <summary>
    /// Returns tooltip text for powershell <seealso cref="System.Management.Automation.CompletionResult"/>
    /// </summary>
    public string ToTooltip()
    {
        var sb = new StringBuilder($"[{Type}:{Id}]");
        var hasName = !string.IsNullOrWhiteSpace(Name);
        var hasDesc = !string.IsNullOrWhiteSpace(Description);
        if (hasName)
        {
            sb.Append(' ').Append(Name);
        }
        if (hasDesc)
        {
            sb.Append(' ');
            if (hasName)
                sb.Append("- ");
            sb.Append(Description);
        }
        if (hasDesc || hasName) sb.Append(' ');
        if (Metadata.Count > 0)
        {
            sb.Append('{');
            bool isWritten = false;
            foreach (var kv in Metadata)
            {
                if (isWritten) sb.Append(',');
                sb.Append(' ').Append(kv.Key).Append(" = ").Append(kv.Value);
                isWritten = true;
            }
            if (isWritten) sb.Append(' ');
            sb.Append('}');
        }
        return sb.ToString();
    }
}

public static class Caches
{
    private static readonly Lazy<List<CacheItem>> _items = new(static () => []);
    internal static List<CacheItem> Data => _items.Value;
    // FIXME: to be configurable
    public static int MAX_COUNT = 100;

    public static void Add(CacheItem item)
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

    private static IEnumerable<CacheItem> GetItems(IResource resource)
    {
        if (resource is ICacheableResource res)
        {
            yield return res.GetCacheItem();
        }
        if (resource is IHasCacheableItems c)
        {
            foreach (var child in c.GetCacheableItems())
            {
                yield return child;
            }
        }
    }
    private static IEnumerable<CacheItem> GetItems(IEnumerable<IResource> resources)
    {
        foreach (var resource in resources)
        {
            if (resource is ICacheableResource res)
            {
                yield return res.GetCacheItem();
            }
            if (resource is IHasCacheableItems c)
            {
                foreach (var child in c.GetCacheableItems())
                {
                    yield return child;
                }
            }
        }
    }

    public static void Add(IResource resource)
    {
        foreach (var item in new HashSet<CacheItem>(GetItems(resource)))
        {
            Add(item);
        }
    }

    public static void Add(IEnumerable<IResource> resources)
    {
        foreach (var item in new HashSet<CacheItem>(GetItems(resources)))
        {
            Add(item);
        }
    }

    public static void Clear()
    {
        Data.Clear();
    }

    public static IEnumerable<CacheItem> GetEnumerator(params ResourceType[] types)
    {
        return types is { Length: 0 } ? Data : Data.Where(item => types.Contains(item.Type));
    }
}
