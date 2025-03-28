using System.Diagnostics.CodeAnalysis;
using Jagabata.Resources;

namespace Jagabata
{
    public interface IResource
    {
        /// <summary>
        /// Database ID for the resource
        /// </summary>
        ulong Id { get; }
        /// <summary>
        /// Data type for the resource
        /// </summary>
        ResourceType Type { get; }
    }

    public record struct Resource(ResourceType Type, ulong Id) : IResource, IParsable<Resource>
    {
        public static Resource Parse(string s, IFormatProvider? provider = null)
        {
            return TryParse(s, provider, out var result)
                   ? result
                   : throw new FormatException($"Resource format should be '{{Type}}:{{Id}}': {s}");
        }

        private static readonly char[] separator = [':', '#'];
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resource result)
        {
            result = default;
            if (s is null)
            {
                return false;
            }
            var list = s.Split(separator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (list.Length > 1
                && Enum.TryParse<ResourceType>(list[0], true, out var resourceType)
                && ulong.TryParse(list[1], System.Globalization.NumberStyles.Integer, provider, out var id))
            {
                result = new Resource(resourceType, id);
                return true;
            }
            return false;
        }
        public override readonly string ToString()
        {
            return $"{Type}:{Id}";
        }
    }

    public abstract class ResourceBase : IResource, ICacheableResource, IHasCacheableItems
    {
        public abstract ulong Id { get; }
        public abstract ResourceType Type { get; }
        /// <summary>
        /// URL for this resource
        /// </summary>
        public abstract string Url { get; }
        /// <summary>
        /// Data structure with URLs of related resources.
        /// </summary>
        public abstract RelatedDictionary Related { get; }
        /// <summary>
        /// Data structure with name/description for related resources.
        /// The output for some objects may be limited for performance reasons.
        /// </summary>
        public abstract SummaryFieldsDictionary SummaryFields { get; }

        protected IEnumerable<T> GetResultsByRelatedKey<T>(string relatedKey, HttpQuery? query = null)
            where T : class
        {
            return Related.TryGetPath(relatedKey, out var path)
                ? RestAPI.GetResultSet<T>(path, query)
                         .SelectMany(static apiResult => apiResult.Contents.Results)
                : [];
        }

        protected IEnumerable<T> GetResultsByRelatedKey<T>(string relatedKey,
                                                           string? searchWords,
                                                           string orderBy = "",
                                                           ushort pageSize = 20,
                                                           uint page = 1)
            where T : class
        {
            return GetResultsByRelatedKey<T>(relatedKey, new QueryBuilder().SetSearchWords(searchWords)
                                                                           .SetOrderBy(orderBy)
                                                                           .SetPageSize(pageSize)
                                                                           .SetStartPage(page)
                                                                           .Build());
        }

        protected abstract CacheItem GetCacheItem();
        CacheItem ICacheableResource.GetCacheItem()
        {
            return GetCacheItem();
        }

        IEnumerable<CacheItem> IHasCacheableItems.GetCacheableItems()
        {
            foreach (var summaryItem in SummaryFields.Values)
            {
                switch (summaryItem)
                {
                    case Array arr:
                        foreach (var item in arr.OfType<ICacheableResource>())
                        {
                            yield return item.GetCacheItem();
                        }
                        continue;
                    case ListSummary<ICacheableResource> list:
                        foreach (var item in list.Results)
                        {
                            yield return item.GetCacheItem();
                        }
                        continue;
                    case ICacheableResource res:
                        yield return res.GetCacheItem();
                        continue;
                    default:
                        continue;
                }
            }
        }

        public override string ToString()
        {
            return $"{Type}:{Id}";
        }
    }
}
